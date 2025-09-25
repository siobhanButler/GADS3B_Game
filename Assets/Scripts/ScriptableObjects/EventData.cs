using UnityEngine;

[CreateAssetMenu(fileName = "EventData", menuName = "Scriptable Objects/EventData")]
public class EventData : ScriptableObject
{
    public string eventName;
    public string eventDescription;
    public string eventEffect;
    public int eventLifetime;

    public EEventType eventType;            //Mega, Macro, Micro
    public ESectorType sectorType;          //Government, Media, Economic, Social, Activism, Universal
    public EEffectType effectType;          //Influence, Resource

    [Header("For Influence EffectType (%)")]
    public float influenceModifier;         //for Influence type effects
    [Header("For Resource EffectType")]
    public Resource resourceModifier;       //for resource type effects
}

public enum EEventType
{
    Mega,
    Macro,
    Micro
}

public enum EEffectType
{
    Influence,
    Resource
}