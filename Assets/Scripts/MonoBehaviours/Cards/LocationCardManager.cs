using UnityEngine;

public class LocationCardManager : CardManager
{
    public CountryManager targetCountry;    //which country this card is being played on

    public LocationCardManager(ActionCardData data) : base(data)
    {
        SetupCardManager();
        targetType = CardTargetType.Country;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Setup()
    {
        targetType = CardTargetType.Country;
    }

    public override void ApplyCardEffect(ICardTarget target, PlayerManager player)
    {
        targetCountry = target.TargetCountryManager;
        if (targetCountry == null)
        {
            Debug.Log("SingleTurnCardManager ApplyCardEffect(): Card can not find CountryManager");
        }
    }
}
