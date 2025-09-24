using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public enum ApproachType
{
    None,
    Peaceful,
    Violent
}

public static class Approach
{
    public static Color noneColor;
    public static Color peacefulColor;
    public static Color violentColor;
    
    public static Color GetApproachColor(ApproachType approach)
    {
        switch (approach)
        {
            case ApproachType.None:
                return noneColor;
            case ApproachType.Peaceful:
                return peacefulColor;
            case ApproachType.Violent:
                return violentColor;
            default:
                return Color.black;
        }
    }
}

