using UnityEngine;

public class MultiTurnCardManager : CardManager
{
    public SectorManager targetSector;      //which sector this card is being played on
    public float influencePerTurn;
    public Resource costPerTurn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
    public override void ApplyCardEffect(CardTarget target, PlayerManager player)
    {
        targetSector = target.GetComponent<SectorManager>();
        if (targetSector == null)
        {
            Debug.Log("SingleTurnCardManager ApplyCardEffect(): Card can not find SectorManager");
        }
    }

    private void HandleNewTurn(int roundNumber, int turnNumber)
    {
        currentLifetime -= 1;   //subtract one turn from lifetime
        if (currentLifetime > 0) 
        { 
            //ApplyCardEffect();
        }
    }

    private void HandleNewRound(int roundNumber)
    {
        // TODO: implement per-round behavior if needed
    }
}
