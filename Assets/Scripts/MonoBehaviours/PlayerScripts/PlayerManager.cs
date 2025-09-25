using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Info")]
    public string playerName;
    public int playerID;
    public Color playerColor;
    public ApproachType preferredApproach;

    public List<PlayerAction> previousActions = new List<PlayerAction>(); //The actions that the player has taken in previous turns
    public bool tookActionThisTurn;
    private int cardsCraftedThisTurn;

    public HandManager handManager;
    public PlayerInput playerInput;

    [Header("Resources")]
    public Resource personalResources;
    public ResourceManager sharedResources;

    public List<SectorManager> influencedSectors = new List<SectorManager>(); //The sections that the player controls/posesses

    [Header("Managers")]
    public GameManager gameManager;
    public UIManager uiManager;

    public void Initialize(GameManager pGameManager, UIManager pUiManager, string pPlayerName, int pPlayerIndex, Color pPlayerColor, ApproachType pPreferredApproach, Resource pStartingResources, ResourceManager pSharedResources)
    {
        gameManager = pGameManager;
        uiManager = pUiManager; //later, each player will have their own instance

        playerName = pPlayerName;
        name = "Player " + playerName;
        playerID = pPlayerIndex;
        playerColor = pPlayerColor;
        preferredApproach = pPreferredApproach;

        personalResources = pStartingResources;
        sharedResources = pSharedResources;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup()
    {
        tookActionThisTurn = false;
        
        if(handManager == null) handManager = GetComponent<HandManager>();
            if(handManager == null) handManager = gameObject.AddComponent<HandManager>();
    }

    public void SelectCard(CardManager card)
    {
        handManager.SelectCard(card);

        //logic to grey out all Game objects that it can not be played on
    }

    public void PlayCard(CardManager card, ICardTarget target)
    {
        if(tookActionThisTurn) //if an action has already been taken this turn, do not play a card
        {
            Debug.Log($"PlayerManager.PlayCard(): {playerName} has already taken an action this turn, do not play a card"); 
            return;  
        }

        // Validate inputs
        if (card == null)
        {
            Debug.LogError("PlayerManager.PlayCard(): card is null!");
            return;
        }
        
        if (target == null)
        {
            Debug.LogError("PlayerManager.PlayCard(): target is null!");
            return;
        }
        
        Debug.Log($"PlayerManager.PlayCard(): Playing card '{card.cardName}' on target '{target.TargetName}'");
        
        PlayerAction action = new PlayerAction();
        action.SetPlayerAction(ActionType.Craft, this, card);
        previousActions.Add(action);

        card.ApplyCardEffect(target, this);
    }

    public void AddCraftedCard(CardManager card)
    {
        if(cardsCraftedThisTurn >= 3 && tookActionThisTurn) //if player has already crafted 3 cards this turn and has already taken an action this turn, do not craft card
        {
            Debug.Log($"PlayerManager.PlayCard(): {playerName} has already crafted 3 cards this turn, do not craft a card"); 
            return;  
        }

        Debug.Log($"PlayerManager.AddCraftedCard(): Adding card '{card.cardName}' to {playerName}'s hand");
        handManager.AddCard(card);

        PlayerAction action = new PlayerAction();
        action.SetPlayerAction(ActionType.Craft, this, card);
        previousActions.Add(action);
        
        Debug.Log($"PlayerManager.AddCraftedCard(): Card '{card.cardName}' successfully added. Hand now has {handManager.hand.Count} cards");

        uiManager.playerHandUI.UpdateHandUI(this);

        cardsCraftedThisTurn++;
        tookActionThisTurn = true;  //crafting card counts as an action, so player can not play a card this turn, but can craft up to 3 cards
    }

    public void OnNewTurn()
    {
        cardsCraftedThisTurn = 0;
        tookActionThisTurn = false;
    }

    public void EnableInput(bool enable)
    {
        if(playerInput == null) playerInput = GetComponent<PlayerInput>();
        if(playerInput != null) playerInput.enabled = enable;
    }
}
