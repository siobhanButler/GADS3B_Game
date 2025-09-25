using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public List<EventData> megaEvents = new List<EventData>();
    public List<EventData> macroEvents = new List<EventData>();
    public List<EventData> microEvents = new List<EventData>();

    public List<Event> currentEvents = new List<Event>();

    [SerializeField] private GameObject eventPanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button closeEventUIButton;

    private Event eventAddedThisTurn = null;        //set every new turn, populated when event is added that turn
    private System.Random random = new System.Random();
    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closeEventUIButton.onClick.AddListener(OnCloseEventUIButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(GameManager pGameManager)
    {
        gameManager = pGameManager;
    }

    void OnEnable()
    {
        RoundManager.OnNewTurn += HandleNewTurn;
        RoundManager.OnNewRound += HandleNewRound;
        RoundManager.OnNextPlayer += HandleNextPlayer;
    }

    void OnDisable()
    {
        RoundManager.OnNewTurn -= HandleNewTurn;
        RoundManager.OnNewRound -= HandleNewRound;
        RoundManager.OnNextPlayer -= HandleNextPlayer;
    }

    public Event GenerateEvent(EEventType eventType)
    {
        EventData newEventData = null;
        List<SectorManager> targetSectors = new List<SectorManager>();

        switch (eventType)
        {
            case EEventType.Mega:
                newEventData = megaEvents[random.Next(0, megaEvents.Count)];
                CountryManager[] megaCountries = gameManager.countries;         //targets all countries, multiple sectors
                targetSectors = GetTargetSectors(newEventData.sectorType, megaCountries);
                break;
            case EEventType.Macro:
                newEventData = macroEvents[random.Next(0, macroEvents.Count)];
                CountryManager[] macroCountries = new CountryManager[1];        //targets one country, multiple (all) sectors
                macroCountries[0] = gameManager.countries[random.Next(0, gameManager.countries.Length)];    //one random country
                targetSectors = GetTargetSectors(newEventData.sectorType, macroCountries);
                break;
            case EEventType.Micro:
                newEventData = microEvents[random.Next(0, microEvents.Count)];
                CountryManager[] microCountries = new CountryManager[1];        //targets one country, one sector
                microCountries[0] = gameManager.countries[random.Next(0, gameManager.countries.Length)];    //one random country
                targetSectors = GetTargetSectors(newEventData.sectorType, microCountries);
                break;
        }

        Event newEvent = new Event(newEventData, targetSectors);
        currentEvents.Add(newEvent);
        EnableEventUI(true, newEvent);
        return newEvent;
    }

    private List<SectorManager> GetTargetSectors(ESectorType type, CountryManager[] countries)
    {
        List<SectorManager> sectors = new List<SectorManager>();

        foreach (CountryManager country in countries)
        {
            switch (type)
            {
                case ESectorType.Activism:
                    sectors.Add(country.activismSector); 
                    break;
                case ESectorType.Social:
                    sectors.Add(country.socialSector);
                    break;
                case ESectorType.Economic:
                    sectors.Add(country.economicSector);
                    break;
                case ESectorType.Government:
                    sectors.Add(country.governmentSector);
                    break;
                case ESectorType.Media:
                    sectors.Add(country.mediaSector);
                    break;
                case ESectorType.Universal:
                    sectors.Add(country.activismSector);
                    sectors.Add(country.socialSector);
                    sectors.Add(country.economicSector);
                    sectors.Add(country.governmentSector);
                    sectors.Add(country.mediaSector);
                    break;
            }
        }

        return sectors;
    }

    private void HandleNewTurn(int roundNumber, int turnNumber)
    {
        eventAddedThisTurn = null;

        if (turnNumber == 3 || turnNumber == 9)
            eventAddedThisTurn = GenerateEvent(EEventType.Macro);    //Macro events generated twice per round
        if (turnNumber == 2 || turnNumber == 4 || turnNumber == 6 || turnNumber == 8)
            eventAddedThisTurn = GenerateEvent(EEventType.Micro);  //Micro Events 4 times per round 

        List<Event> eventsToBeRemoved = new List<Event>();
        //Apply the effects for all the events currently active
        foreach (Event mEvent in currentEvents) //have to use mEvent because event is a data type
        {
            mEvent.ApplyEventEffect();
            if(mEvent.eventLifetime <= 0) eventsToBeRemoved.Add(mEvent);     //I dont know how to delete/destroy
        }
        foreach(Event mEvent in eventsToBeRemoved)
        {
            currentEvents.Remove(mEvent);
        }
        eventsToBeRemoved.Clear();
    }

    private void HandleNewRound(int roundNumber)
    {
        eventAddedThisTurn = GenerateEvent(EEventType.Mega);     //one mega event per round
    }

    private void HandleNextPlayer()
    {
        if (eventAddedThisTurn == null) return;

        //The newly added event is displayed once per player
        EnableEventUI(true, eventAddedThisTurn);
    }

    public void EnableEventUI(bool enable, Event mEvent)
    {
        if (eventPanel != null) eventPanel.SetActive(enable);
        if (mEvent == null) return;

        string desription = mEvent.eventDescription + "\n \n Effect: " 
                                + mEvent.eventEffect + "\n Duration: " 
                                + mEvent.eventLifetime.ToString() + " turns.";

        if (nameText != null) nameText.text = mEvent.eventType.ToString() + " Event! \n" + mEvent.eventName;
        if (descriptionText != null) descriptionText.text = desription;
    }

    private void OnCloseEventUIButtonClick()
    {
        EnableEventUI(false, null);
    }
}
