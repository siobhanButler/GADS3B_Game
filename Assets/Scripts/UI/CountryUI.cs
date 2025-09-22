using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountryUI : MonoBehaviour
{
    [Header("Country Panel Components")]
    [SerializeField] private GameObject countryPanel;
    [SerializeField] private TextMeshProUGUI countryNameText;
    [SerializeField] private TextMeshProUGUI archetypeText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Slider countryInfluenceSlider;
    [SerializeField] private Image countryCardImage;
    
    [Header("Country Sectors Panel")]
    [SerializeField] private GameObject countrySectorsPanel;
    [SerializeField] private TextMeshProUGUI sectorsText;
    
    [Header("Sector Sliders")]
    [SerializeField] private GameObject governmentSector;
    [SerializeField] private Slider governmentSectorSlider;
    [SerializeField] private GameObject economicSector;
    [SerializeField] private Slider economicSectorSlider;
    [SerializeField] private GameObject mediaSector;
    [SerializeField] private Slider mediaSectorSlider;
    [SerializeField] private GameObject socialSector;
    [SerializeField] private Slider socialSectorSlider;
    [SerializeField] private GameObject activismSector;
    [SerializeField] private Slider activismSectorSlider;
    
    private void Awake()
    {
        InitializeReferences();
    }
    
    private void InitializeReferences()
    {
        countryPanel = transform.Find("Country_Panel")?.gameObject;
        
        if (countryPanel != null)
        {
            countryNameText = countryPanel.transform.Find("CountryName_Text")?.GetComponent<TextMeshProUGUI>();
            archetypeText = countryPanel.transform.Find("Archetype_Text")?.GetComponent<TextMeshProUGUI>();
            countryInfluenceSlider = countryPanel.transform.Find("CountryInfluence_Slider")?.GetComponent<Slider>();
            countryCardImage = countryPanel.transform.Find("Card1_Img")?.GetComponent<Image>();
            
            Transform descriptionTransform = countryPanel.transform.Find("Description");
            if (descriptionTransform != null)
            {
                descriptionText = descriptionTransform.Find("Description_Text")?.GetComponent<TextMeshProUGUI>();
            }
            
            countrySectorsPanel = countryPanel.transform.Find("CountrySectors_Panel")?.gameObject;
            
            if (countrySectorsPanel != null)
            {
                InitializeSectorReferences();
            }
        }
    }
    
    private void InitializeSectorReferences()
    {
        sectorsText = countrySectorsPanel.transform.Find("Sectors")?.GetComponent<TextMeshProUGUI>();
        
        governmentSector = countrySectorsPanel.transform.Find("GovernmentSector")?.gameObject;
        if (governmentSector != null)
        {
            governmentSectorSlider = governmentSector.transform.Find("GovernmentSectorSlider")?.GetComponent<Slider>();
        }
        
        economicSector = countrySectorsPanel.transform.Find("EconomicSector")?.gameObject;
        if (economicSector != null)
        {
            economicSectorSlider = economicSector.transform.Find("EconomicSectorSlider")?.GetComponent<Slider>();
        }
        
        mediaSector = countrySectorsPanel.transform.Find("MediaSector")?.gameObject;
        if (mediaSector != null)
        {
            mediaSectorSlider = mediaSector.transform.Find("MediaSectorSlider")?.GetComponent<Slider>();
        }
        
        socialSector = countrySectorsPanel.transform.Find("SocialSector")?.gameObject;
        if (socialSector != null)
        {
            socialSectorSlider = socialSector.transform.Find("SocialSectorSlider")?.GetComponent<Slider>();
        }
        
        activismSector = countrySectorsPanel.transform.Find("ActivismSector")?.gameObject;
        if (activismSector != null)
        {
            activismSectorSlider = activismSector.transform.Find("ActivismSectorSlider")?.GetComponent<Slider>();
        }
    }
    
    public void ShowCountryPanel(bool show)
    {
        if (countryPanel != null)
            countryPanel.SetActive(show);
    }
    
    public void SetCountryName(string countryName)
    {
        if (countryNameText != null)
            countryNameText.text = countryName;
    }
    
    public void SetArchetype(string archetype)
    {
        if (archetypeText != null)
            archetypeText.text = archetype;
    }
    
    public void SetDescription(string description)
    {
        if (descriptionText != null)
            descriptionText.text = description;
    }
    
    public void SetCountryInfluence(float influence)
    {
        if (countryInfluenceSlider != null)
            countryInfluenceSlider.value = influence;
    }
    
    public void SetCountryCardImage(Sprite cardSprite)
    {
        if (countryCardImage != null)
            countryCardImage.sprite = cardSprite;
    }
    
    public void SetGovernmentSectorInfluence(float influence)
    {
        if (governmentSectorSlider != null)
            governmentSectorSlider.value = influence;
    }
    
    public void SetEconomicSectorInfluence(float influence)
    {
        if (economicSectorSlider != null)
            economicSectorSlider.value = influence;
    }
    
    public void SetMediaSectorInfluence(float influence)
    {
        if (mediaSectorSlider != null)
            mediaSectorSlider.value = influence;
    }
    
    public void SetSocialSectorInfluence(float influence)
    {
        if (socialSectorSlider != null)
            socialSectorSlider.value = influence;
    }
    
    public void SetActivismSectorInfluence(float influence)
    {
        if (activismSectorSlider != null)
            activismSectorSlider.value = influence;
    }
    
    public void UpdateAllSectorInfluences(float government, float economic, float media, float social, float activism)
    {
        SetGovernmentSectorInfluence(government);
        SetEconomicSectorInfluence(economic);
        SetMediaSectorInfluence(media);
        SetSocialSectorInfluence(social);
        SetActivismSectorInfluence(activism);
    }
    
    public void ShowSector(string sectorName, bool show)
    {
        GameObject sector = GetSectorByName(sectorName);
        if (sector != null)
            sector.SetActive(show);
    }
    
    public void ShowAllSectors(bool show)
    {
        if (governmentSector != null) governmentSector.SetActive(show);
        if (economicSector != null) economicSector.SetActive(show);
        if (mediaSector != null) mediaSector.SetActive(show);
        if (socialSector != null) socialSector.SetActive(show);
        if (activismSector != null) activismSector.SetActive(show);
    }
    
    public float GetSectorInfluence(string sectorName)
    {
        Slider slider = GetSectorSliderByName(sectorName);
        return slider != null ? slider.value : 0f;
    }
    
    public GameObject GetSectorByName(string sectorName)
    {
        return sectorName.ToLower() switch
        {
            "government" => governmentSector,
            "economic" => economicSector,
            "media" => mediaSector,
            "social" => socialSector,
            "activism" => activismSector,
            _ => null
        };
    }
    
    public Slider GetSectorSliderByName(string sectorName)
    {
        return sectorName.ToLower() switch
        {
            "government" => governmentSectorSlider,
            "economic" => economicSectorSlider,
            "media" => mediaSectorSlider,
            "social" => socialSectorSlider,
            "activism" => activismSectorSlider,
            _ => null
        };
    }
    
    public GameObject GetCountryPanel()
    {
        return countryPanel;
    }
    
    public float GetCountryInfluence()
    {
        return countryInfluenceSlider != null ? countryInfluenceSlider.value : 0f;
    }
}