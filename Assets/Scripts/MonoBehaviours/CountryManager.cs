using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CountryManager : MonoBehaviour, IClickable
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

    [Range(0f, 100f)] public float totalInfluence;   //for the control bar
    public CardManager cardSlot;   //a location card

    [Header("Clickable Interface Settings")]
    public Animation clickAnimation;
    public AudioSource clickAudio;

    //Propperties
    //public Animation anim => clickAnimation;
    //public AudioSource audioSource => clickAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

        if (countryRenderer == null) countryRenderer = GetComponent<Renderer>();
        if (influenceMaterials == null) influenceMaterials = new Material[5];
        countryRenderer.material = influenceMaterials[0];
    }

    public void UpdateCountryInfluence()
    {
        //Calculate total influence based on Sectors and archetype's weighting

        totalInfluence = Mathf.Clamp(totalInfluence, 0f, 100f); //clamp total influence to 0-100

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
        else if (totalInfluence > 80)
        {
            countryRenderer.material = influenceMaterials[4];
        }
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
    }
}
