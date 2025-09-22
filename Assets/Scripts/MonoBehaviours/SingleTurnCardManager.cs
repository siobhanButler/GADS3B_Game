using UnityEngine;

public class SingleTurnCardManager : CardManager
{
    public SectorManager targetSector;      //which sector this card is being played on

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
}
