using UnityEngine;

public class SingleTurnCardManager : CardManager
{
    public SectorManager targetSector;      //which sector this card is being played on
    public float influenceToAdd;

    public SingleTurnCardManager(ActionCardData data) : base(data)
    {
        SetupCardManager();
        influenceToAdd = data.influencePerTurn;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Setup();
        influenceToAdd = cardData.influencePerTurn;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Setup()
    {
        targetType = CardTargetType.Sector;
    }

    public override void ApplyCardEffect(ICardTarget target, PlayerManager player)
    {
        targetSector = target.TargetSectorManager;
        if (targetSector == null)
        {
            Debug.Log("SingleTurnCardManager ApplyCardEffect(): Card can not find SectorManager");
            return;
        }

        // Create and add the player action to recent actions
        PlayerAction action = new PlayerAction();
        action.SetPlayerAction(ActionType.ActionCard, player, this);
        targetSector.AddRecentAction(action);

        targetSector.AddInfluence(influenceToAdd, player, approach);
        
        Debug.Log($"SingleTurnCardManager.ApplyCardEffect(): Applied '{cardName}' to sector '{targetSector.sectorName}'");
    }
}
