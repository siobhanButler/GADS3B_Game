using UnityEngine;

public abstract class CardManager : MonoBehaviour
{
    //CardType cardType;      //type of card
    public string cardName;    //name of the card
    public string description; //description of the card's effect

    public PlayerManager player;   //which player played this card
    public int lifetime;           //how many turns this card will remain in play
    public ApproachType approach;  //which approach this card belongs to

    public SectorManager targetSector; //which sector this card is being played on

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void ApplyCardEffect();  //must be overridden by subclasses
}
