using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SectorManager : MonoBehaviour, IClickable
{
    public SectorType sectorType;
    public Resource grantedResources;   //resources granted by this sector
    public int totalResources;         //total resources in this sector

    public CountryManager country;
    public int index;                  //index of this sector in the country

    public float currentInfluence;     //influence in this sector (out of 100)

    public List<KeyValuePair<PlayerManager, float>> playerInfluence;

    public PlayerAction[] playerActions = new PlayerAction[3]; //cards/actions played in this sector, most recent will be displayed in UI
    public CardManager cardSlot1;
    public CardManager cardSlot2;

    [Header("Clickable Interface Settings")]
    public Animation clickAnimation;
    public AudioSource clickAudio;

    //Propperties
    //public Animation anim => clickAnimation;
    //public AudioSource audioSource => clickAudio;
    public string sectorName => sectorType.ToString(); //name of the sector, derived from sectorType
    string description => $"This is the {index} {sectorName} sector in {country.countryName}." + sectorType.Description; //description of the sector, can be expanded later

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

        // Initialize player influence list
        if (playerInfluence == null) playerInfluence = new List<KeyValuePair<PlayerManager, float>>();

        // Initialize player actions list
        if (playerActions == null) playerActions = new PlayerAction[3];

        // Set default influence if not set
        if (currentInfluence <= 0) currentInfluence = 0f; // Start with no influence

        // Validate resource breakdown
        if (!sectorType.ResourceBreakdown.IsValid())
        {
            Debug.LogWarning($"SectorManager Setup(): Resource breakdown for {sectorType.SectorName} doesn't total 100%! " +
                           $"Current total: {sectorType.ResourceBreakdown.GetTotalPercentage()}%");
        }

        Debug.Log($"SectorManager Setup(): {sectorType.SectorName} sector initialized with {totalResources} total resources");
    }

    string RecentActions(int amount)
    {
        string result = "";
        int count = Mathf.Min(amount, playerActions.Length);

        for (int i = 0; i < count; i++)
        {
            if (i > 0) result += "\n";
            result += playerActions[playerActions.Length - 1 - i].GetActionMessage();
            result += "\n";
            result += playerActions[playerActions.Length - 1 - i].card.description;
        }
        return result;
    }

    public void AddRecentAction(PlayerAction action)
    {
        //add most recent action to the index 0, move all other up by one and forget the last one
        playerActions[2] = playerActions[1];
        playerActions[1] = playerActions[0];
        playerActions[0] = action; 
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
            Debug.Log("SectorManager CustomClick(): Can not find ui manager.");
            return;
        }

        ui.ShowSectorUI(true, this);
    }
}
