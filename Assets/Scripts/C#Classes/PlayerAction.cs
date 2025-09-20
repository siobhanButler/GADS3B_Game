using UnityEngine;

public class PlayerAction
{
    ActionType actionType;
    PlayerManager player;       //the player taking the action
    public CardManager card;    //only applicable if actionType is ActionCard
    int turnNumber;             //the turn number this action was taken

    public string ActionMessage()
    {
        switch (actionType)
        {
            case ActionType.None:
                card = null;
                return $"{player.playerName} did not do anything.";
            case ActionType.Craft:
                card = null;
                return $"{player.playerName} crafted a card.";
            case ActionType.ActionCard:
                return $"{player.playerName} played {card.cardName} on {card.targetSector.sectorName}.";
            ;
            default:
                return $"{player.playerName} is doing something unknown.";
        }
        return null;
    }
}

public enum ActionType
{
    None,
    Craft,
    ActionCard
}
