using UnityEngine;

[CreateAssetMenu(fileName = "ActionCardData", menuName = "Scriptable Objects/ActionCardData")]
public class ActionCardData : ScriptableObject
{
    CardManager cardClass;      //type of card/class
    public string cardName;    //name of the card
    public string description; //description of the card's effect
    public Sprite cardSprite;
    public CardTargetType targetType;

    //public int totalLifetime;           //how many turns this card will remain in play
    //public int currentLifetime;
    public ApproachType approach;  //which approach this card belongs to
    public Resource cost;

    public float influencePerTurn;
    public Resource costPerTurn;
}
