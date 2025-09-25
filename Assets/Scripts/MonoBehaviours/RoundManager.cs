using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoundManager : MonoBehaviour
{
    // Events for notifying other systems about turn/round changes
    public delegate void NewTurnHandler(int roundNumber, int turnNumber);
    public static event NewTurnHandler OnNewTurn;

    public delegate void NewRoundHandler(int roundNumber);
    public static event NewRoundHandler OnNewRound;

    public delegate void NextPlayerHandler();       //might add (PlayerManager player) later
    public static event NextPlayerHandler OnNextPlayer;

    public PlayerManager[] players;
    public PlayerManager currentPlayer;
    public int currentPlayerIndex;

    [Header("Round Settings")]
    [SerializeField] private int totalRounds;
    [SerializeField] private int turnsPerRound;       //number of turns in each round, last turn is discussion phase
    [SerializeField] private int craftingTurns;       //number of turns in each round where players can craft cards
    [SerializeField] public float secondsPerTurn;

    [Header("Current Round State")]
    public float currentSeconds;    //how many seconds are left
    public int currentRound;
    public int currentTurn;

    public RoundState currentState;          //which actions are allowed in this round state
    private bool isTurnActive;
    private bool awaitingConfirm;            // true if a turn ended and we're waiting for confirm
    private bool isLastPlayerAwaitingAdvance; // remembers if the ended player was the last this cycle
    //private bool awaitingConfirm;            // true if the turn ended (by timer or early) and we're waiting for confirm
    public bool canCraft;                    //can players craft cards in this turn
    public bool canPlayActionCards;          //can players play action cards in this turn
    public bool canSpeak;                    //can players speak in this turn

    List<PlayerAction>[,] playerActions;     //All actions of all players in this game session
                                             //2D array where each element stores a PlayerAction list, row = turnNumber, column = rowNumber

    [Header("Managers")]
    public GameManager gameManager;
    public UIManager uiManager;

    [Header("Audio")]
    [SerializeField] private AudioSource uiAudioSource;
    [SerializeField] private AudioClip beepSound;
    private float beepTimer;
    // NextTurn -> StartPlayerTurn -> Timer ->EndPlayerTurn 
    //          -> StartPlayerTurn -> ... -> EndPlayerTurn 
    //          -> StartPlayerTurn --lastPlayer raeched--> NextTurn -> ... -> NextRound

    public void InitializeManagers(GameManager pGameManager, UIManager pUiManager, PlayerManager[] pPlayers)
    {
        gameManager = pGameManager;
        uiManager = pUiManager;
        players = pPlayers;
    }

    // Start is intentionally empty; GameManager initializes and calls Setup()
    void Start() 
    {
        Setup();
     }

    // Update is called once per frame
    void Update()
    {
        UpdateTurnTimer();
    }

    public void Setup()
    {
        //Settig Defaults
        playerActions = new List<PlayerAction>[totalRounds, turnsPerRound];
        currentRound = 1;
        currentTurn = 1;
        currentPlayerIndex = -1; // will increment to 0 on first StartPlayerTurn
        awaitingConfirm = false;
        isLastPlayerAwaitingAdvance = false;
        beepTimer = 0f;

        if (players == null || players.Length == 0)
        {
            Debug.LogError("RoundManager.Setup(): players not initialized.");
            return;
        }
        if (uiManager == null)
        {
            Debug.LogError("RoundManager.Setup(): uiManager not initialized.");
            return;
        }

        // Announce initial round and turn
        OnNewRound?.Invoke(currentRound);
        OnNewTurn?.Invoke(currentRound, currentTurn);

        StartPlayerTurn();
    }

    public void NextTurn()
    {
        currentTurn++;

        if (currentTurn < turnsPerRound - 1)     //not the last turn, so normal crafting/action phase
        {
            if(currentTurn <= craftingTurns) currentState = RoundState.Crafting;    //crafting phase
            else currentState = RoundState.Action;                                //action phase
        }
        else if (currentTurn == turnsPerRound) currentState = RoundState.Discussion; //last turn, meaning it is the discussion phase
        else        //currentTurn > turnsPerRound, meaning the round is over
        {
            NextRound();
            return;
        }

        if (uiManager != null) uiManager.EnableCraftButton(canCraft);

        // Announce the new turn (still within the same round)
        OnNewTurn?.Invoke(currentRound, currentTurn);

        currentPlayerIndex = -1;
        awaitingConfirm = false;
        isLastPlayerAwaitingAdvance = false;
        StartPlayerTurn();
    }

    private void NextRound()
    {
        currentTurn = 0;    //because next turn incriments current turn
        currentRound++;
        //if (currentRound >= totalRounds)   //game over

        // Announce new round and the first turn of that round
        OnNewRound?.Invoke(currentRound);
        if (uiManager != null) uiManager.UpdateRound(currentRound);
        NextTurn();
    }

    public void StartPlayerTurn()
    {
        //Next player
        if (players == null || players.Length == 0)
        {
            Debug.LogError("RoundManager.StartPlayerTurn(): players not initialized.");
            return;
        }

        // advance to next player (no wrap logic here; EndPlayerTurn decides when to advance the round)
        currentPlayerIndex++;
        if (currentPlayerIndex >= players.Length) currentPlayerIndex = 0;
        currentPlayer = players[currentPlayerIndex];

        //enable all player actions
        //currentPlayer.EnableInput(true);
        currentSeconds = secondsPerTurn;
        isTurnActive = true;
        awaitingConfirm = false;
        beepTimer = 0f;
        Time.timeScale = 1f;    //resume time

        //Update UI
        if (uiManager != null)
        {
            uiManager.multiplayerUI.UpdatePlayers(currentPlayerIndex);
            uiManager.UpdateUIPerTurn();
            uiManager.EnableConfirmButton(true);
        }

        OnNextPlayer?.Invoke();
    }

    // Called by the Confirm button
    public void OnConfirmPressed()
    {
        // If the turn is still active, end it now (early end)
        if (isTurnActive)
        {
            EndPlayerTurn();
        }

        // If we're not awaiting confirm (e.g., invalid state), do nothing
        if (!awaitingConfirm) return;

        // Decide where to go next now that player confirmed end
        bool wasLastPlayer = isLastPlayerAwaitingAdvance;
        awaitingConfirm = false;
        isLastPlayerAwaitingAdvance = false;
        if (wasLastPlayer)
        {
            NextTurn();
        }
        else
        {
            StartPlayerTurn();
        }
    }

    private void EndPlayerTurn()
    {
        //disable all player actions other than clicking the Confirm button
        //currentPlayer.GetComponent<PlayerInput>().actions.FindActionMap("Player").Disable();
        if (uiManager != null)
        {
            uiManager.EnableCraftButton(false);
            uiManager.EnableConfirmButton(true); // keep confirm visible to proceed
        }
        isTurnActive = false;
        awaitingConfirm = true; // wait for button press to advance
        isLastPlayerAwaitingAdvance = players != null && players.Length > 0 && currentPlayerIndex == players.Length - 1;
        Time.timeScale = 0f;    //pause time
        beepTimer = 0f;
    }

    private void SetState(RoundState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case RoundState.Crafting:
                canCraft = true;
                canPlayActionCards = true;
                canSpeak = false;
                break;
            case RoundState.Action:
                canCraft = false;
                canPlayActionCards = true;
                canSpeak = false;
                break;
            case RoundState.Discussion:
                canCraft = false;
                canPlayActionCards = false;
                canSpeak = true;
                break;
        }
    }

    private void UpdateTurnTimer()
    {
        if (!isTurnActive) return;
        if (secondsPerTurn <= 0f) return;

        currentSeconds -= Time.deltaTime;
        if (uiManager != null) uiManager.UpdateTimer(currentSeconds);

        if(currentSeconds <= 6)
        {
            // Play beep every 1 second while turn is active and less than 5 seconds
            if (uiAudioSource != null && beepSound != null)
            {
                beepTimer += Time.deltaTime;
                if (beepTimer >= 1f)
                {
                    beepTimer -= 1f;
                    uiAudioSource.PlayOneShot(beepSound);
                }
            }
        }


        if (currentSeconds <= 0f)
        {
            currentSeconds = 0f;
            // Time is up: end the turn but wait for player to confirm to advance
            if (isTurnActive)
            {
                EndPlayerTurn();
            }
        }
    }
}

public enum RoundState
{
    Crafting,   //players can craft cards and play action cards
    Action,     //players can only play action cards
    Discussion  //players can now talk
}