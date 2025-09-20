using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CountryManager : MonoBehaviour
{
    CountryArchetype archetype;

    public string countryName;    //name of the country
    public string description => $"{countryName} is of the {archetype.name} archetype.";    //description of the country

    List<GameObject> Tiles;
    List<SectorManager> Sectors;

    float totalInfluence;   //for the control bar
    CardManager cardSlot;   //a location card

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
    }
}
