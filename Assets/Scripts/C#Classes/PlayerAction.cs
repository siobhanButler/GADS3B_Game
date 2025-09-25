using UnityEngine;

public class PlayerAction
{
    ActionType actionType;
    public PlayerManager player;       //the player taking the action
    public CardManager card;    //only applicable if actionType is ActionCard
     
    //int turnNumber;             //the turn number this action was taken

    public void SetPlayerAction(ActionType pActionType, PlayerManager pPlayer, CardManager pCard)
    {
        actionType = pActionType;
        player = pPlayer;
        card = pCard;
        //turnNumber = pTurnNumber;
    }

    public string GetActionMessage()
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
                try { return $"{player.playerName} played {card.cardName} on {card.target.TargetName}."; }
                catch { return $"{player.playerName} played {card.cardName}."; }
            ;
            default:
                return $"{player.playerName} is doing something unknown.";
        }
    }
}

public enum ActionType
{
    None,
    Craft,
    ActionCard
}
