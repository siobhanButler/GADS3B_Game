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

    List<PlayerAction> previousActions = new List<PlayerAction>(); //The actions that the player has taken in previous turns
    bool tookActionThisTurn;

    public HandManager handManager;

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
        handManager.SelectCard(card);

        //logic to grey out all Game objects that it can not be played on
    }

    public void PlayCard(CardManager card, ICardTarget target)
    {
        PlayerAction action = new PlayerAction();
        action.SetPlayerAction(ActionType.Craft, this, card);
        previousActions.Add(action);

        card.ApplyCardEffect(target, this);
    }

    public void AddCraftedCard(CardManager card)
    {
        Debug.Log($"PlayerManager.AddCraftedCard(): Adding card '{card.cardName}' to {playerName}'s hand");
        handManager.AddCard(card);

        PlayerAction action = new PlayerAction();
        action.SetPlayerAction(ActionType.Craft, this, card);
        previousActions.Add(action);
        
        Debug.Log($"PlayerManager.AddCraftedCard(): Card '{card.cardName}' successfully added. Hand now has {handManager.hand.Count} cards");

        uiManager.playerHandUI.UpdateHandUI(this);
    }
}
