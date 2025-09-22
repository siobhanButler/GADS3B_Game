using UnityEngine;

/// <summary>
/// Helper class to create CountryArchetype assets based on the provided table
/// This is for reference - create these archetypes as ScriptableObject assets
/// </summary>
public static class CountryArchetypePresets
{
    /// <summary>
    /// Creates an Authoritarian State archetype (North Korea)
    /// </summary>
    public static CountryArchetype CreateAuthoritarianState()
    {
        var archetype = ScriptableObject.CreateInstance<CountryArchetype>();
        
        // Set basic info
        // archetype.archetypeName = "Authoritarian State";
        // archetype.description = "Total state control, little to no independent movement activity";
        
        // Sector weights: Government=5, Media=5, Economic=2, Social=1, Activism=1
        // archetype.sectorWeights = new SectorWeights
        // {
        //     government = 5,
        //     media = 5,
        //     economic = 2,
        //     social = 1,
        //     activism = 1
        // };
        
        // Approach effectiveness: Government=5, Media=5, Economic=2, Social=1, Activism=1, Universal=3
        // archetype.approachEffectiveness = new ApproachEffectiveness
        // {
        //     government = 5,
        //     media = 5,
        //     economic = 2,
        //     social = 1,
        //     activism = 1,
        //     universal = 3
        // };
        
        return archetype;
    }
    
    /// <summary>
    /// Creates a Liberal Democracy archetype (USA)
    /// </summary>
    public static CountryArchetype CreateLiberalDemocracy()
    {
        var archetype = ScriptableObject.CreateInstance<CountryArchetype>();
        
        // Sector weights: Government=4, Media=4, Economic=5, Social=4, Activism=3
        // Approach effectiveness: Government=4, Media=4, Economic=5, Social=4, Activism=3, Universal=4
        
        return archetype;
    }
    
    /// <summary>
    /// Creates a State-Capitalist Authoritarianism archetype (China)
    /// </summary>
    public static CountryArchetype CreateStateCapitalistAuthoritarianism()
    {
        var archetype = ScriptableObject.CreateInstance<CountryArchetype>();
        
        // Sector weights: Government=5, Media=4, Economic=5, Social=2, Activism=2
        // Approach effectiveness: Government=5, Media=4, Economic=5, Social=2, Activism=2, Universal=3
        
        return archetype;
    }
    
    /// <summary>
    /// Creates a Social Democracy archetype (Scandinavia)
    /// </summary>
    public static CountryArchetype CreateSocialDemocracy()
    {
        var archetype = ScriptableObject.CreateInstance<CountryArchetype>();
        
        // Sector weights: Government=4, Media=3, Economic=4, Social=5, Activism=4
        // Approach effectiveness: Government=4, Media=3, Economic=4, Social=5, Activism=4, Universal=4
        
        return archetype;
    }
    
    /// <summary>
    /// Creates a Fluctuating Democracy archetype (Brazil)
    /// </summary>
    public static CountryArchetype CreateFluctuatingDemocracy()
    {
        var archetype = ScriptableObject.CreateInstance<CountryArchetype>();
        
        // Sector weights: Government=3, Media=3, Economic=3, Social=4, Activism=4
        // Approach effectiveness: Government=3, Media=3, Economic=3, Social=4, Activism=4, Universal=3
        
        return archetype;
    }
    
    /// <summary>
    /// Creates a Transitional Democracy archetype (South Africa)
    /// </summary>
    public static CountryArchetype CreateTransitionalDemocracy()
    {
        var archetype = ScriptableObject.CreateInstance<CountryArchetype>();
        
        // Sector weights: Government=3, Media=4, Economic=2, Social=5, Activism=5
        // Approach effectiveness: Government=3, Media=4, Economic=2, Social=5, Activism=5, Universal=4
        
        return archetype;
    }
    
    /// <summary>
    /// Creates a Resource-Rich Autocracy archetype (Saudi Arabia)
    /// </summary>
    public static CountryArchetype CreateResourceRichAutocracy()
    {
        var archetype = ScriptableObject.CreateInstance<CountryArchetype>();
        
        // Sector weights: Government=5, Media=4, Economic=5, Social=2, Activism=1
        // Approach effectiveness: Government=5, Media=4, Economic=5, Social=2, Activism=1, Universal=3
        
        return archetype;
    }
}
