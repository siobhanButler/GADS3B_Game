using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
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
    [SerializeField] private MultiplayerUI multiplayerUI;
    [SerializeField] private CountryUI countryUI;
    [SerializeField] private SectorUI sectorUI;
    [SerializeField] private PlayerHandUI playerHandUI;
    
    [Header("Other Panels")]
    [SerializeField] private GameObject playCardPanel;
    [SerializeField] private Button playCardYesButton;
    [SerializeField] private Button playCardNoButton;
    
    private void Awake()
    {
        InitializeReferences();
        SetupButtonListeners();
    }
    
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
        craftButton = transform.Find("Craft_Button")?.GetComponent<Button>();
        confirmButton = transform.Find("Confirm_Button")?.GetComponent<Button>();
        
        GameObject playPanel = transform.Find("PlayCard_Panel")?.gameObject;
        if (playPanel != null)
        {
            playCardPanel = playPanel;
            playCardYesButton = playPanel.transform.Find("Yes_Button")?.GetComponent<Button>();
            playCardNoButton = playPanel.transform.Find("No_Button")?.GetComponent<Button>();
        }
    }
    
    private void InitializeOtherManagers()
    {
        multiplayerUI = GetComponent<MultiplayerUI>();
        countryUI = GetComponent<CountryUI>();
        sectorUI = GetComponent<SectorUI>();
        playerHandUI = GetComponent<PlayerHandUI>();
    }
    
    private void SetupButtonListeners()
    {
        if (craftButton != null)
            craftButton.onClick.AddListener(OnCraftButtonClicked);
        
        if (confirmButton != null)
            confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        
        if (playCardYesButton != null)
            playCardYesButton.onClick.AddListener(OnPlayCardYesClicked);
        
        if (playCardNoButton != null)
            playCardNoButton.onClick.AddListener(OnPlayCardNoClicked);
    }
    
    public void UpdatePersonalResources(int knowledge, int media, int legitimacy, int money, int labour, int solidarity)
    {
        if (personalKnowledgeText != null) personalKnowledgeText.text = knowledge.ToString();
        if (personalMediaText != null) personalMediaText.text = media.ToString();
        if (personalLegitimacyText != null) personalLegitimacyText.text = legitimacy.ToString();
        if (personalMoneyText != null) personalMoneyText.text = money.ToString();
        if (personalLabourText != null) personalLabourText.text = labour.ToString();
        if (personalSolidarityText != null) personalSolidarityText.text = solidarity.ToString();
    }
    
    public void UpdateSharedResources(int knowledge, int media, int legitimacy, int money, int labour, int solidarity)
    {
        if (sharedKnowledgeText != null) sharedKnowledgeText.text = knowledge.ToString();
        if (sharedMediaText != null) sharedMediaText.text = media.ToString();
        if (sharedLegitimacyText != null) sharedLegitimacyText.text = legitimacy.ToString();
        if (sharedMoneyText != null) sharedMoneyText.text = money.ToString();
        if (sharedLabourText != null) sharedLabourText.text = labour.ToString();
        if (sharedSolidarityText != null) sharedSolidarityText.text = solidarity.ToString();
    }
    
    public void UpdateTimer(string timerValue)
    {
        if (timerText != null)
            timerText.text = timerValue;
    }
    
    public void UpdateRound(int round)
    {
        if (roundText != null)
            roundText.text = $"Round {round}";
    }
    
    public void UpdateTurn(int turn)
    {
        if (turnText != null)
            turnText.text = $"Turn {turn}";
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
    
    public void SetPlayerImage(Sprite sprite)
    {
        if (playerImage != null)
            playerImage.sprite = sprite;
    }
    
    public void ShowPlayCardPanel(bool show)
    {
        if (playCardPanel != null)
            playCardPanel.SetActive(show);
    }
    
    private void OnCraftButtonClicked()
    {
        Debug.Log("Craft button clicked");
    }
    
    private void OnConfirmButtonClicked()
    {
        Debug.Log("Confirm button clicked");
    }
    
    private void OnPlayCardYesClicked()
    {
        Debug.Log("Play card Yes button clicked");
        ShowPlayCardPanel(false);
    }
    
    private void OnPlayCardNoClicked()
    {
        Debug.Log("Play card No button clicked");
        ShowPlayCardPanel(false);
    }
}