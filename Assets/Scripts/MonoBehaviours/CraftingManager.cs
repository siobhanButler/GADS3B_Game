using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    CardManager card;               //select card's class
    ActionCardData actionCardData;  //data with which the new card is populated

    PlayerManager player;

    public Button craftButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //craftButton.onClick.AddListener(CraftCard());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        player = GetComponent<PlayerManager>(); //fix this to get correct player
        //craftButton.interactable(CanCraftCard());
    }

    public void CraftCard() //attached to the button called Craft
    {
        ApplyCostModifyer();
        if (CanCraftCard())
        {
            //CardManager craftedCard = new cardPrefab;
            //subtract resources from player and communal resources
            //do I need to create an instance of the card cause its a class not a mono behaviour?
            player.AddCraftedCard(card);
        }
    }

    bool CanCraftCard()
    {
        if (card == null || player == null) return false;

        //Resource minCost = card.cost / 2;

        if(player.personalResources.CanAfford(card.cost) && player.sharedResources.resources.CanAfford(card.cost))
        {
            return true;
        }
        else
        {
            //calculate difference between sharedResources and minCost, then add that to personalResourcesCost
            //check if personalResourceCost <= personalResources
            //show warning that personal resources will be used
            //return true
            return false;
        }
    }

    public void ApplyCostModifyer()
    {
        if (player.preferredApproach != card.approach) 
        {
            card.cost = card.cost + card.cost;  //double the cost
        }
    }
}
