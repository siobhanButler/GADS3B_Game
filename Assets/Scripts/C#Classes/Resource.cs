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

    // Multiplication operator for scaling resources by a float value
    public static Resource operator *(Resource a, float multiplier)
    {
        return new Resource(
            Mathf.RoundToInt(a.knowledge * multiplier),
            Mathf.RoundToInt(a.money * multiplier),
            Mathf.RoundToInt(a.media * multiplier),
            Mathf.RoundToInt(a.labour * multiplier),
            Mathf.RoundToInt(a.solidarity * multiplier),
            Mathf.RoundToInt(a.legitimacy * multiplier)
        );
    }

    // Division operator for scaling resources down by an float
    public static Resource operator /(Resource a, float divisor)
    {
        if (divisor == 0)
        {
            Debug.LogError("Resource division by zero! Returning zero resource.");
            return Resource.Zero;
        }
        
        return new Resource(
            Mathf.RoundToInt(a.knowledge / divisor),
            Mathf.RoundToInt(a.money / divisor),
            Mathf.RoundToInt(a.media / divisor),
            Mathf.RoundToInt(a.labour / divisor),
            Mathf.RoundToInt(a.solidarity / divisor),
            Mathf.RoundToInt(a.legitimacy / divisor)
        );
    }

    // Method to scale resources by a float multiplier (alternative to operator)
    public Resource ScaleBy(float multiplier)
    {
        return this * multiplier;
    }

    // Method to scale resources by a percentage (0.0 to 1.0)
    public Resource ScaleByPercentage(float percentage)
    {
        return this * percentage;
    }

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