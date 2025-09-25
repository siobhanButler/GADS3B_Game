using TMPro;
using TreeEditor;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class UIManager : MonoBehaviour
{
    public GameManager gameManager;
    public RoundManager roundManager;
    public PlayerManager[] players;
    public PlayerManager currentPlayer;

    public Resource personalResources;
    public ResourceManager sharedResources;

    [Header("Top Panel References")]
    [SerializeField] private GameObject topPanel;
    [SerializeField] private GameObject personalResourcesPanel;
    [SerializeField] private GameObject sharedResourcesPanel;
    [SerializeField] private GameObject timerPanel;
    [SerializeField] private GameObject winConditionsPanel;
    [SerializeField] private Image playerImage;

    [Header("Personal Resources")]
    [SerializeField] private TextMeshProUGUI personalKnowledgeText;
    [SerializeField] private TextMeshProUGUI personalMediaText;
    [SerializeField] private TextMeshProUGUI personalLegitimacyText;
    [SerializeField] private TextMeshProUGUI personalMoneyText;
    [SerializeField] private TextMeshProUGUI personalLabourText;
    [SerializeField] private TextMeshProUGUI personalSolidarityText;

    [Header("Shared Resources")]
    [SerializeField] private TextMeshProUGUI sharedKnowledgeText;
    [SerializeField] private TextMeshProUGUI sharedMediaText;
    [SerializeField] private TextMeshProUGUI sharedLegitimacyText;
    [SerializeField] private TextMeshProUGUI sharedMoneyText;
    [SerializeField] private TextMeshProUGUI sharedLabourText;
    [SerializeField] private TextMeshProUGUI sharedSolidarityText;
    
    [Header("Timer Components")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private TextMeshProUGUI turnText;
    
    [Header("Sliders")]
    [SerializeField] private Slider peacefulSlider;
    [SerializeField] private Slider violentSlider;
    
    [Header("Buttons")]
    [SerializeField] private Button craftButton;
    [SerializeField] private Button confirmButton;
    
    [Header("Other UI Managers")]
    [SerializeField] public MultiplayerUI multiplayerUI;
    [SerializeField] private CountryUI countryUI;
    [SerializeField] private SectorUI sectorUI;
    [SerializeField] public PlayerHandUI playerHandUI;
    
    [Header("Crafting UI")]
    [SerializeField] private GameObject craftingUIPanel;
    [SerializeField] private Button closeCraftingUIButton;
    
    [Header("Other Panels")]

    // Cached resource strings to reduce allocations
    private string cachedPersonalKnowledge = "";
    private string cachedPersonalMedia = "";
    private string cachedPersonalLegitimacy = "";
    private string cachedPersonalMoney = "";
    private string cachedPersonalLabour = "";
    private string cachedPersonalSolidarity = "";
    
    private string cachedSharedKnowledge = "";
    private string cachedSharedMedia = "";
    private string cachedSharedLegitimacy = "";
    private string cachedSharedMoney = "";
    private string cachedSharedLabour = "";
    private string cachedSharedSolidarity = "";
    
    private string cachedRoundText = "";
    private string cachedTurnText = "";
    private string cachedTimerText = "";

    public void InitializeManagers(GameManager pGameManager, RoundManager pRoundManager, PlayerManager[] pPlayers, ResourceManager pSharedResources)
    {
        gameManager = pGameManager;
        roundManager = pRoundManager;
        players = pPlayers;
        sharedResources = pSharedResources;
    }
    
    private void Awake()
    {
        InitializeReferences();
        SetupButtonListeners();
    }

    private void Start()
    {
        playerHandUI.ShowPlayCardPanel(false, null, null);
        sectorUI.ShowSectorPanel(false, null);
        countryUI.ShowCountryPanel(false, null);
        ShowCraftingUI(false);
    }

    public void ShowSectorUI(bool show, SectorManager sector)
    {
        if (sectorUI == null)
        {
            Debug.Log("UIManager ShowSectorUI(): sector UI is null.");
            return;
        }
        //Debug.Log($"UIManager ShowSectorUI(): Showing sector UI: {show}, Sector: {sector?.sectorName}");
        sectorUI.ShowSectorPanel(show, sector);

        // If showing crafting UI, hide other panels to avoid overlap
        if (show)
        {
            ShowCountryUI(false, null);
            ShowCraftingUI(false);
        }
    }

    public void ShowCountryUI(bool show, CountryManager country)
    {
        if (countryUI == null)
        {
            Debug.Log("UIManager ShowCountryUI(): country UI is null.");
            return;
        }
        //Debug.Log($"UIManager ShowCountryUI(): Showing country UI: {show}, Country: {country?.countryName}");
        countryUI.ShowCountryPanel(show, country);

        // If showing crafting UI, hide other panels to avoid overlap
        if (show)
        {
            ShowSectorUI(false, null);
            ShowCraftingUI(false);
        }
    }

    public void ShowPlayCardUI(bool show, CardManager card, ICardTarget target)
    {
        if (playerHandUI == null)
        {
            Debug.Log("UIManager ShowPlayCardUI(): PlayerHandUI UI is null.");
            return;
        }
        //Debug.Log($"UIManager ShowPlayCardUI(): Showing play card UI: {show}, Card: {card?.cardName}");
        playerHandUI.ShowPlayCardPanel(show, card, target);

        if (show)
        {
            ShowCraftingUI(false);
        }
    }

    public void ShowCraftingUI(bool show)
    {
        if (craftingUIPanel == null)
        {
            Debug.Log("UIManager ShowCraftingUI(): Crafting UI panel is null.");
            return;
        }
        
        Debug.Log($"UIManager ShowCraftingUI(): Showing crafting UI: {show}");
        craftingUIPanel.SetActive(show);
        
        // If showing crafting UI, hide other panels to avoid overlap
        if (show)
        {
            ShowSectorUI(false, null);
            ShowCountryUI(false, null);
            ShowPlayCardUI(false, null, null);
        }
    }

    public void UpdateUIPerTurn()
    {
        currentPlayer = roundManager.currentPlayer;

        UpdateTimer(roundManager.secondsPerTurn);
        UpdateRound(roundManager.currentRound);
        UpdateTurn(roundManager.currentTurn);
        craftButton.interactable = roundManager.canCraft;

        UpdatePersonalResources();
        UpdateSharedResources();

        UpdatePeacefulSlider(gameManager.peacefulPoints / gameManager.maxPeacefulPoints);
        UpdateViolentSlider(gameManager.violentPoints / gameManager.maxViolentPoints);

        multiplayerUI.UpdatePlayers(roundManager.currentPlayerIndex);
    }

    public void UpdateResourcesUI()
    {
        UpdatePersonalResources();
        UpdateSharedResources();
    }

    public void UpdatePersonalResources()
    {
        if (currentPlayer == null) return;
        
        // Only update and allocate strings when values change
        string newKnowledge = currentPlayer.personalResources.knowledge.ToString();
        if (cachedPersonalKnowledge != newKnowledge)
        {
            cachedPersonalKnowledge = newKnowledge;
            if (personalKnowledgeText != null) personalKnowledgeText.text = cachedPersonalKnowledge;
        }
        
        string newMedia = currentPlayer.personalResources.media.ToString();
        if (cachedPersonalMedia != newMedia)
        {
            cachedPersonalMedia = newMedia;
            if (personalMediaText != null) personalMediaText.text = cachedPersonalMedia;
        }
        
        string newLegitimacy = currentPlayer.personalResources.legitimacy.ToString();
        if (cachedPersonalLegitimacy != newLegitimacy)
        {
            cachedPersonalLegitimacy = newLegitimacy;
            if (personalLegitimacyText != null) personalLegitimacyText.text = cachedPersonalLegitimacy;
        }
        
        string newMoney = currentPlayer.personalResources.money.ToString();
        if (cachedPersonalMoney != newMoney)
        {
            cachedPersonalMoney = newMoney;
            if (personalMoneyText != null) personalMoneyText.text = cachedPersonalMoney;
        }
        
        string newLabour = currentPlayer.personalResources.labour.ToString();
        if (cachedPersonalLabour != newLabour)
        {
            cachedPersonalLabour = newLabour;
            if (personalLabourText != null) personalLabourText.text = cachedPersonalLabour;
        }
        
        string newSolidarity = currentPlayer.personalResources.solidarity.ToString();
        if (cachedPersonalSolidarity != newSolidarity)
        {
            cachedPersonalSolidarity = newSolidarity;
            if (personalSolidarityText != null) personalSolidarityText.text = cachedPersonalSolidarity;
        }
    }

    public void UpdateSharedResources()
    {
        if (sharedResources == null) return;
        if (currentPlayer == null) return;
        
        // Only update and allocate strings when values change
        string newKnowledge = sharedResources.resources.knowledge.ToString();
        if (cachedSharedKnowledge != newKnowledge)
        {
            cachedSharedKnowledge = newKnowledge;
            if (sharedKnowledgeText != null) sharedKnowledgeText.text = cachedSharedKnowledge;
        }
        
        string newMedia = sharedResources.resources.media.ToString();
        if (cachedSharedMedia != newMedia)
        {
            cachedSharedMedia = newMedia;
            if (sharedMediaText != null) sharedMediaText.text = cachedSharedMedia;
        }
        
        string newLegitimacy = sharedResources.resources.legitimacy.ToString();
        if (cachedSharedLegitimacy != newLegitimacy)
        {
            cachedSharedLegitimacy = newLegitimacy;
            if (sharedLegitimacyText != null) sharedLegitimacyText.text = cachedSharedLegitimacy;
        }
        
        string newMoney = sharedResources.resources.money.ToString();
        if (cachedSharedMoney != newMoney)
        {
            cachedSharedMoney = newMoney;
            if (sharedMoneyText != null) sharedMoneyText.text = cachedSharedMoney;
        }
        
        string newLabour = sharedResources.resources.labour.ToString();
        if (cachedSharedLabour != newLabour)
        {
            cachedSharedLabour = newLabour;
            if (sharedLabourText != null) sharedLabourText.text = cachedSharedLabour;
        }
        
        string newSolidarity = sharedResources.resources.solidarity.ToString();
        if (cachedSharedSolidarity != newSolidarity)
        {
            cachedSharedSolidarity = newSolidarity;
            if (sharedSolidarityText != null) sharedSolidarityText.text = cachedSharedSolidarity;
        }
    }

    public void UpdateTimer(float currentSeconds)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentSeconds / 60f);
            int seconds = Mathf.FloorToInt(currentSeconds % 60f);
            string newTimerText = string.Format("{0:00}:{1:00}", minutes, seconds);
            
            if (cachedTimerText != newTimerText)
            {
                cachedTimerText = newTimerText;
                timerText.text = cachedTimerText;
            }

            if (currentSeconds <= 5)
            {
                timerText.color = Color.red;
            }
            else timerText.color = Color.black;
        }
    }
    
    public void UpdateRound(int round)
    {
        if (roundText != null)
        {
            string newRoundText = round.ToString();
            if (cachedRoundText != newRoundText)
            {
                cachedRoundText = newRoundText;
                roundText.text = cachedRoundText;
            }
        }
    }
    
    public void UpdateTurn(int turn)
    {
        if (turnText != null)
        {
            string newTurnText = turn.ToString();
            if (cachedTurnText != newTurnText)
            {
                cachedTurnText = newTurnText;
                turnText.text = cachedTurnText;
            }
        }
    }
    
    public void UpdatePeacefulSlider(float value)
    {
        if (peacefulSlider != null)
            peacefulSlider.value = value;
    }
    
    public void UpdateViolentSlider(float value)
    {
        if (violentSlider != null)
            violentSlider.value = value;
    }

// ============= BUTTON LISTENERS =============

    private void OnCraftButtonClicked()
    {
        Debug.Log("Craft button clicked");
        ShowCraftingUI(true);
    }
    
    private void OnCloseCraftingUIButtonClicked()
    {
        Debug.Log("Close crafting UI button clicked");
        ShowCraftingUI(false);
    }
    
    private void OnConfirmButtonClicked()
    {
        Debug.Log("Confirm button clicked");
        roundManager.OnConfirmPressed();
    }

    public void EnableCraftButton(bool enable)
    {
        if (craftButton != null)
            craftButton.interactable = enable;
    }

    public void EnableConfirmButton(bool enable)
    {
        if (confirmButton != null)
            confirmButton.interactable = enable;
    }


    // ============= INITIALIZING AND SETUP FUNCTIONS =============

    private void InitializeReferences()
    {
        if (topPanel == null)
            topPanel = transform.Find("TopPanel")?.gameObject;

        if (topPanel != null)
        {
            personalResourcesPanel = topPanel.transform.Find("PersonalResources")?.gameObject;
            sharedResourcesPanel = topPanel.transform.Find("SharedResources")?.gameObject;
            timerPanel = topPanel.transform.Find("Timer_Panel")?.gameObject;
            winConditionsPanel = topPanel.transform.Find("WinConditions_Panel")?.gameObject;
            playerImage = topPanel.transform.Find("Image_Player")?.GetComponent<Image>();

            peacefulSlider = topPanel.transform.Find("Peaceful_Slider")?.GetComponent<Slider>();
            violentSlider = topPanel.transform.Find("Violent_Slider")?.GetComponent<Slider>();
        }

        InitializeResourceTexts();
        InitializeTimerTexts();
        InitializeButtons();
        InitializeOtherManagers();
    }

    private void InitializeResourceTexts()
    {
        if (personalResourcesPanel != null)
        {
            personalKnowledgeText = personalResourcesPanel.transform.Find("Knowledge_Text")?.GetComponent<TextMeshProUGUI>();
            personalMediaText = personalResourcesPanel.transform.Find("Media_Text")?.GetComponent<TextMeshProUGUI>();
            personalLegitimacyText = personalResourcesPanel.transform.Find("Legitimacy_Text")?.GetComponent<TextMeshProUGUI>();
            personalMoneyText = personalResourcesPanel.transform.Find("Money_Text")?.GetComponent<TextMeshProUGUI>();
            personalLabourText = personalResourcesPanel.transform.Find("Labour_Text")?.GetComponent<TextMeshProUGUI>();
            personalSolidarityText = personalResourcesPanel.transform.Find("Solidarity_Text")?.GetComponent<TextMeshProUGUI>();
        }

        if (sharedResourcesPanel != null)
        {
            sharedKnowledgeText = sharedResourcesPanel.transform.Find("Knowledge_Text")?.GetComponent<TextMeshProUGUI>();
            sharedMediaText = sharedResourcesPanel.transform.Find("Media_Text")?.GetComponent<TextMeshProUGUI>();
            sharedLegitimacyText = sharedResourcesPanel.transform.Find("Legitimacy_Text")?.GetComponent<TextMeshProUGUI>();
            sharedMoneyText = sharedResourcesPanel.transform.Find("Money_Text")?.GetComponent<TextMeshProUGUI>();
            sharedLabourText = sharedResourcesPanel.transform.Find("Labour_Text")?.GetComponent<TextMeshProUGUI>();
            sharedSolidarityText = sharedResourcesPanel.transform.Find("Solidarity_Text")?.GetComponent<TextMeshProUGUI>();
        }
    }

    private void InitializeTimerTexts()
    {
        if (timerPanel != null)
        {
            timerText = timerPanel.transform.Find("Timer_text")?.GetComponent<TextMeshProUGUI>();
            roundText = timerPanel.transform.Find("Round_text")?.GetComponent<TextMeshProUGUI>();
            turnText = timerPanel.transform.Find("Turn_text")?.GetComponent<TextMeshProUGUI>();
        }
    }

    private void InitializeButtons()
    {
        if(craftButton == null) craftButton = transform.Find("Craft_Button")?.GetComponent<Button>();
        if (confirmButton == null) confirmButton = transform.Find("Confirm_Button")?.GetComponent<Button>();
        
        // Initialize crafting UI components
        if (craftingUIPanel == null) craftingUIPanel = transform.Find("CraftingUI_Panel")?.gameObject;
        if (closeCraftingUIButton == null) closeCraftingUIButton = craftingUIPanel?.transform.Find("Close_Button")?.GetComponent<Button>();
    }

    private void InitializeOtherManagers()
    {
        if (multiplayerUI == null) multiplayerUI = GetComponentInChildren<MultiplayerUI>();
        if (countryUI == null) countryUI = GetComponentInChildren<CountryUI>();
        if (sectorUI == null) sectorUI = GetComponentInChildren<SectorUI>();
        if (playerHandUI == null) playerHandUI = GetComponentInChildren<PlayerHandUI>();
    }

    private void SetupButtonListeners()
    {
        if (craftButton != null)
            craftButton.onClick.AddListener(OnCraftButtonClicked);

        if (confirmButton != null)
            confirmButton.onClick.AddListener(OnConfirmButtonClicked);
            
        if (closeCraftingUIButton != null)
            closeCraftingUIButton.onClick.AddListener(OnCloseCraftingUIButtonClicked);
    }
}