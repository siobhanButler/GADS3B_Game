using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

//Add Later: 
//resources are given to top influencing players (based on most recent influence)
//influence can be lost over time through time decay and events
//influence is given on a per round basis

public class SectorManager : MonoBehaviour, IClickable, ICardTarget
{
    public SectorType sectorType;
    public Resource grantedResources;   //resources granted by this sector
    public int totalResources;         //total resources in this sector

    public CountryManager country;

    public float influenceThreshold;    //min influence until sector is influenced/claimed
    public float currentInfluence;     //influence in this sector (out of 100)
    public bool isInfluenced;          //if the sector is influenced (aka claimed)
    public Dictionary<ApproachType, float> approachInfluence = new Dictionary<ApproachType, float>();

    public Dictionary<PlayerManager, float> playerInfluence = new Dictionary<PlayerManager, float>();

    public PlayerAction[] playerActions = new PlayerAction[3]; //cards/actions played in this sector, most recent will be displayed in UI
    public CardManager cardSlot1;
    public CardManager cardSlot2;

    [Header("Clickable Interface Settings")]
    public Animation clickAnimation;
    public AudioSource clickAudio;

    public Light sectorLight;

    //IClickable implimentation
    public Animation anim => clickAnimation;
    public AudioSource audioSource => clickAudio;
    public string sectorName => sectorType.ToString(); //name of the sector, derived from sectorType
    // ICardTarget implementation
    public CardTargetType TargetType => CardTargetType.Country;
    public string TargetName => sectorName;
    public CountryManager TargetCountryManager => null;
    public SectorManager TargetSectorManager => this;
    string description => $"This is the {sectorName} sector in {country.countryName}." + sectorType.Description; //description of the sector, can be expanded later

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
        isInfluenced = false;
        currentInfluence = 0f;
        playerInfluence = new Dictionary<PlayerManager, float>();
        playerActions = new PlayerAction[3];

        if (sectorLight == null) sectorLight = GetComponent<Light>();
        sectorLight.enabled = false;
        
        // Validate that sectorType is assigned
        if (sectorType == null)
        {
            Debug.LogError($"SectorManager Setup(): No SectorType assigned to {gameObject.name}!");
            return;
        }

        // Set default total resources if not already set
        if (totalResources <= 0) totalResources = 100; // Default base resources

        // Calculate granted resources based on SectorType breakdown
        Resource baseResource = new Resource(totalResources, totalResources, totalResources, totalResources, totalResources, totalResources);
        grantedResources = new Resource(
            Mathf.FloorToInt(baseResource.knowledge * sectorType.ResourceBreakdown.knowledgePercentage / 100f),
            Mathf.FloorToInt(baseResource.money * sectorType.ResourceBreakdown.moneyPercentage / 100f),
            Mathf.FloorToInt(baseResource.media * sectorType.ResourceBreakdown.mediaPercentage / 100f),
            Mathf.FloorToInt(baseResource.labour * sectorType.ResourceBreakdown.labourPercentage / 100f),
            Mathf.FloorToInt(baseResource.solidarity * sectorType.ResourceBreakdown.solidarityPercentage / 100f),
            Mathf.FloorToInt(baseResource.legitimacy * sectorType.ResourceBreakdown.legitimacyPercentage / 100f)
        );

        // Validate resource breakdown
        if (!sectorType.ResourceBreakdown.IsValid())
        {
            Debug.LogWarning($"SectorManager Setup(): Resource breakdown for {sectorType.SectorName} doesn't total 100%! " +
                           $"Current total: {sectorType.ResourceBreakdown.GetTotalPercentage()}%");
        }

        Debug.Log($"SectorManager Setup(): {sectorType.SectorName} sector initialized with {totalResources} total resources");
    }

    public void AddRecentAction(PlayerAction action)
    {
        //add most recent action to the index 0, move all other up by one and forget the last one
        playerActions[2] = playerActions[1];
        playerActions[1] = playerActions[0];
        playerActions[0] = action; 
    }

    public void CustomClick(ClickDetector clicker)  //displays Sector Ui
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
            Debug.Log("SectorManager CustomClick(): Can not find ui manager.");
            return;
        }

        ui.ShowSectorUI(true, this);

        ICardTarget target = GetComponent<ICardTarget>();
        if(target != null)  //if this has a cardTarget component
        {
            if (player.handManager.HasCardSelected() && player.handManager.selectedCard.targetType == CardTargetType.Sector)   //if a card is selected
            {
                ui.ShowPlayCardUI(true, player.handManager.selectedCard, target);
            }
        }  
    }

    public void AddInfluence(float influence, PlayerManager player, ApproachType approach)
    {
        if(currentInfluence >= 100f) return;

        if(currentInfluence + influence > 100f)
        {
            //find out how much of the influence will be added
            float remainingInfluence = 100f - currentInfluence;
            influence = remainingInfluence;
        }

        //add the influence to the sector
        currentInfluence += influence;
        currentInfluence = Mathf.Clamp(currentInfluence, 0f, 100f);
        country.UpdateCountryInfluence();

        //add the influence to the player
        if(playerInfluence.ContainsKey(player))
        {
            playerInfluence[player] += influence;
            playerInfluence[player] = Mathf.Clamp(playerInfluence[player], 0f, 100f);
        }
        else
        {
            playerInfluence.Add(player, influence);
            playerInfluence[player] = Mathf.Clamp(playerInfluence[player], 0f, 100f);
        }

        //add the influence to the approach type
        if (approachInfluence.ContainsKey(approach))
        {
            approachInfluence[approach] += influence;
            approachInfluence[approach] = Mathf.Clamp(approachInfluence[approach], 0f, 100f);
        }
        else
        {
            approachInfluence.Add(approach, influence);
            approachInfluence[approach] = Mathf.Clamp(approachInfluence[approach], 0f, 100f);
        }

        //If Influence goes above Influence threshold
        if(currentInfluence >= influenceThreshold) OnSectorInfluenced();
    }

    public void OnSectorInfluenced()    //when the total influence threshold is reached
    {
        if (isInfluenced) return; // Prevent multiple triggers  
        isInfluenced = true;
        
        //GETTING PLAYERS
            //Get top influencing Player & last acting player (the one who influenced it)
            PlayerManager topPlayer = GetTopInfluencingPlayer();
            if (topPlayer == null)
            {
                Debug.LogWarning($"SectorManager OnSectorInfluenced(): No top influencing player found in sector {sectorName}");
                return;
            }
            //Get Last player
            PlayerManager lastPlayer = null;
            if (playerActions != null && playerActions.Length > 0 && playerActions[0] != null)
                lastPlayer = playerActions[0].player;

        //VISUALS
            //Set sector color to the top player's color and light to dominant approach
            Renderer sectorRenderer = GetComponent<Renderer>();
            if (sectorRenderer != null) sectorRenderer.material.color = topPlayer.playerColor;
            // Prefer Color on standard Unity Light; if using LinearColor, convert as needed elsewhere
            sectorLight.color = Approach.GetApproachColor(GetTopInfluencingApproach());
            sectorLight.enabled = true;

        //RESOURCES
            //Give half of the resources to the shared resources 
            Resource sharedResources = grantedResources / 2.0f;         //50% of the resources
            if (topPlayer.sharedResources != null)  topPlayer.sharedResources.resources += sharedResources;     //(all players have same shared resources)

            //Give remaining half of the resources to the players
            Resource playerResources = grantedResources - sharedResources; //aka grantedResources/2
        
            // Top player gets 75% of the player portion (37.5% of total)
            topPlayer.personalResources += playerResources * 0.75f;

            // Last acting player gets 25% of the player portion (12.5% of total)
            if (lastPlayer != null) lastPlayer.personalResources += playerResources * 0.25f;
            if (lastPlayer == null) topPlayer.personalResources += playerResources * 0.25f;     // If no last player, give the 25% to the top player instead
            
            // Update UI for all players who received resources
            UpdateResourceUIForPlayer(topPlayer);
            if (lastPlayer != null && lastPlayer != topPlayer)
            {
                UpdateResourceUIForPlayer(lastPlayer);
            }
        }
        
        private void UpdateResourceUIForPlayer(PlayerManager player)
        {
            if (player != null && player.uiManager != null)
            {
                player.uiManager.UpdateResourcesUI();
                Debug.Log($"SectorManager.UpdateResourceUIForPlayer(): Updated resource UI for {player.playerName}");
            }
        }

    public PlayerManager GetTopInfluencingPlayer()
    {
        if(playerInfluence.Count == 0) return null;
        return playerInfluence.OrderByDescending(x => x.Value).First().Key;
    }

    public float GetPlayerInfluence(PlayerManager player)
    {
        if (playerInfluence.ContainsKey(player)) return playerInfluence[player];
        return 0f;
    }

    public ApproachType GetTopInfluencingApproach()
    {
        if (playerInfluence.Count == 0) return ApproachType.None;
        return approachInfluence.OrderByDescending(x => x.Value).First().Key;
    }

    public float GetApproachInfluence(ApproachType approach)
    {
        if (approachInfluence.ContainsKey(approach)) return approachInfluence[approach];
        return 0f;
    }

    public bool CanReceiveCard(CardManager card)
    {
        return card.targetType == CardTargetType.Sector;
    }
    
    public string RecentActions(int amount)
    {
        string result = "";
        int count = Mathf.Min(amount, playerActions.Length);

        for (int i = 0; i < count; i++)
        {
            if (i > 0) result += "\n";
            if (playerActions[playerActions.Length - 1 - i] != null)
            {
                result += playerActions[playerActions.Length - 1 - i].GetActionMessage();
                result += "\n";
                result += playerActions[playerActions.Length - 1 - i].card.description;
            }
        }
        return result;
    }
}