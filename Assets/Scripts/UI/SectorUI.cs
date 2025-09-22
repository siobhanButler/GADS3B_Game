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
    
    [Header("Card Slots")]
    [SerializeField] private Image cardSlot1Image;      //Location card or multi-turn card can be played here
    [SerializeField] private Image cardSlot2Image;
    
    [Header("Recent Actions Panel")]
    [SerializeField] private GameObject recentActionsPanel;
    [SerializeField] private TextMeshProUGUI sectorsText;
    [SerializeField] private TextMeshProUGUI recentAction1Text;
    [SerializeField] private TextMeshProUGUI recentAction1DescrText;
    [SerializeField] private TextMeshProUGUI recentAction2Text;
    [SerializeField] private TextMeshProUGUI recentAction2DescrText;
    [SerializeField] private TextMeshProUGUI recentAction3Text;
    [SerializeField] private TextMeshProUGUI recentAction3DescrText;
    
    private void Awake()
    {
        InitializeReferences();
    }
    
    private void InitializeReferences()
    {
        sectorPanel = transform.Find("Sector_Panel")?.gameObject;
        
        if (sectorPanel != null)
        {
            sectorNameText = sectorPanel.transform.Find("SectorName_Text")?.GetComponent<TextMeshProUGUI>();
            countryText = sectorPanel.transform.Find("Country_Text")?.GetComponent<TextMeshProUGUI>();
            sectorInfluenceSlider = sectorPanel.transform.Find("SectorInfluence_Slider")?.GetComponent<Slider>();
            resourcesObject = sectorPanel.transform.Find("Resources")?.gameObject;
            
            cardSlot1Image = sectorPanel.transform.Find("CardSlot1_Img")?.GetComponent<Image>();
            cardSlot2Image = sectorPanel.transform.Find("CardSlot2_Img")?.GetComponent<Image>();
            
            InitializeRecentActionsReferences();
        }
    }
    
    private void InitializeRecentActionsReferences()
    {
        recentActionsPanel = sectorPanel.transform.Find("RecentActions_Panel")?.gameObject;
        
        if (recentActionsPanel != null)
        {
            sectorsText = recentActionsPanel.transform.Find("Sectors")?.GetComponent<TextMeshProUGUI>();
            
            Transform action1Transform = recentActionsPanel.transform.Find("RecentAction1_Text");
            if (action1Transform != null)
            {
                recentAction1Text = action1Transform.GetComponent<TextMeshProUGUI>();
                recentAction1DescrText = action1Transform.Find("RecentAction1Descr_Text")?.GetComponent<TextMeshProUGUI>();
            }
            
            Transform action2Transform = recentActionsPanel.transform.Find("RecentAction2_Text");
            if (action2Transform != null)
            {
                recentAction2Text = action2Transform.GetComponent<TextMeshProUGUI>();
                recentAction2DescrText = action2Transform.Find("RecentAction2Descr_Text")?.GetComponent<TextMeshProUGUI>();
            }
            
            Transform action3Transform = recentActionsPanel.transform.Find("RecentAction3_Text");
            if (action3Transform != null)
            {
                recentAction3Text = action3Transform.GetComponent<TextMeshProUGUI>();
                recentAction3DescrText = action3Transform.Find("RecentAction3Descr_Text")?.GetComponent<TextMeshProUGUI>();
            }
        }
    }
    
    public void ShowSectorPanel(bool show)
    {
        if (sectorPanel != null)
            sectorPanel.SetActive(show);
    }

    public void UpdateSectorUI()
    {
        if (selectedSector == null) return;

        sectorNameText.text = selectedSector.sectorName;
        countryText.text = selectedSector.country.countryName;
        sectorInfluenceSlider.value = selectedSector.currentInfluence;

        if (selectedSector.cardSlot1 != null) cardSlot1Image.sprite = selectedSector.cardSlot1.cardImage;
        if (selectedSector.cardSlot2 != null) cardSlot2Image.sprite = selectedSector.cardSlot2.cardImage;

        recentAction1Text.text = selectedSector.playerActions[0].GetActionMessage();
        recentAction1DescrText.text = selectedSector.playerActions[0].card.description;
        recentAction2Text.text = selectedSector.playerActions[1].GetActionMessage();
        recentAction2DescrText.text = selectedSector.playerActions[1].card.description;
        recentAction3Text.text = selectedSector.playerActions[2].GetActionMessage();
        recentAction3DescrText.text = selectedSector.playerActions[2].card.description;
    }

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
    
    public void SetSectorsText(string sectorsInfo)
    {
        if (sectorsText != null)
            sectorsText.text = sectorsInfo;
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
}