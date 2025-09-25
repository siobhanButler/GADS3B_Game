using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandUI : MonoBehaviour
{
    public PlayerManager currentPlayer;
    public const int maxCards = 5;  //how many cards can be displayed

    [Header("Player Hand Panel")]
    [SerializeField] private GameObject playerHandPanel;
    
    [Header("Card Images")]
    [SerializeField] private Image card1Image;  //index 0
    [SerializeField] private Image card2Image;  //index 1
    [SerializeField] private Image card3Image;  //index 3
    [SerializeField] private Image card4Image;  //index 4
    [SerializeField] private Image selectedCardImage;   //so index 2
    [SerializeField] private Outline selectedCardOutline;
    
    [Header("Navigation Buttons")]
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button selectButton;
    
    [Header("Play Card Panel")]
    [SerializeField] private GameObject playCardPanel;
    [SerializeField] private TextMeshProUGUI playCardPromptText;
    [SerializeField] private Image cardToPlayImage;
    [SerializeField] private Button playCardYesButton;
    [SerializeField] private Button playCardNoButton;
    
    private int currentStartIndex;
    
    [SerializeField] private List<CardManager> cardsInHand = new List<CardManager>();
    [SerializeField] private CardManager[] cardsDisplayed = new CardManager[5];
    [SerializeField] private Image[] images = new Image[5];
    [SerializeField] private bool isCardSelected;

    private CardManager cardToPlay;
    private ICardTarget cardTarget;
    
    private void Awake()
    {
        InitializeReferences();
        SetupButtonListeners();
    }
    
    private void Start()
    {
        Setup();
        UpdateCardDisplay();
        playCardPanel.SetActive(false);   
    }

    private void Update()
    {
        
    }

    private void Setup()
    {
        currentStartIndex = 0;
        isCardSelected = false;

        images[0] = card1Image;
        images[1] = card2Image;
        images[2] = selectedCardImage;
        images[3] = card3Image;
        images[4] = card4Image;

        cardToPlay = null;
        cardTarget = null;
    }

    public void UpdateHandUI(PlayerManager player)
    {
        currentPlayer = player;
        cardsInHand = currentPlayer.handManager.hand;   //get the cards of the new player

        UpdateCardDisplay();
    }
    
    public void ShowPlayCardPanel(bool show, CardManager card, ICardTarget target)
    {
        // Only block showing when an action was already taken; always allow hiding
        if (show && currentPlayer != null && currentPlayer.tookActionThisTurn) return;

        if (playCardPanel != null)
            playCardPanel.SetActive(show);

        if (show)
        {
            cardToPlay = card;
            cardTarget = target;

            if (cardToPlayImage != null)
                cardToPlayImage.sprite = cardToPlay != null ? cardToPlay.cardSprite : null;

            if (playCardPromptText != null)
                playCardPromptText.text = (cardToPlay != null && target != null)
                    ? $"Are you sure you want to play {cardToPlay.cardName} on {target.TargetName}?"
                    : string.Empty;
        }
        else
        {
            // Clearing references when hiding ensures panel does not re-open with stale data
            cardToPlay = null;
            cardTarget = null;
            if (cardToPlayImage != null) cardToPlayImage.sprite = null;
            if (playCardPromptText != null) playCardPromptText.text = string.Empty;
        }
    }

    
    private void UpdateCardDisplay()
    {
        Debug.Log($"PlayerHandUI.UpdateCardDisplay(): Updating display. Hand has {cardsInHand.Count} cards, currentStartIndex: {currentStartIndex}");
        
        // Clear all displayed cards first
        for (int i = 0; i < cardsDisplayed.Length; i++)
        {
            cardsDisplayed[i] = null;
            if (images[i] != null)
            {
                images[i].sprite = null;
                images[i].gameObject.SetActive(false);
            }
        }

        if (cardsInHand.Count > 0)
        {
            // Calculate starting slot based on number of cards
            int startSlot = GetStartingSlot(cardsInHand.Count);
            Debug.Log($"PlayerHandUI.UpdateCardDisplay(): Starting slot for {cardsInHand.Count} cards: {startSlot}");
            
            // Fill display slots with cards from hand
            for (int i = 0; i < cardsInHand.Count; i++)
            {
                int displaySlot = (startSlot + i) % cardsDisplayed.Length;
                int handIndex = (currentStartIndex + i) % cardsInHand.Count;
                
                if (cardsInHand[handIndex] != null)
                {
                    cardsDisplayed[displaySlot] = cardsInHand[handIndex];
                    if (images[displaySlot] != null)
                    {
                        images[displaySlot].sprite = cardsInHand[handIndex].cardSprite;
                        images[displaySlot].gameObject.SetActive(true);
                    }
                    Debug.Log($"PlayerHandUI.UpdateCardDisplay(): Display slot {displaySlot} shows card '{cardsInHand[handIndex].cardName}' (hand index {handIndex})");
                }
            }
        }
        
        Debug.Log($"PlayerHandUI.UpdateCardDisplay(): Display updated. Showing {cardsInHand.Count} cards");
    }

    private int GetStartingSlot(int cardCount)
    {
        // Determine starting slot based on card count
        switch (cardCount)
        {
            case 1: return 2; // Start in slot 2 (middle)
            case 2: return 2; // Start in slot 2
            case 3: return 1; // Start in slot 1
            case 4: return 1; // Start in slot 1
            default: return 0; // 5 or more cards start in slot 0
        }
    }
    
    private void OnPreviousCardClicked()
    {
        // Always rotate through cards
        if (cardsInHand.Count > 0)
        {
            currentStartIndex = (currentStartIndex - 1 + cardsInHand.Count) % cardsInHand.Count;
            UpdateCardDisplay();
            Debug.Log($"PlayerHandUI.OnPreviousCardClicked(): Rotated to previous. New start index: {currentStartIndex}");
        }
        if (isCardSelected)     //If a card is selected, but player is moving away, then Deselect card
        {
            if (currentPlayer != null) currentPlayer.SelectCard(null);
            if (selectedCardOutline != null) selectedCardOutline.effectDistance = new Vector2(1, 1);
            isCardSelected = false;
        }
    }
    
    private void OnNextCardClicked()
    {
        // Always rotate through cards
        if (cardsInHand.Count > 0)
        {
            currentStartIndex = (currentStartIndex + 1) % cardsInHand.Count;
            UpdateCardDisplay();
            Debug.Log($"PlayerHandUI.OnNextCardClicked(): Rotated to next. New start index: {currentStartIndex}");
        }
        if (isCardSelected)     //If a card is selected, but player is moving away, then Deselect card
        {
            if (currentPlayer != null) currentPlayer.SelectCard(null);
            if (selectedCardOutline != null) selectedCardOutline.effectDistance = new Vector2(1, 1);
            isCardSelected = false;
        }
    }

    private void OnSelectCardClicked()
    {
        if (isCardSelected)     //Deselect card
        {
            if (currentPlayer != null) currentPlayer.SelectCard(null);
            if (selectedCardOutline != null) selectedCardOutline.effectDistance = new Vector2(1, 1);
            isCardSelected = false;
        }
        else        //Select card
        {
            if (cardsDisplayed[2] != null && currentPlayer != null) // Use index 2 (selectedCardImage position)
            {
                currentPlayer.SelectCard(cardsDisplayed[2]);
                if (selectedCardOutline != null) selectedCardOutline.effectDistance = new Vector2(3, 3);
                isCardSelected = true;
            }
        }       
    }

    private void OnPlayCardYesClicked()
    {
        Debug.Log("Play card Yes button clicked");
        
        // Validate that we have the required components
        if (currentPlayer == null)
        {
            Debug.LogError("PlayerHandUI.OnPlayCardYesClicked(): currentPlayer is null!");
            return;
        }
        
        if (cardToPlay == null)
        {
            Debug.LogError("PlayerHandUI.OnPlayCardYesClicked(): cardToPlay is null!");
            return;
        }
        
        if (cardTarget == null)
        {
            Debug.LogError("PlayerHandUI.OnPlayCardYesClicked(): cardTarget is null!");
            return;
        }
        
        Debug.Log($"PlayerHandUI.OnPlayCardYesClicked(): Playing card '{cardToPlay.cardName}' on target '{cardTarget.TargetName}'");
        
        //Call PlayerManager's play card with the stored references
        currentPlayer.PlayCard(cardToPlay, cardTarget);

        if (currentPlayer != null) currentPlayer.SelectCard(null);
        
        ShowPlayCardPanel(false, null, null);   //hide play card (this clears cardToPlay and cardTarget)
        if (selectedCardOutline != null) selectedCardOutline.effectDistance = new Vector2(1, 1);
        isCardSelected = false;
    }

    private void OnPlayCardNoClicked()
    {
        Debug.Log("Play card No button clicked");
        ShowPlayCardPanel(false, null, null);
    }

    protected virtual void OnCardSelected(Sprite selectedCard, int cardIndex)
    {
        // Override this method in derived classes or use events to handle card selection
        Debug.Log($"Card selected: {selectedCard.name} at index {cardIndex}");
    }

    private void InitializeReferences()
    {
        if (playerHandPanel == null) playerHandPanel = transform.Find("PlayerHand_Panel")?.gameObject;

        if (playerHandPanel != null)
        {
            if (card1Image == null) card1Image = playerHandPanel.transform.Find("Card1_Img")?.GetComponent<Image>();
            if (card2Image == null) card2Image = playerHandPanel.transform.Find("Card2_Img")?.GetComponent<Image>();
            if (card3Image == null) card3Image = playerHandPanel.transform.Find("Card3_Img")?.GetComponent<Image>();
            if (card4Image == null) card4Image = playerHandPanel.transform.Find("Card4_Img")?.GetComponent<Image>();
            if (selectedCardImage == null) selectedCardImage = playerHandPanel.transform.Find("SelectedCard")?.GetComponent<Image>();
            if (selectedCardOutline == null) selectedCardOutline = selectedCardImage != null ? selectedCardImage.GetComponent<Outline>() : null;

            if (previousButton == null) previousButton = playerHandPanel.transform.Find("Button_Previous")?.GetComponent<Button>();
            if (nextButton == null) nextButton = playerHandPanel.transform.Find("Button_Next")?.GetComponent<Button>();
            if (selectButton == null) selectButton = playerHandPanel.transform.Find("Button_Select")?.GetComponent<Button>();
        }

        // Initialize play card panel references
        if (playCardPanel == null) playCardPanel = transform.Find("PlayCard_Panel")?.gameObject;
        if (playCardPanel != null)
        {
            if (playCardPromptText == null) playCardPromptText = playCardPanel.transform.Find("Prompt_Text")?.GetComponent<TextMeshProUGUI>();
            if(cardToPlayImage == null) cardToPlayImage = playCardPanel.transform.Find("CardToPlay_Image")?.GetComponent<Image>();
            if (playCardYesButton == null) playCardYesButton = playCardPanel.transform.Find("Yes_Button")?.GetComponent<Button>();
            if (playCardNoButton == null) playCardNoButton = playCardPanel.transform.Find("No_Button")?.GetComponent<Button>();
        }
    }

    private void SetupButtonListeners()
    {
        if (previousButton != null)
            previousButton.onClick.AddListener(OnPreviousCardClicked);

        if (nextButton != null)
            nextButton.onClick.AddListener(OnNextCardClicked);

        if (selectButton != null)
            selectButton.onClick.AddListener(OnSelectCardClicked);

        // Setup play card panel button listeners
        if (playCardYesButton != null)
            playCardYesButton.onClick.AddListener(OnPlayCardYesClicked);

        if (playCardNoButton != null)
            playCardNoButton.onClick.AddListener(OnPlayCardNoClicked);
    }
}
