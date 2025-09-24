using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SectorUI : MonoBehaviour
{
    public SectorManager selectedSector;

    [Header("Sector Panel Components")]
    [SerializeField] private GameObject sectorPanel;
    [SerializeField] private TextMeshProUGUI sectorNameText;
    [SerializeField] private TextMeshProUGUI countryText;
    [SerializeField] private Slider sectorInfluenceSlider;
    [SerializeField] private GameObject resourcesObject;
    [SerializeField] private Button closeSectorUIButton;
    
    [Header("Card Slots")]
    [SerializeField] private Image cardSlot1Image;      //Location card or multi-turn card can be played here
    [SerializeField] private Image cardSlot2Image;

    [Header("Granted Resources")]
    [SerializeField] private TextMeshProUGUI knowledgeText;
    [SerializeField] private TextMeshProUGUI mediaText;
    [SerializeField] private TextMeshProUGUI legitimacyText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI labourText;
    [SerializeField] private TextMeshProUGUI solidarityText;

    [Header("Recent Actions Panel")]
    [SerializeField] private GameObject recentActionsPanel;
    [SerializeField] private TextMeshProUGUI recentAction1Text;
    [SerializeField] private TextMeshProUGUI recentAction1DescrText;
    [SerializeField] private TextMeshProUGUI recentAction2Text;
    [SerializeField] private TextMeshProUGUI recentAction2DescrText;
    [SerializeField] private TextMeshProUGUI recentAction3Text;
    [SerializeField] private TextMeshProUGUI recentAction3DescrText;
    
    private void Awake()
    {
        InitializeReferences();
        SetupButtonListeners();
    }
    
    public void ShowSectorPanel(bool show, SectorManager sector)
    {
        if (sectorPanel != null)
            sectorPanel.SetActive(show);
        selectedSector = sector;
        UpdateSectorUI();
    }

    public void UpdateSectorUI()
    {
        if (selectedSector == null) return;

        UpdateSectorInfo();
        UpdateSectorCards();
        UpdateRecentActions();
        UpdateSectorResources();
        UpdateSectorInfluence();
    }

    public void UpdateSectorInfo()
    {
        if (selectedSector == null) return;
        try { sectorNameText.text = selectedSector.sectorName; }
            catch { sectorNameText.text = ""; }
        try { countryText.text = selectedSector.country.countryName; }
            catch { countryText.text = ""; }
        try { sectorInfluenceSlider.value = selectedSector.currentInfluence; }
            catch { sectorInfluenceSlider.value = 0;}
    }

    public void UpdateSectorCards()
    {
        if (selectedSector == null) return;
        if (selectedSector.cardSlot1 != null) cardSlot1Image.sprite = selectedSector.cardSlot1.cardSprite;
        if (selectedSector.cardSlot2 != null) cardSlot2Image.sprite = selectedSector.cardSlot2.cardSprite;
    }

    public void UpdateRecentActions()
    {
        if (selectedSector == null) return;
        try { recentAction1Text.text = selectedSector.playerActions[0].GetActionMessage(); }
            catch { recentAction1Text.text = ""; }
        try { recentAction1DescrText.text = selectedSector.playerActions[0].card.description; } 
            catch { recentAction1DescrText.text = ""; }
        try { recentAction2Text.text = selectedSector.playerActions[1].GetActionMessage(); } 
            catch { recentAction2Text.text = ""; }
        try { recentAction2DescrText.text = selectedSector.playerActions[1].card.description; } 
            catch { recentAction2DescrText.text = ""; }
        try { recentAction3Text.text = selectedSector.playerActions[2].GetActionMessage(); } 
            catch { recentAction3Text.text = ""; }
        try { recentAction3DescrText.text = selectedSector.playerActions[2].card.description; } 
            catch { recentAction3DescrText.text = ""; }
    }
    public void UpdateSectorResources()
    {
        if (selectedSector == null) return;
        if (knowledgeText != null) try { knowledgeText.text = selectedSector.grantedResources.knowledge.ToString(); } 
                                    catch { knowledgeText.text = ""; }
        if (mediaText != null) try { mediaText.text = selectedSector.grantedResources.media.ToString(); } 
                                    catch { mediaText.text = ""; }
        if (legitimacyText != null) try { legitimacyText.text = selectedSector.grantedResources.legitimacy.ToString(); } 
                                        catch { legitimacyText.text = ""; }
        if (moneyText != null) try { moneyText.text = selectedSector.grantedResources.money.ToString(); } 
                                    catch { moneyText.text = ""; }
        if (labourText != null) try { labourText.text = selectedSector.grantedResources.labour.ToString(); } 
                                    catch { labourText.text = ""; }
        if (solidarityText != null) try { solidarityText.text = selectedSector.grantedResources.solidarity.ToString(); } 
                                        catch { solidarityText.text = ""; }
    }
    
    public void UpdateSectorInfluence()
    {
        if (selectedSector == null) return;
        if (sectorInfluenceSlider != null) sectorInfluenceSlider.value = selectedSector.currentInfluence;
        sectorInfluenceSlider.fillRect.GetComponent<Image>().color = selectedSector.GetTopInfluencingPlayer().playerColor;  //set the color of the fill rect to the top influencing player's color
    }

    private void InitializeReferences()
    {
        if (sectorPanel == null)
            sectorPanel = transform.Find("Sector_Panel")?.gameObject;

        if (sectorPanel != null)
        {
            if (sectorNameText == null) sectorNameText = sectorPanel.transform.Find("SectorName_Text")?.GetComponent<TextMeshProUGUI>();
            if (countryText == null) countryText = sectorPanel.transform.Find("Country_Text")?.GetComponent<TextMeshProUGUI>();
            if (sectorInfluenceSlider == null) sectorInfluenceSlider = sectorPanel.transform.Find("SectorInfluence_Slider")?.GetComponent<Slider>();
            if (resourcesObject == null) resourcesObject = sectorPanel.transform.Find("Resources")?.gameObject;

            if (cardSlot1Image == null) cardSlot1Image = sectorPanel.transform.Find("CardSlot1_Img")?.GetComponent<Image>();
            if (cardSlot2Image == null) cardSlot2Image = sectorPanel.transform.Find("CardSlot2_Img")?.GetComponent<Image>();
            if (closeSectorUIButton == null) closeSectorUIButton = sectorPanel.transform.Find("CloseSectorUI_Button")?.GetComponent<Button>();

            //InitializeRecentActionsReferences
            if (recentActionsPanel == null) recentActionsPanel = sectorPanel.transform.Find("RecentActions_Panel")?.gameObject;

            if (recentActionsPanel != null)
            {
                Transform action1Transform = recentActionsPanel.transform.Find("RecentAction1_Text");
                if (action1Transform != null)
                {
                    if (recentAction1Text == null) recentAction1Text = action1Transform.GetComponent<TextMeshProUGUI>();
                    if (recentAction1DescrText == null) recentAction1DescrText = action1Transform.Find("RecentAction1Descr_Text")?.GetComponent<TextMeshProUGUI>();
                }

                Transform action2Transform = recentActionsPanel.transform.Find("RecentAction2_Text");
                if (action2Transform != null)
                {
                    if (recentAction2Text == null) recentAction2Text = action2Transform.GetComponent<TextMeshProUGUI>();
                    if (recentAction2DescrText == null) recentAction2DescrText = action2Transform.Find("RecentAction2Descr_Text")?.GetComponent<TextMeshProUGUI>();
                }

                Transform action3Transform = recentActionsPanel.transform.Find("RecentAction3_Text");
                if (action3Transform != null)
                {
                    if (recentAction3Text == null) recentAction3Text = action3Transform.GetComponent<TextMeshProUGUI>();
                    if (recentAction3DescrText == null) recentAction3DescrText = action3Transform.Find("RecentAction3Descr_Text")?.GetComponent<TextMeshProUGUI>();
                }
            }
        }
    }

    private void SetupButtonListeners()
    {
        if (closeSectorUIButton != null)
            closeSectorUIButton.onClick.AddListener(OnCloseSectorUIClicked);
    }

    private void OnCloseSectorUIClicked()
    {
        Debug.Log("Close Sector UI button clicked");
        ShowSectorPanel(false, null);
    }
}

/*
        public void SetSectorName(string sectorName)
    {
        if (sectorNameText != null)
            sectorNameText.text = sectorName;
    }
    
    public void SetCountryName(string countryName)
    {
        if (countryText != null)
            countryText.text = countryName;
    }
    
    public void SetSectorInfluence(float influence)
    {
        if (sectorInfluenceSlider != null)
            sectorInfluenceSlider.value = influence;
    }
    
    public void SetCardSlot1(Sprite cardSprite)
    {
        if (cardSlot1Image != null)
            cardSlot1Image.sprite = cardSprite;
    }
    
    public void SetCardSlot2(Sprite cardSprite)
    {
        if (cardSlot2Image != null)
            cardSlot2Image.sprite = cardSprite;
    }
    
    public void ClearCardSlot1()
    {
        if (cardSlot1Image != null)
            cardSlot1Image.sprite = null;
    }
    
    public void ClearCardSlot2()
    {
        if (cardSlot2Image != null)
            cardSlot2Image.sprite = null;
    }
    
    public void ShowResources(bool show)
    {
        if (resourcesObject != null)
            resourcesObject.SetActive(show);
    }

    public void SetRecentAction1(string actionTitle, string actionDescription)
    {
        if (recentAction1Text != null)
            recentAction1Text.text = actionTitle;
        
        if (recentAction1DescrText != null)
            recentAction1DescrText.text = actionDescription;
    }
    
    public void SetRecentAction2(string actionTitle, string actionDescription)
    {
        if (recentAction2Text != null)
            recentAction2Text.text = actionTitle;
        
        if (recentAction2DescrText != null)
            recentAction2DescrText.text = actionDescription;
    }
    
    public void SetRecentAction3(string actionTitle, string actionDescription)
    {
        if (recentAction3Text != null)
            recentAction3Text.text = actionTitle;
        
        if (recentAction3DescrText != null)
            recentAction3DescrText.text = actionDescription;
    }
    
    public void UpdateAllRecentActions(string[] actionTitles, string[] actionDescriptions)
    {
        if (actionTitles.Length >= 1 && actionDescriptions.Length >= 1)
            SetRecentAction1(actionTitles[0], actionDescriptions[0]);
        
        if (actionTitles.Length >= 2 && actionDescriptions.Length >= 2)
            SetRecentAction2(actionTitles[1], actionDescriptions[1]);
        
        if (actionTitles.Length >= 3 && actionDescriptions.Length >= 3)
            SetRecentAction3(actionTitles[2], actionDescriptions[2]);
    }
    
    public void ClearAllRecentActions()
    {
        SetRecentAction1("", "");
        SetRecentAction2("", "");
        SetRecentAction3("", "");
    }
    
    public void ShowRecentActionsPanel(bool show)
    {
        if (recentActionsPanel != null)
            recentActionsPanel.SetActive(show);
    }
    
    public float GetSectorInfluence()
    {
        return sectorInfluenceSlider != null ? sectorInfluenceSlider.value : 0f;
    }
    
    public string GetSectorName()
    {
        return sectorNameText != null ? sectorNameText.text : "";
    }
    
    public string GetCountryName()
    {
        return countryText != null ? countryText.text : "";
    }
    
    public GameObject GetSectorPanel()
    {
        return sectorPanel;
    }
    
    public GameObject GetRecentActionsPanel()
    {
        return recentActionsPanel;
    }
    
    public Image GetCardSlot1Image()
    {
        return cardSlot1Image;
    }
    
    public Image GetCardSlot2Image()
    {
        return cardSlot2Image;
    }
 
 */