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
        }

        targetSector.AddInfluence(influenceToAdd, player, approach);
    }
}
