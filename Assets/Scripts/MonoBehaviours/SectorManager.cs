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

    List<PlayerAction> playerActions; //cards/actions played in this sector, most recent will be displayed in UI
    CardManager cardSlot1;
    CardManager cardSlot2;

    [Header("Clickable Interface Settings")]
    public Animation clickAnimation;
    public AudioSource clickAudio;

    //Propperties
    public Animation anim => clickAnimation;
    public AudioSource audioSource => clickAudio;
    public string sectorName => sectorType.ToString(); //name of the sector, derived from sectorType
    string description => $"This is the {index} {sectorName} sector in {country.countryName}."; //description of the sector, can be expanded later

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string LastPlayers(int amount)
    {
        string result = "";
        int count = Mathf.Min(amount, playerActions.Count);

        for (int i = 0; i < count; i++)
        {
            if (i > 0) result += "\n";
            result += playerActions[playerActions.Count - 1 - i].ActionMessage();
            result += "\n";
            result += playerActions[playerActions.Count - 1 - i].card.description;
        }
        return result;
    }

    public void CustomClick(ClickDetector clicker)
    {
        PlayerManager player = clicker.GetComponent<PlayerManager>();
        if (player == null)
        {
            Debug.Log("SectorManager CustomClick(): was not clicked by a player.");
            return;
        }
    }
}
