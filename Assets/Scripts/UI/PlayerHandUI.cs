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
    
    private List<CardManager> cardsInHand;
    private CardManager[] cardsDisplayed = new CardManager[5];
    private Image[] images = new Image[5];
    private bool isCardSelected;

    private CardManager cardToPlay;
    private ICardTarget cardTarget;
    
    private void Awake()
    {
        InitializeReferences();
        SetupButtonListeners();
    }
    
    private void Start()
    {
        UpdateCardDisplay();
        playCardPanel.SetActive(false);
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
    }

    public void UpdateHandUI(PlayerManager player)
    {
        currentPlayer = player;
        cardsInHand = currentPlayer.handManager.hand;   //get the cards of the new player

        UpdateCardDisplay();
    }
    
    public void ShowPlayCardPanel(bool show, CardManager card, ICardTarget target)
    {
        if (playCardPanel != null)
            playCardPanel.SetActive(show);

        cardToPlay = card;
        cardTarget = target;

        if (cardToPlay != null && cardToPlayImage != null)
            cardToPlayImage.sprite = cardToPlay.cardSprite;
            
        if (playCardPromptText != null && target != null)
            playCardPromptText.text = $"Are you sure you want to play {cardToPlay?.cardName} on {target.TargetName}?";
    }

    
    private void UpdateCardDisplay()
    {
        int arrayStart = Mathf.Clamp(cardsDisplayed.Length - cardsInHand.Count, 0, 5);  //if there arent enough cards, from where will they start populating
        for (int i = 0, j = currentStartIndex; (i < cardsDisplayed.Length); i++, j++)
        {
            if(i < arrayStart)  //it has no card, so it must be deactivated
            {
                images[i].gameObject.SetActive(false);
            }
            else
            {
                cardsInHand[j] = cardsDisplayed[i];
                images[i].sprite = cardsDisplayed[i].cardSprite;
                images[i].gameObject.SetActive(true);
            }
        }
    }
    
    private void OnPreviousCardClicked()
    {
        //shift all images to the right, so image1 = image0, image 2 = image1 etc
        currentStartIndex -= 1;
        UpdateCardDisplay();
    }
    
    private void OnNextCardClicked()
    {
        //shift all images to the left, so image1 = image2, image 2 = image 3 etc
        currentStartIndex += 1;
        UpdateCardDisplay();
    }

    private void OnSelectCardClicked()
    {
        if (isCardSelected)     //Deselect card
        {
            currentPlayer.SelectCard(null);
            selectedCardOutline.effectDistance = new Vector2(1, 1);
        }
        else        //Select card
        {
            currentPlayer.SelectCard(cardsDisplayed[3]);
            selectedCardOutline.effectDistance = new Vector2(3, 3);
        }       
    }

    private void OnPlayCardYesClicked()
    {
        Debug.Log("Play card Yes button clicked");
        ShowPlayCardPanel(false, null, null);   //hide play card

        //Call PlayerManager's play card
        currentPlayer.PlayCard(cardToPlay, cardTarget);
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
            if (selectedCardOutline == null) selectedCardOutline = selectedCardOutline.GetComponent<Outline>();

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
            if (cardToPlayImage == null) playCardYesButton = playCardPanel.transform.Find("Yes_Button")?.GetComponent<Button>();
            if (cardToPlayImage == null) playCardNoButton = playCardPanel.transform.Find("No_Button")?.GetComponent<Button>();
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