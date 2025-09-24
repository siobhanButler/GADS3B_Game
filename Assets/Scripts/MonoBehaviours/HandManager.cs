using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandManager : MonoBehaviour
{
    public PlayerManager player;
    public List<CardManager> hand;      //The cards that the player posesses
    public CardManager selectedCard;
    public int maxHandSize = 5;

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

    public int GetCardIndex(CardManager card)
    {
        if (!hand.Contains(card)) return -1;
        return hand.IndexOf(card);
    }

    public void PlayCard()
    {

    }

    public void AddCard(CardManager card)
    {
        hand.Add(card);
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
