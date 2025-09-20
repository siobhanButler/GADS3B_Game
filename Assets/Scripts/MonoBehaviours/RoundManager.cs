using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [Header("Round Settings")]
    public int totalRounds;
    public int turnsPerRound;       //number of turns in each round, last turn is discussion phase
    public int craftingTurns;       //number of turns in each round where players can craft cards

    int currentRound;
    int currentTurn;
    RoundState currentState;          //which actions are allowed in this round state
    bool canCraft;                    //can players craft cards in this turn
    bool canPlayActionCards;          //can players play action cards in this turn
    bool canSpeak;                    //can players speak in this turn

    List<PlayerAction>[,] playerActions;     //All actions of all players in this game session
                                             //2D array where each element stores a PlayerAction list, row = turnNumber, column = rowNumber

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup()
    {
        //Settig Defaults
        playerActions = new List<PlayerAction>[totalRounds, turnsPerRound];
        currentRound = 1;
        currentTurn = 1;
    }

    private void NextTurn()
    {
        currentTurn++;

        if (currentTurn < turnsPerRound - 1)     //not the last turn, so normal crafting/action phase
        {
            if(currentTurn <= craftingTurns)    //crafting phase
            {
                currentState = RoundState.Crafting;
            }
            else                                //action phase
            {
                currentState = RoundState.Action;
            }
        }
        else if (currentTurn == turnsPerRound) //last turn, meaning it is the discussion phase
        {
            canCraft = false;
        }
        else        //currentTurn > turnsPerRound, meaning the round is over
        {
            NextRound();
        }
    }

    private void NextRound()
    {
        currentTurn = 1;
        currentRound++;
        //if (currentRound >= totalRounds)   //game over
    }

    private void SetState(RoundState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case RoundState.Crafting:
                canCraft = true;
                canPlayActionCards = true;
                canSpeak = false;
                break;
            case RoundState.Action:
                canCraft = false;
                canPlayActionCards = true;
                canSpeak = false;
                break;
            case RoundState.Discussion:
                canCraft = false;
                canPlayActionCards = false;
                canSpeak = true;
                break;
        }
    }
}

public enum RoundState
{
    Crafting,   //players can craft cards and play action cards
    Action,     //players can only play action cards
    Discussion  //players can now talk
}