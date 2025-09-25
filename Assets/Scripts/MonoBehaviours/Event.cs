using System.Collections.Generic;
using UnityEngine;

public class Event
{
    public EventData eventData;
    public string eventName;
    public string eventDescription;
    public string eventEffect;
    public int eventLifetime;

    public EEventType eventType;            //Mega, Macro, Micro
    public ESectorType sectorType;          //Government, Media, Economic, Social, Activism, Universal
    public EEffectType effectType;          //Influence, Resource

    public float influenceModifier;         //for Influence type effects
    public Resource resourceModifier;       //for Resource type effects

    [Header("For Mega, Macro & Micro Events")]
    public List<SectorManager> targetSectors;       //will be length = 1 for micro events

    public Event(EventData pEventData, List<SectorManager> pTargetSectors)
    {
        eventData = pEventData;
        targetSectors = pTargetSectors;

        Setup();
    }

    public void Setup()
    {
        eventName = eventData.eventName;
        eventDescription = eventData.eventDescription;
        eventEffect = eventData.eventEffect;
        eventType = eventData.eventType;
        sectorType = eventData.sectorType;
        effectType = eventData.effectType;
        influenceModifier = eventData.influenceModifier;
        resourceModifier = eventData.resourceModifier;
        eventLifetime = eventData.eventLifetime;
    }

    public void ApplyEventEffect()
    {
        switch (effectType)
        {
            case EEffectType.Influence:
                ApplyInfluenceEffect();
                break;
            case EEffectType.Resource:
                ApplyResourceEffect();
                break;
        }

        eventLifetime--;
    }

    private void ApplyInfluenceEffect()
    {
        foreach (SectorManager sector in targetSectors)
        {
            sector.currentInfluence += sector.currentInfluence * influenceModifier;   //if influenceModifier is a negative value, it will decrease it instead
        }
    }

    private void ApplyResourceEffect()
    {
        foreach (SectorManager sector in targetSectors)
        {
            sector.grantedResources += resourceModifier;    //if resourceModifier is a negative value, it will subtract it instead
        }
    }
}
