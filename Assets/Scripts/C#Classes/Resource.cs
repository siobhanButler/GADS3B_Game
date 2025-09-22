using UnityEngine;

[System.Serializable]
public struct Resource
{
    public int knowledge;
    public int money;
    public int media;
    public int labour;
    public int solidarity;
    public int legitimacy;

    public Resource(int knowledge, int money, int media, int labour, int solidarity, int legitimacy)
    {
        this.knowledge = knowledge;
        this.money = money;
        this.media = media;
        this.labour = labour;
        this.solidarity = solidarity;
        this.legitimacy = legitimacy;
    }

    // Static zero resource for initialization
    public static Resource Zero => new Resource(0, 0, 0, 0, 0, 0);

    // Addition operator for combining resources
    public static Resource operator +(Resource a, Resource b)
    {
        return new Resource(
            a.knowledge + b.knowledge,
            a.money + b.money,
            a.media + b.media,
            a.labour + b.labour,
            a.solidarity + b.solidarity,
            a.legitimacy + b.legitimacy
        );
    }

    // Subtraction operator for spending resources
    public static Resource operator -(Resource a, Resource b)
    {
        return new Resource(
            a.knowledge - b.knowledge,
            a.money - b.money,
            a.media - b.media,
            a.labour - b.labour,
            a.solidarity - b.solidarity,
            a.legitimacy - b.legitimacy
        );
    }

    /*
    public static Resource operator /(Resource a, int b)
    {
        return new Resource()
        {
            a.knowledge / b,
            a.money / b,
            a.media / b,
            a.labour / b,
            a.solidarity / b,
            a.legitimacy / b
        };
    }
    */

    // Check if we can afford a cost (all values must be >= 0 after subtraction)
    public bool CanAfford(Resource cost)
    {
        var result = this - cost;
        return result.knowledge >= 0 && result.money >= 0 && result.media >= 0 && 
               result.labour >= 0 && result.solidarity >= 0 && result.legitimacy >= 0;
    }

    // Get total resource value (useful for debugging/balancing)
    public int GetTotalValue()
    {
        return knowledge + money + media + labour + solidarity + legitimacy;
    }

    // Check if any resource is negative (for shared pool tracking)
    public bool HasNegativeResources()
    {
        return knowledge < 0 || money < 0 || media < 0 || 
               labour < 0 || solidarity < 0 || legitimacy < 0;
    }

    // Override ToString for debugging
    public override string ToString()
    {
        return $"Knowledge: {knowledge}, \n Money: {money}, \n Media: {media}, \n Labour: {labour}, \n Solidarity: {solidarity}, \n Legitimacy: {legitimacy}";
    }

    public void AddKnowledge(int amount)
    {
        knowledge += amount;
    }
    
    public void AddMoney(int amount)
    {
        money += amount;
    }
    
    public void AddMedia(int amount)
    {
        media += amount;
    }
    
    public void AddLabour(int amount)
    {
        labour += amount;
    }
    
    public void AddSolidarity(int amount)
    {
        solidarity += amount;
    }
    
    public void AddLegitimacy(int amount)
    {
        legitimacy += amount;
    }
}