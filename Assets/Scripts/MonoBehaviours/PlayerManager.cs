using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Info")]
    public string playerName;
    int playerID;
    ApproachType preferredApproach;

    List<PlayerAction> previousActions; //The actions that the player has taken in previous turns

    [Header("Cards")]
    List<CardManager> hand; //HandManager hand; The cards that the player posesses

    [Header("Resources")]
    ResourceManager personalResources;
    ResourceManager sharedResources;

    List<SectorManager> influencedSectors; //The sections that the player controls/posesses

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
