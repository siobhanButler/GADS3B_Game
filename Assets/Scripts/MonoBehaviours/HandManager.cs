using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandManager : MonoBehaviour
{
    [Header("Cards")]
    List<CardManager> hand; //HandManager hand; The cards that the player posesses
    CardManager selectedCard;

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

    public void PlayCard()
    {

    }

    public void AddCard(CardManager card)
    {
        hand.Add(card);
    }
}
