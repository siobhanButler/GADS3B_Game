using Unity.VisualScripting;
using UnityEngine;

public class CardTarget : MonoBehaviour
{
    public delegate void PlayCardHandler(CardTarget target, PlayerManager player, CardManager card);
    public static event PlayCardHandler OnCardPlayed;

    public Canvas cardTargetCanvas; //Or panel? Get from Player UI

    public PlayerManager currentPlayer;
    public CardManager currentCard;
    
    //RoundManager.OnNewTurn += HandleNewTurn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowCardTargetUI(PlayerManager player, CardManager card)   //called if Player has a card selected and clicks on a target location
    {
        currentPlayer = player;
        currentCard = card;
        
        if (CanCardBePlayed(card))
        {
            //UIManager.UpdateInfo(targetname, card etc0
        }
        else
        {
            //UIManager.UpdateInfo("This card can not be played on this location type");
        }
    }

    bool CanCardBePlayed(CardManager card)
    {
        switch (card.targetType)
        {
            case CardTargetType.Sector:
                return GetComponent<SectorManager>() != null;  //if null -> false, !null -> true
                break;
            case CardTargetType.Country:
                return GetComponent<CountryManager>() != null;  //if null -> false, !null -> true
                break;
            case CardTargetType.Player:
                return false;
                break;
            default:
                Debug.Log("Something not right");
                return false;
                break;
        }
    }

    void PlayCard(PlayerManager player, CardManager card)   //called by button
    {
        //OnCardPlayed?.Invoke(this, player, card);
        card.ApplyCardEffect(this, player);

        //reset after card is played
        currentPlayer = null;
        currentCard = null;
    }
}
