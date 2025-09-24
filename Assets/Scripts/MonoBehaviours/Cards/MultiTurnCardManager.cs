using UnityEngine;

public class MultiTurnCardManager : CardManager
{
    public SectorManager targetSector;      //which sector this card is being played on
    public float influencePerTurn;
    public Resource costPerTurn;

    public MultiTurnCardManager(ActionCardData data) : base(data)
    {
        SetupCardManager();
        influencePerTurn = data.influencePerTurn;
        costPerTurn = data.costPerTurn;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        SetupMultiTurnCardManager();
    }
    
    void Start()
    {
        
    }

    void OnEnable()
    {
        RoundManager.OnNewTurn += HandleNewTurn;
        RoundManager.OnNewRound += HandleNewRound;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetupMultiTurnCardManager()
    {
        targetType = CardTargetType.Sector;
        targetSector = null;

        cardData.influencePerTurn = influencePerTurn;
        cardData.costPerTurn = costPerTurn;
    }

    public override void ApplyCardEffect(ICardTarget pTarget, PlayerManager pPlayer)
    {
        player = pPlayer;
        target = pTarget;
        targetSector = pTarget.TargetSectorManager;
        if (targetSector == null)
        {
            Debug.Log("MultiTurnCardManager ApplyCardEffect(): Card can not find SectorManager");
            return;
        }
        CardEffect();
    }

    private void CardEffect()
    {
        //Add influence to the targeted sector
        targetSector.AddInfluence(influencePerTurn, player, approach);
    }

    private void HandleNewTurn(int roundNumber, int turnNumber)
    {
        if (targetSector == null) return;   //card has not been played on anything yet

        //Charging player happens here so it doesnt happen when card is first played, only the rounds thereafter
        if (ChargePlayer()) //if charge was sucsessful, apply effect
        {
            CardEffect();
        }
        else //if charge unsuccessful, remove card
        {
            RemoveCard();
        }
    }

    private void HandleNewRound(int roundNumber)
    {
        // TODO: implement per-round behavior if needed
    }

    private bool ChargePlayer()
    {
        if (player.personalResources.CanAfford(costPerTurn))    //only charge player if they can afford it
        {
            player.personalResources -= costPerTurn;
            return true;
        }
        return false;   //can not afford/ unsuccessful charge
    }

    protected override void RemoveCard()
    {
        isRemoved = true;

        //Unsubscribing from these events, so that effect is no longer applied
        RoundManager.OnNewTurn -= HandleNewTurn;
        RoundManager.OnNewRound -= HandleNewRound;
    }
}
