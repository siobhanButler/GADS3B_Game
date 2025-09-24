using UnityEngine;
using UnityEngine.UI;
using TMPro;

//To add: if you click on one of the sectors, it will show the sector UI for that sector

public class CountryUI : MonoBehaviour
{
    CountryManager selectedCountry;

    [Header("Country Panel Components")]
    [SerializeField] private GameObject countryPanel;
    [SerializeField] private TextMeshProUGUI countryNameText;
    [SerializeField] private TextMeshProUGUI archetypeText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Slider countryInfluenceSlider;
    [SerializeField] private Image countryCardImage;
    [SerializeField] private Button closeCountryUIButton;
    
    [Header("Country Sectors Panel")]
    [SerializeField] private GameObject countrySectorsPanel;
    
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
        SetupButtonListeners();
    }
    
    public void ShowCountryPanel(bool show, CountryManager country)
    {
        if (countryPanel != null)
            countryPanel.SetActive(show);
        else 
            Debug.Log("CountryUI ShowCountryPanel(): no country panel");

        selectedCountry = country;
        UpdateCountryUI();
    }

    public void UpdateCountryUI()
    {
        if (selectedCountry == null)
        {
            Debug.Log("CountryUI UpdateCountryUI: no selected country.");
            return;
        }
        UpdateCountryInfo();
        UpdateCountryCard();
        UpdateSectorInfluences();
    }

    public void UpdateCountryInfo()
    {
        if (selectedCountry == null) return;
        if (countryNameText != null) try { countryNameText.text = selectedCountry.countryName; } 
                                        catch { countryNameText.text = ""; }
        if (archetypeText != null)  try { archetypeText.text = selectedCountry.archetype.name; } 
                                        catch { archetypeText.text = ""; }
        if (descriptionText != null) try { descriptionText.text = selectedCountry.description; } 
                                        catch { descriptionText.text = ""; }
        if (countryInfluenceSlider != null) countryInfluenceSlider.value = selectedCountry.totalInfluence;
    }

    public void UpdateCountryCard()
    {
        if (selectedCountry == null) return;
        if (countryCardImage != null && selectedCountry.cardSlot != null)
            countryCardImage.sprite = selectedCountry.cardSlot.cardSprite;
    }

    public void UpdateSectorInfluences()
    {
        if (selectedCountry == null) return;

        //Get the sector's influence and set the slider value and fill rect color with sector's top influencing player's color
        if (governmentSectorSlider != null && selectedCountry.governmentSector != null)
            governmentSectorSlider.value = selectedCountry.governmentSector.currentInfluence;
            try { governmentSectorSlider.fillRect.GetComponent<Image>().color = selectedCountry.governmentSector.GetTopInfluencingPlayer().playerColor; }
                catch { governmentSectorSlider.fillRect.GetComponent<Image>().color = Color.beige; }

        if (economicSectorSlider != null && selectedCountry.economicSector != null)
            economicSectorSlider.value = selectedCountry.economicSector.currentInfluence;
            try { economicSectorSlider.fillRect.GetComponent<Image>().color = selectedCountry.economicSector.GetTopInfluencingPlayer().playerColor; }
                catch { economicSectorSlider.fillRect.GetComponent<Image>().color = Color.beige; }

        if (mediaSectorSlider != null && selectedCountry.mediaSector != null)
            mediaSectorSlider.value = selectedCountry.mediaSector.currentInfluence;
            try { mediaSectorSlider.fillRect.GetComponent<Image>().color = selectedCountry.mediaSector.GetTopInfluencingPlayer().playerColor; }
                catch { mediaSectorSlider.fillRect.GetComponent<Image>().color = Color.beige; }

        if (socialSectorSlider != null && selectedCountry.socialSector != null)
            socialSectorSlider.value = selectedCountry.socialSector.currentInfluence;
            try { socialSectorSlider.fillRect.GetComponent<Image>().color = selectedCountry.socialSector.GetTopInfluencingPlayer().playerColor; }
                catch { socialSectorSlider.fillRect.GetComponent<Image>().color = Color.beige; }

        if (activismSectorSlider != null && selectedCountry.activismSector != null)
            activismSectorSlider.value = selectedCountry.activismSector.currentInfluence;
            try { activismSectorSlider.fillRect.GetComponent<Image>().color = selectedCountry.activismSector.GetTopInfluencingPlayer().playerColor; }
                catch { activismSectorSlider.fillRect.GetComponent<Image>().color = Color.beige; }
    }

    private void InitializeReferences()
    {
        if (countryPanel == null)
            countryPanel = transform.Find("Country_Panel")?.gameObject;

        if (countryPanel != null)
        {
            if (countryNameText == null) countryNameText = countryPanel.transform.Find("CountryName_Text")?.GetComponent<TextMeshProUGUI>();
            if (archetypeText == null) archetypeText = countryPanel.transform.Find("Archetype_Text")?.GetComponent<TextMeshProUGUI>();
            if (countryInfluenceSlider == null) countryInfluenceSlider = countryPanel.transform.Find("CountryInfluence_Slider")?.GetComponent<Slider>();
            if (countryCardImage == null) countryCardImage = countryPanel.transform.Find("Card1_Img")?.GetComponent<Image>();
            if (closeCountryUIButton == null) closeCountryUIButton = countryPanel.transform.Find("CloseCountryUI_Button")?.GetComponent<Button>();

            Transform descriptionTransform = countryPanel.transform.Find("Description");
            if (descriptionTransform != null)
            {
                if (descriptionText == null) descriptionText = descriptionTransform.Find("Description_Text")?.GetComponent<TextMeshProUGUI>();
            }

            if (countrySectorsPanel == null) countrySectorsPanel = countryPanel.transform.Find("CountrySectors_Panel")?.gameObject;

            if (countrySectorsPanel != null)
            {
                //InitializeSectorReferences();
                if (governmentSector == null) governmentSector = countrySectorsPanel.transform.Find("GovernmentSector")?.gameObject;
                if (governmentSector != null)
                {
                    if (governmentSectorSlider == null) governmentSectorSlider = governmentSector.transform.Find("GovernmentSectorSlider")?.GetComponent<Slider>();
                }

                if (economicSector == null) economicSector = countrySectorsPanel.transform.Find("EconomicSector")?.gameObject;
                if (economicSector != null)
                {
                    if (economicSectorSlider == null) economicSectorSlider = economicSector.transform.Find("EconomicSectorSlider")?.GetComponent<Slider>();
                }

                if (mediaSector == null) mediaSector = countrySectorsPanel.transform.Find("MediaSector")?.gameObject;
                if (mediaSector != null)
                {
                    if (mediaSectorSlider == null) mediaSectorSlider = mediaSector.transform.Find("MediaSectorSlider")?.GetComponent<Slider>();
                }

                if (socialSector == null) socialSector = countrySectorsPanel.transform.Find("SocialSector")?.gameObject;
                if (socialSector != null)
                {
                    if (socialSectorSlider == null) socialSectorSlider = socialSector.transform.Find("SocialSectorSlider")?.GetComponent<Slider>();
                }

                if (activismSector == null) activismSector = countrySectorsPanel.transform.Find("ActivismSector")?.gameObject;
                if (activismSector != null)
                {
                    if (activismSectorSlider == null) activismSectorSlider = activismSector.transform.Find("ActivismSectorSlider")?.GetComponent<Slider>();
                }
            }
        }
    }

    private void SetupButtonListeners()
    {
        if (closeCountryUIButton != null)
            closeCountryUIButton.onClick.AddListener(OnCloseCountryUIClicked);
    }

    private void OnCloseCountryUIClicked()
    {
        Debug.Log("Close Country UI button clicked");
        ShowCountryPanel(false, null);
    }
}

/*
    public void SetCountryName(string countryName)
    {
        if (countryNameText != null)
            try { countryNameText.text = countryName; } catch { countryNameText.text = ""; }
    }
    
    public void SetArchetype(string archetype)
    {
        if (archetypeText != null)
            try { archetypeText.text = archetype; } catch { archetypeText.text = ""; }
    }
    
    public void SetDescription(string description)
    {
        if (descriptionText != null)
            try { descriptionText.text = description; } catch { descriptionText.text = ""; }
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
 
 */