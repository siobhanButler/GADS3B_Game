using UnityEngine;

public abstract class CardManager
{
    //CardType cardType;      //type of card
    public string cardName;    //name of the card
    public string description; //description of the card's effect
    public Sprite cardSprite;
    public CardTargetType targetType;
    public ActionCardData cardData;

    //public int totalLifetime;           //how many turns this card will remain in play
    //public int currentLifetime;
    public ApproachType approach;  //which approach this card belongs to
    public Resource cost;

    public PlayerManager player;   //which player played this card
    public ICardTarget target;
    public bool isRemoved;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        SetupCardManager();
    }
    
    void Start()
    {
        //currentLifetime = totalLifetime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetupCardManager()
    {
        player = null;
        target = null;
        isRemoved = false;

        if (cardData == null)
        {
            Debug.Log($"CardManager Setup(): cardData is empty!");
            return;
        }

        cardName = cardData.cardName;
        description = cardData.description;
        cardSprite = cardData.cardSprite;
        targetType = cardData.targetType;
        approach = cardData.approach;
        cost = cardData.cost;
    }

    public abstract void ApplyCardEffect(ICardTarget target, PlayerManager player);  //must be overridden by subclasses

    bool CanCardBePlayed(ICardTarget target)
    {
        return target != null && target.CanReceiveCard(this);
    }

    protected virtual void RemoveCard()     //can optionally be overridden
    {
        isRemoved = true;
    }
}
