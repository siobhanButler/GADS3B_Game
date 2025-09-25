using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public RoundManager roundManager;
    public UIManager uiManager;
    public EventManager eventManager;

    public ResourceManager sharedResources;
    public PlayerManager[] players;
    public CountryManager[] countries;
    public SectorManager[] sectors;

    public float maxPeacefulPoints;
    public float peacefulPoints;
    public float maxViolentPoints;
    public float violentPoints;

    public float resourcesToTopInfluencingPlayer;
    public float resourcesToLastInfluencingPlayer;

    [SerializeField] private int maxPlayers;
    [SerializeField] private Resource startingResources;
    [SerializeField] private string[] playerNames;
    [SerializeField] private Color[] playerColors;
    [SerializeField] private ApproachType[] playerApproaches;
    [SerializeField] private ApproachType[] unevenPlayerApproaches;

    public GameObject gameOverPanel;
    public TextMeshProUGUI messageText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Setup();
        InitializeMembers();
    }

    void SetPlayerUp()
    {

    }

    // Start is left empty to avoid double-initialization
    void Start() {
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Setup()
    {
        if (roundManager == null) roundManager = FindFirstObjectByType<RoundManager>();
        if (uiManager == null) uiManager = FindFirstObjectByType<UIManager>();
        if (eventManager == null) eventManager = FindFirstObjectByType<EventManager>();

        countries = FindObjectsByType<CountryManager>(FindObjectsSortMode.None);
        sectors = FindObjectsByType<SectorManager>(FindObjectsSortMode.None);
        if(players == null) players = FindObjectsByType<PlayerManager>(FindObjectsSortMode.None);
    }

    void InitializeMembers()
    {
        if (roundManager == null)
        {
            Debug.LogError("GameManager.InitializeMembers(): roundManager missing");
            return;
        }
        if (uiManager == null)
        {
            Debug.LogError("GameManager.InitializeMembers(): uiManager missing");
            return;
        }
        if (players == null || players.Length == 0)
        {
            Debug.LogError("GameManager.InitializeMembers(): players not found");
            return;
        }

        roundManager.InitializeManagers(this, uiManager, players);
        uiManager.InitializeManagers(this, roundManager, players, sharedResources);
        eventManager.Initialize(this);

        foreach (PlayerManager player in players)
        {
            int index = System.Array.IndexOf(players, player);
            if (index < 0 || index >= playerNames.Length || index >= playerColors.Length)
            {
                Debug.LogError($"GameManager.InitializeMembers(): index {index} out of range for player meta arrays.");
                continue;
            }
            System.Random random = new System.Random();
            ApproachType approach = playerApproaches[random.Next(0, playerApproaches.Length-1)];  //randomly choose one of the approach types

            player.Initialize(this, uiManager, playerNames[index], index, playerColors[index], approach, startingResources, sharedResources);
        }
        foreach (CountryManager country in countries)
        {
            country.Initialize(this);
        }
        foreach (SectorManager sector in sectors)
        {
            sector.Initialize(this);
        }

        uiManager.multiplayerUI.Setup(players);
        sharedResources.resources = startingResources;
    }

   public void CheckWinLoseConditions()
    {
        if (peacefulPoints+violentPoints > maxPeacefulPoints + maxViolentPoints / 1.5)  //too polarized
        {
            ShowGameEndPanel("You Loose! \n The movement became too polarized.");
        }
        else if(sharedResources.resources.GetTotalValue() <= 0)                             //no resources left
        {
            ShowGameEndPanel("You Loose! \n You ran out of resources.");
        }

        int influencedCountries = 0;
        foreach (CountryManager country in countries)
        {
            if (country.isInfluenced) influencedCountries++;
        }
        if(influencedCountries > countries.Length/2)
        {
            ShowGameEndPanel("You Win! You influenced more than half of the countries!");
        }

        int influencedSectors = 0;
        foreach (SectorManager sector in sectors)
        {
            if (sector.isInfluenced) influencedSectors++;
        }

        if (influencedSectors > sectors.Length/2)
        {
            ShowGameEndPanel("You Win! You influenced more than half of the sectors!");
        }
    }

    private void ShowGameEndPanel(string message)
    {
        roundManager.EndPlayerTurn();
        gameOverPanel.SetActive(true);
        messageText.text = message + "\n \n Round: " + roundManager.currentRound + "Turns: " + roundManager.currentTurn;
    }
}
