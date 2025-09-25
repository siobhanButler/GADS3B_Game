using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;

public class HandManager : MonoBehaviour
{
    public PlayerManager player;
    public List<CardManager> hand = new List<CardManager>();      //The cards that the player posesses
    public CardManager selectedCard;
    public int maxHandSize = 6;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = null;
        selectedCard = null;
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

    public int GetCardIndex(CardManager card)
    {
        if (!hand.Contains(card)) return -1;
        return hand.IndexOf(card);
    }

    public void PlayCard(CardManager card, ICardTarget target, PlayerManager player)
    {
        card.ApplyCardEffect(target, player);
        hand.Remove(card);
    }

    public void AddCard(CardManager card)
    {
        Debug.Log($"HandManager.AddCard(): Adding card '{card.cardName}' to hand. Current hand size: {hand.Count}");
        
        if (hand.Count >= maxHandSize)
        {
            Debug.LogWarning($"HandManager.AddCard(): Hand is full! Cannot add '{card.cardName}'. Max hand size: {maxHandSize}");
            return;
        }
        
        hand.Add(card);
        Debug.Log($"HandManager.AddCard(): Successfully added '{card.cardName}'. New hand size: {hand.Count}");
        
        // Log all cards in hand for debugging
        Debug.Log($"HandManager.AddCard(): Current hand contains: {string.Join(", ", hand.ConvertAll(c => c.cardName))}");
    }

    public bool CanAddCard(int cardsToAdd)
    {
        return (hand.Count + cardsToAdd <= maxHandSize);
    }

    public bool HasCardSelected()
    {
        return (selectedCard != null);
    }
}
