using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    //public ECardClass cardClass;          //select card's class
    public CardManager card;
    public ActionCardData actionCardData;  //data with which the new card is populated

    public PlayerManager player;

    public Button craftButton;
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI cardDescriptionText;

    public UIManager uiManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        craftButton.onClick.AddListener(CraftCard);
        cardNameText.text = actionCardData.name;
        cardDescriptionText.text = actionCardData.description;
        if(uiManager == null) uiManager = GetComponentInParent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        // Get the current player from UIManager
        if (uiManager != null)
        {
            player = uiManager.currentPlayer;
        }
        else
        {
            uiManager = GetComponentInParent<UIManager>();
            if (uiManager != null)
            {
                player = uiManager.currentPlayer;
            }
        }
        
        UpdateCraftButtonState();
    }

    public void CraftCard() //attached to the button called Craft
    {
        player = uiManager.currentPlayer;
        if (!CanCraftCard())
        {
            Debug.LogWarning($"CraftingManager: Cannot craft {actionCardData.cardName} - insufficient resources");
            if (uiManager != null) uiManager.ShowCraftingUI(false);
            return;
        }

        // Apply cost modifier before creating card
        Resource finalCost = actionCardData.cost;
        if (player.preferredApproach != actionCardData.approach)
        {
            finalCost = actionCardData.cost * 2; // Double the cost
            Debug.Log($"CraftingManager: Cost doubled due to approach mismatch. New cost: {finalCost}");
        }

        // Deduct resources
        DeductResources(finalCost);

        // Create the appropriate card type
        CardManager craftedCard = null;
        switch (actionCardData.cardClass)
        {
            case ECardClass.LocationCard:
                craftedCard = new LocationCardManager(actionCardData);
                break;
            case ECardClass.SingleTurnCard:
                craftedCard = new SingleTurnCardManager(actionCardData);
                break;
            case ECardClass.MultiTurnCard:
                craftedCard = new MultiTurnCardManager(actionCardData);
                break;
            default:
                Debug.LogError($"CraftingManager: Unknown card class {actionCardData.cardClass}");
                return;
        }

        if (craftedCard != null)
        {
            Debug.Log($"CraftingManager.CraftCard(): Created card '{craftedCard.cardName}' successfully");
            // Add card to player's hand
            player.AddCraftedCard(craftedCard);
            Debug.Log($"CraftingManager.CraftCard(): Card '{craftedCard.cardName}' sent to player '{player.playerName}'");
            
            // Update UI to reflect resource changes
            UpdateResourceUI();
        }
        else
        {
            Debug.LogError($"CraftingManager.CraftCard(): Failed to create card from {actionCardData.cardName}");
        }
    }

    bool CanCraftCard()
    {
        if (actionCardData == null || player == null) return false;

        // Check if player can afford the card cost
        if (player.personalResources.CanAfford(actionCardData.cost) && 
            player.sharedResources.resources.CanAfford(actionCardData.cost))
        {
            return true;
        }
        else
        {
            // Calculate if player can afford with both resources combined
            Resource totalAvailable = player.personalResources + player.sharedResources.resources;
            if (totalAvailable.CanAfford(actionCardData.cost))
            {
                // Show warning that personal resources will be used
                Debug.LogWarning($"CraftingManager: Player will need to use personal resources to craft {actionCardData.cardName}");
                return true;
            }
            return false;
        }
    }

    private void DeductResources(Resource cost)
    {
        // Try to pay with shared resources first
        if (player.sharedResources.resources.CanAfford(cost))
        {
            player.sharedResources.resources.knowledge -= cost.knowledge;
            player.sharedResources.resources.money -= cost.money;
            player.sharedResources.resources.media -= cost.media;
            player.sharedResources.resources.labour -= cost.labour;
            player.sharedResources.resources.solidarity -= cost.solidarity;
            player.sharedResources.resources.legitimacy -= cost.legitimacy;
            Debug.Log($"CraftingManager: Deducted {cost} from shared resources");
        }
        else
        {
            // Use shared resources first, then personal
            int remainingKnowledge = cost.knowledge;
            int remainingMoney = cost.money;
            int remainingMedia = cost.media;
            int remainingLabour = cost.labour;
            int remainingSolidarity = cost.solidarity;
            int remainingLegitimacy = cost.legitimacy;
            
            // Deduct what we can from shared resources
            if (player.sharedResources.resources.knowledge > 0)
            {
                int deductKnowledge = Mathf.Min(remainingKnowledge, player.sharedResources.resources.knowledge);
                player.sharedResources.resources.knowledge -= deductKnowledge;
                remainingKnowledge -= deductKnowledge;
            }
            if (player.sharedResources.resources.money > 0)
            {
                int deductMoney = Mathf.Min(remainingMoney, player.sharedResources.resources.money);
                player.sharedResources.resources.money -= deductMoney;
                remainingMoney -= deductMoney;
            }
            if (player.sharedResources.resources.media > 0)
            {
                int deductMedia = Mathf.Min(remainingMedia, player.sharedResources.resources.media);
                player.sharedResources.resources.media -= deductMedia;
                remainingMedia -= deductMedia;
            }
            if (player.sharedResources.resources.labour > 0)
            {
                int deductLabour = Mathf.Min(remainingLabour, player.sharedResources.resources.labour);
                player.sharedResources.resources.labour -= deductLabour;
                remainingLabour -= deductLabour;
            }
            if (player.sharedResources.resources.solidarity > 0)
            {
                int deductSolidarity = Mathf.Min(remainingSolidarity, player.sharedResources.resources.solidarity);
                player.sharedResources.resources.solidarity -= deductSolidarity;
                remainingSolidarity -= deductSolidarity;
            }
            if (player.sharedResources.resources.legitimacy > 0)
            {
                int deductLegitimacy = Mathf.Min(remainingLegitimacy, player.sharedResources.resources.legitimacy);
                player.sharedResources.resources.legitimacy -= deductLegitimacy;
                remainingLegitimacy -= deductLegitimacy;
            }

            // Deduct remaining from personal resources
            player.personalResources.knowledge -= remainingKnowledge;
            player.personalResources.money -= remainingMoney;
            player.personalResources.media -= remainingMedia;
            player.personalResources.labour -= remainingLabour;
            player.personalResources.solidarity -= remainingSolidarity;
            player.personalResources.legitimacy -= remainingLegitimacy;
            
            Debug.Log($"CraftingManager: Deducted remaining resources from personal: K:{remainingKnowledge}, M:{remainingMoney}, Me:{remainingMedia}, L:{remainingLabour}, S:{remainingSolidarity}, Le:{remainingLegitimacy}");
        }
    }

    private void UpdateResourceUI()
    {
        if (player != null && player.uiManager != null)
        {
            player.uiManager.UpdateResourcesUI();
            Debug.Log("CraftingManager.UpdateResourceUI(): Updated resource UI");
        }
        else
        {
            Debug.LogWarning("CraftingManager.UpdateResourceUI(): Cannot update UI - player or uiManager is null");
        }
    }

    public void ApplyCostModifier()
    {
        if (player.preferredApproach != actionCardData.approach) 
        {
            actionCardData.cost = actionCardData.cost * 2;  //double the cost
        }
    }

    public void UpdateCraftButtonState()
    {
        if (craftButton != null)
        {
            craftButton.interactable = CanCraftCard();
            Debug.Log($"CraftingManager.UpdateCraftButtonState(): Button for '{actionCardData?.cardName}' is {(craftButton.interactable ? "interactable" : "not interactable")}");
        }
    }

    public void SetPlayer(PlayerManager newPlayer)
    {
        player = newPlayer;
        UpdateCraftButtonState();
    }
}
