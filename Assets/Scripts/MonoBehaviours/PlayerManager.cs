using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Info")]
    public string playerName;
    public int playerID;
    public Color playerColor;
    public ApproachType preferredApproach;

    List<PlayerAction> previousActions; //The actions that the player has taken in previous turns
    bool tookActionThisTurn;

    [Header("Cards")]
    List<CardManager> hand; //HandManager hand; The cards that the player posesses
    CardManager selectedCard;

    [Header("Resources")]
    public Resource personalResources;
    public ResourceManager sharedResources;

    List<SectorManager> influencedSectors; //The sections that the player controls/posesses

    [Header("UI")]
    public UIManager uiManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectCard(CardManager card)
    {
        selectedCard = card;

        //logic to grey out all Game objects that it can not be played on
    }

    public void PlayCard(CardManager card)
    {
        PlayerAction action = new PlayerAction();
        action.SetPlayerAction(ActionType.Craft, this, card);
        previousActions.Add(action);
    }

    public void AddCraftedCard(CardManager card)
    {
        hand.Add(card);

        PlayerAction action = new PlayerAction();
        action.SetPlayerAction(ActionType.Craft, this, card);
        previousActions.Add(action);
    }
}
