using UnityEngine;

public interface ICardTarget
{
    CardTargetType TargetType { get; }
    string TargetName { get; }

    CountryManager TargetCountryManager { get; }
    SectorManager TargetSectorManager { get; }

    bool CanReceiveCard(CardManager card);
}

public enum CardTargetType
{
    Country,
    Sector,
    Player
}
