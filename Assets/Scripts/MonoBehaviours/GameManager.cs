using UnityEngine;

public class GameManager : MonoBehaviour
{
    public RoundManager roundManager;
    public UIManager uiManager;

    public PlayerManager[] players;
    public CountryManager[] countries;
    public SectorManager[] sectors;

    public float maxPeacefulPoints;
    public float peacefulPoints;
    public float maxViolentPoints;
    public float violentPoints;

    public float resourcesToTopInfluencingPlayer;
    public float resourcesToLastInfluencingPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Setup()
    {
        if (roundManager == null) roundManager = Object.FindFirstObjectByType<RoundManager>();
        if (roundManager == null) uiManager = Object.FindFirstObjectByType<UIManager>();
        
        players = FindObjectsByType<PlayerManager>(FindObjectsSortMode.None);
        countries = FindObjectsByType<CountryManager>(FindObjectsSortMode.None);
        sectors = FindObjectsByType<SectorManager>(FindObjectsSortMode.None);
    }
}
