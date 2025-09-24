using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CountryManager : MonoBehaviour, IClickable, ICardTarget
{
    public string countryName;    //name of the country
    public string description => $"{countryName} is of the {archetype.name} archetype.";    //description of the country
    public CountryArchetype archetype;

    public Material[] influenceMaterials = new Material[5];
    public Renderer countryRenderer;

    //List<GameObject> Tiles;
    //public SectorManager[] sectors; for later
    public SectorManager governmentSector;
    public SectorManager economicSector;
    public SectorManager mediaSector;
    public SectorManager socialSector;
    public SectorManager activismSector;

    public float influenceThreshold;
    public float totalInfluence;       //for the control bar, between 0 and 100
    public bool isInfluenced;          //if the country is influenced (aka claimed)
    Dictionary<PlayerManager, float> influencingPlayers;
    Dictionary<ApproachType, float> influencingApproaches;
    public CardManager cardSlot;   //a location card

    [Header("Clickable Interface Settings")]
    public Animation clickAnimation;
    public AudioSource clickAudio;

    public Outline countryOutline;

    //Propperties
    public Animation anim => clickAnimation;
    public AudioSource audioSource => clickAudio;
    
    // ICardTarget implementation
    public CardTargetType TargetType => CardTargetType.Country;
    public string TargetName => countryName;
    public CountryManager TargetCountryManager => this;
    public SectorManager TargetSectorManager => null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Setup()
    {
        name = "Country" + countryName;     //set the GameObject's name to the country's name
        totalInfluence = 0;
        cardSlot = null;
        influencingPlayers = new Dictionary<PlayerManager, float>();
        influencingApproaches = new Dictionary<ApproachType, float>();
        isInfluenced = false;

        if (countryRenderer == null) countryRenderer = GetComponent<Renderer>();
        if (influenceMaterials == null) influenceMaterials = new Material[5];
        countryRenderer.material = influenceMaterials[0];
        if(countryOutline == null) countryOutline = GetComponent<Outline>();
        countryOutline.effectColor = Color.black;
        countryOutline.effectDistance = new Vector2(2, 2);

        //Change Scale
        governmentSector.transform.localScale *= archetype.GetSectorWeight(ESectorType.Government) / 2;
        economicSector.transform.localScale *= archetype.GetSectorWeight(ESectorType.Economic) / 2;
    }

    public void UpdateCountryInfluence()
    {
        //Calculate total influence based on Sectors and archetype's weighting
        if (archetype == null)
        {
            Debug.LogWarning($"CountryManager UpdateCountryInfluence(): No archetype assigned for {countryName}");
            totalInfluence = 0f;
            return;
        }

        float weightedInfluence = 0f;
        float totalWeight = 0f;

        // Calculate weighted influence for each sector
        if (governmentSector != null)
        {
            int weight = archetype.GetSectorWeight(ESectorType.Government);
            weightedInfluence += governmentSector.currentInfluence * weight;
            totalWeight += weight;
            SetInfluencingPlayer(governmentSector, weight);
            SetInfluencingApproach(governmentSector, weight);
        }

        if (economicSector != null)
        {
            int weight = archetype.GetSectorWeight(ESectorType.Economic);
            weightedInfluence += economicSector.currentInfluence * weight;
            totalWeight += weight;
            SetInfluencingPlayer(economicSector, weight);
            SetInfluencingApproach(economicSector, weight);
        }

        if (mediaSector != null)
        {
            int weight = archetype.GetSectorWeight(ESectorType.Media);
            weightedInfluence += mediaSector.currentInfluence * weight;
            totalWeight += weight;
            SetInfluencingPlayer(mediaSector, weight);
            SetInfluencingApproach(mediaSector, weight);
        }

        if (socialSector != null)
        {
            int weight = archetype.GetSectorWeight(ESectorType.Social);
            weightedInfluence += socialSector.currentInfluence * weight;
            totalWeight += weight;
            SetInfluencingPlayer(socialSector, weight);
            SetInfluencingApproach (socialSector, weight);
        }

        if (activismSector != null)
        {
            int weight = archetype.GetSectorWeight(ESectorType.Activism);
            weightedInfluence += activismSector.currentInfluence * weight;
            totalWeight += weight;
            SetInfluencingPlayer(activismSector, weight);
            SetInfluencingApproach(activismSector, weight);
        }

        // Calculate final weighted average influence (0-100)
        if (totalWeight > 0)
        {
            totalInfluence = weightedInfluence / totalWeight;
        }
        else
        {
            totalInfluence = 0f;
        }

        // Clamp total influence to 0-100 range
        totalInfluence = Mathf.Clamp(totalInfluence, 0f, 100f);

        //Update Material by changing it to the next material in the array every 20%
        if (totalInfluence < 20)
        {
            countryRenderer.material = influenceMaterials[0];
        }
        else if (totalInfluence < 40)
        {
            countryRenderer.material = influenceMaterials[1];
        }
        else if (totalInfluence < 60)
        {
            countryRenderer.material = influenceMaterials[2];
        }
        else if (totalInfluence < 80)
        {
            countryRenderer.material = influenceMaterials[3];
        }
        else if (totalInfluence >= 80)
        {
            countryRenderer.material = influenceMaterials[4];
        }

        if (totalInfluence >= influenceThreshold) OnCountryInfluenced();
    }

    public void OnCountryInfluenced()
    {
        if (isInfluenced) return; // Prevent multiple triggers 
        isInfluenced = true;

        countryOutline.effectColor = GetTopInfluencingPlayer().playerColor;
        countryOutline.effectDistance = new Vector2(3, 3);

        //Approach.GetApproachColor(GetTopInfluencingApproach());
    }

    public void CustomClick(ClickDetector clicker)
    {
        PlayerManager player = clicker.GetComponent<PlayerManager>();
        if (player == null)
        {
            Debug.Log("SectorManager CustomClick(): was not clicked by a player.");
            return;
        }

        UIManager ui = clicker.GetComponentInChildren<UIManager>();
        if (ui == null) ui = player.uiManager;
        if (ui == null)
        {
            Debug.Log("CountryManager CustomClick(): Can not find ui manager.");
            return;
        }

        ui.ShowCountryUI(true, this);

        ICardTarget target = GetComponent<ICardTarget>();
        if(target != null)  //if this has a cardTarget component
        {
            if (player.handManager.HasCardSelected())   //if a card is selected
            {
                ui.ShowPlayCardUI(true, player.handManager.selectedCard, target);
            }
        }  
    }

    private void SetInfluencingPlayer(SectorManager sector, int weight)
    {
        if (sector == null) return;

        PlayerManager player = sector.GetTopInfluencingPlayer();
        if (player == null) return;

        float influence = sector.GetPlayerInfluence(player) * weight;

        if(influencingPlayers.ContainsKey(player))
        {
            influencingPlayers[player] += influence;
        }
        else
        {
            influencingPlayers.Add(player, influence);
        }
    }

    private void SetInfluencingApproach(SectorManager sector, int weight)
    {
        if (sector == null) return;

        ApproachType approach = sector.GetTopInfluencingApproach();
        if (approach == ApproachType.None) return;

        float influence = sector.GetApproachInfluence(approach) * weight;

        if (influencingApproaches.ContainsKey(approach))
        {
            influencingApproaches[approach] += influence;
        }
        else
        {
            influencingApproaches.Add(approach, influence);
        }
    }

    public PlayerManager GetTopInfluencingPlayer()
    {
        if (influencingPlayers == null || influencingPlayers.Count == 0) return null;
        
        return influencingPlayers.OrderByDescending(x => x.Value).First().Key;
    }

    public ApproachType GetTopInfluencingApproach()
    {
        if(influencingApproaches == null || influencingApproaches.Count == 0) return ApproachType.None;

        return influencingApproaches.OrderByDescending(x => x.Value).First().Key;
    }
    
    public bool CanReceiveCard(CardManager card)
    {
        return card.targetType == CardTargetType.Country;
    }
}
