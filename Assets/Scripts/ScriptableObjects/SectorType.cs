using UnityEngine;

/// <summary>
/// ScriptableObject that defines the properties and behavior of a sector type
/// Each sector type (Media, Activism, Government, Social, Economic) should have its own instance
/// </summary>
[CreateAssetMenu(fileName = "SectorType", menuName = "Scriptable Objects/SectorType")]
public class SectorType : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] private string sectorName;
    [SerializeField] private string description;
    [SerializeField] private ESectorType sectorType;

    [Header("Resource Generation")]
    [SerializeField] private ResourceBreakdown resourceBreakdown;
    
    [Header("Effective Approaches")]
    [SerializeField] private ApproachType[] effectiveApproaches;
    
    [Header("Visual Settings")]
    [SerializeField] private Mesh mesh;
    
    // Properties for easy access
    public string SectorName => sectorName;
    public string Description => description;
    public ResourceBreakdown ResourceBreakdown => resourceBreakdown;
    public ApproachType[] EffectiveApproaches => effectiveApproaches;
    public Mesh Mesh => mesh;
}

/// <summary>
/// Defines how resources are distributed for a sector type
/// </summary>
[System.Serializable]
public struct ResourceBreakdown
{
    [Header("Resource Percentages (should total 100%)")]
    [Range(0, 100)] public int mediaPercentage;
    [Range(0, 100)] public int legitimacyPercentage;
    [Range(0, 100)] public int knowledgePercentage;
    [Range(0, 100)] public int labourPercentage;
    [Range(0, 100)] public int solidarityPercentage;
    [Range(0, 100)] public int moneyPercentage;
    
    /// <summary>
    /// Validates that percentages add up to 100%
    /// </summary>
    public bool IsValid()
    {
        int total = mediaPercentage + legitimacyPercentage + knowledgePercentage + 
                   labourPercentage + solidarityPercentage + moneyPercentage;
        return total == 100;
    }
    
    /// <summary>
    /// Gets the total percentage (for validation)
    /// </summary>
    public int GetTotalPercentage()
    {
        return mediaPercentage + legitimacyPercentage + knowledgePercentage + 
               labourPercentage + solidarityPercentage + moneyPercentage;
    }

}

public enum ESectorType
{
    Government,
    Media,
    Economic,
    Social,
    Activism,
    Universal
}