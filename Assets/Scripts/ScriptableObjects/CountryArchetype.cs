using UnityEngine;

/// <summary>
/// CountryArchetype defines the characteristics and sector weights for different country types
/// Each archetype has different sector strengths and approach effectiveness
/// </summary>
[CreateAssetMenu(fileName = "CountryArchetype", menuName = "Scriptable Objects/CountryArchetype")]
public class CountryArchetype : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] private string archetypeName;
    [SerializeField] private string description;
    [SerializeField] private Color colour;
    
    [Header("Sector Weights (1-5 scale)")]
    [SerializeField] private SectorWeights sectorWeights;
    
    [Header("Approach Effectiveness (1-5 scale)")]
    [SerializeField] private ApproachEffectiveness approachEffectiveness;
    
    [Header("Notes")]
    [SerializeField] private string notes;
    
    // Properties for easy access
    public string ArchetypeName => archetypeName;
    public string Description => description;
    public Color Colour => colour;
    public SectorWeights SectorWeights => sectorWeights;
    public ApproachEffectiveness ApproachEffectiveness => approachEffectiveness;
    public string Notes => notes;
    
    /// <summary>
    /// Gets the weight for a specific sector type
    /// </summary>
    public int GetSectorWeight(ESectorType sectorType)
    {
        switch (sectorType)
        {
            case ESectorType.Government: return sectorWeights.government;
            case ESectorType.Media: return sectorWeights.media;
            case ESectorType.Economic: return sectorWeights.economic;
            case ESectorType.Social: return sectorWeights.social;
            case ESectorType.Activism: return sectorWeights.activism;
            default: return 1;
        }
    }
    
    /// <summary>
    /// Gets the effectiveness for a specific approach type
    /// </summary>
    public int GetApproachEffectiveness(ESectorType approachType)
    {
        switch (approachType)
        {
            case ESectorType.Government: return approachEffectiveness.government;
            case ESectorType.Media: return approachEffectiveness.media;
            case ESectorType.Economic: return approachEffectiveness.economic;
            case ESectorType.Social: return approachEffectiveness.social;
            case ESectorType.Activism: return approachEffectiveness.activism;
            case ESectorType.Universal: return approachEffectiveness.universal;
            default: return 1;
        }
    }
}

/// <summary>
/// Defines sector weights for different country archetypes (1-5 scale)
/// </summary>
[System.Serializable]
public struct SectorWeights
{
    [Header("Sector Strength (1-5 scale)")]
    [Range(1, 5)] public int government;
    [Range(1, 5)] public int media;
    [Range(1, 5)] public int economic;
    [Range(1, 5)] public int social;
    [Range(1, 5)] public int activism;
    
    /// <summary>
    /// Gets the total weight for validation
    /// </summary>
    public int GetTotalWeight()
    {
        return government + media + economic + social + activism;
    }
    
    /// <summary>
    /// Gets the average weight
    /// </summary>
    public float GetAverageWeight()
    {
        return GetTotalWeight() / 5f;
    }
}

/// <summary>
/// Defines approach effectiveness for different country archetypes (1-5 scale)
/// </summary>
[System.Serializable]
public struct ApproachEffectiveness
{
    [Header("Approach Effectiveness (1-5 scale)")]
    [Range(1, 5)] public int government;
    [Range(1, 5)] public int media;
    [Range(1, 5)] public int economic;
    [Range(1, 5)] public int social;
    [Range(1, 5)] public int activism;
    [Range(1, 5)] public int universal;
    
    /// <summary>
    /// Gets the total effectiveness for validation
    /// </summary>
    public int GetTotalEffectiveness()
    {
        return government + media + economic + social + activism + universal;
    }
    
    /// <summary>
    /// Gets the average effectiveness
    /// </summary>
    public float GetAverageEffectiveness()
    {
        return GetTotalEffectiveness() / 6f;
    }
}