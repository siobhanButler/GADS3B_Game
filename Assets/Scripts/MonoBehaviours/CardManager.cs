using UnityEngine;

public abstract class CardManager
{
    //CardType cardType;      //type of card
    public string cardName;    //name of the card
    public string description; //description of the card's effect
    public Sprite cardImage;
    public CardTargetType targetType;

    public int totalLifetime;           //how many turns this card will remain in play
    public int currentLifetime;
    public ApproachType approach;  //which approach this card belongs to
    public Resource cost;

    public PlayerManager player;   //which player played this card
    public CardTarget target;
    //public SectorManager targetSector;      //which sector this card is being played on
    //public CountryManager targetCountry;    //which country this card is being played on
    //public PlayerManager targetPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLifetime = totalLifetime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void ApplyCardEffect(CardTarget target, PlayerManager player);  //must be overridden by subclasses
}

public enum CardTargetType
{
    Sector,
    Country,
    Player
}
