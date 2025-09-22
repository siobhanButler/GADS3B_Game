using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerHandUI : MonoBehaviour
{
    [Header("Player Hand Panel")]
    [SerializeField] private GameObject playerHandPanel;
    
    [Header("Card Images")]
    [SerializeField] private Image card1Image;
    [SerializeField] private Image card2Image;
    [SerializeField] private Image card3Image;
    [SerializeField] private Image card4Image;
    [SerializeField] private Image selectedCardImage;
    
    [Header("Navigation Buttons")]
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button selectButton;
    
    [Header("Hand Management")]
    [SerializeField] private int currentSelectedIndex = 0;
    [SerializeField] private List<Sprite> handCards = new List<Sprite>();
    
    private Image[] cardImages;
    
    private void Awake()
    {
        InitializeReferences();
        SetupCardArray();
        SetupButtonListeners();
    }
    
    private void Start()
    {
        UpdateCardDisplay();
        UpdateSelectedCard();
    }
    
    private void InitializeReferences()
    {
        playerHandPanel = transform.Find("PlayerHand_Panel")?.gameObject;
        
        if (playerHandPanel != null)
        {
            card1Image = playerHandPanel.transform.Find("Card1_Img")?.GetComponent<Image>();
            card2Image = playerHandPanel.transform.Find("Card2_Img")?.GetComponent<Image>();
            card3Image = playerHandPanel.transform.Find("Card3_Img")?.GetComponent<Image>();
            card4Image = playerHandPanel.transform.Find("Card4_Img")?.GetComponent<Image>();
            selectedCardImage = playerHandPanel.transform.Find("SelectedCard")?.GetComponent<Image>();
            
            previousButton = playerHandPanel.transform.Find("Button_Previous")?.GetComponent<Button>();
            nextButton = playerHandPanel.transform.Find("Button_Next")?.GetComponent<Button>();
            selectButton = playerHandPanel.transform.Find("Button_Select")?.GetComponent<Button>();
        }
    }
    
    private void SetupCardArray()
    {
        cardImages = new Image[] { card1Image, card2Image, card3Image, card4Image };
    }
    
    private void SetupButtonListeners()
    {
        if (previousButton != null)
            previousButton.onClick.AddListener(OnPreviousCardClicked);
        
        if (nextButton != null)
            nextButton.onClick.AddListener(OnNextCardClicked);
        
        if (selectButton != null)
            selectButton.onClick.AddListener(OnSelectCardClicked);
    }
    
    public void ShowPlayerHandPanel(bool show)
    {
        if (playerHandPanel != null)
            playerHandPanel.SetActive(show);
    }
    
    public void SetHandCards(List<Sprite> newHandCards)
    {
        handCards = new List<Sprite>(newHandCards);
        currentSelectedIndex = Mathf.Clamp(currentSelectedIndex, 0, handCards.Count - 1);
        UpdateCardDisplay();
        UpdateSelectedCard();
    }
    
    public void AddCardToHand(Sprite cardSprite)
    {
        if (cardSprite != null)
        {
            handCards.Add(cardSprite);
            UpdateCardDisplay();
        }
    }
    
    public void RemoveCardFromHand(int index)
    {
        if (index >= 0 && index < handCards.Count)
        {
            handCards.RemoveAt(index);
            
            if (currentSelectedIndex >= handCards.Count && handCards.Count > 0)
                currentSelectedIndex = handCards.Count - 1;
            else if (handCards.Count == 0)
                currentSelectedIndex = 0;
            
            UpdateCardDisplay();
            UpdateSelectedCard();
        }
    }
    
    public void SetCardImage(int cardIndex, Sprite cardSprite)
    {
        if (cardIndex >= 0 && cardIndex < cardImages.Length && cardImages[cardIndex] != null)
        {
            cardImages[cardIndex].sprite = cardSprite;
        }
    }
    
    public void ClearAllCards()
    {
        handCards.Clear();
        currentSelectedIndex = 0;
        UpdateCardDisplay();
        UpdateSelectedCard();
    }
    
    public void SetSelectedCardIndex(int index)
    {
        if (index >= 0 && index < handCards.Count)
        {
            currentSelectedIndex = index;
            UpdateSelectedCard();
        }
    }
    
    public int GetSelectedCardIndex()
    {
        return currentSelectedIndex;
    }
    
    public Sprite GetSelectedCard()
    {
        if (currentSelectedIndex >= 0 && currentSelectedIndex < handCards.Count)
            return handCards[currentSelectedIndex];
        
        return null;
    }
    
    public int GetHandSize()
    {
        return handCards.Count;
    }
    
    public List<Sprite> GetHandCards()
    {
        return new List<Sprite>(handCards);
    }
    
    private void UpdateCardDisplay()
    {
        for (int i = 0; i < cardImages.Length; i++)
        {
            if (cardImages[i] != null)
            {
                if (i < handCards.Count && handCards[i] != null)
                {
                    cardImages[i].sprite = handCards[i];
                    cardImages[i].gameObject.SetActive(true);
                }
                else
                {
                    cardImages[i].sprite = null;
                    cardImages[i].gameObject.SetActive(false);
                }
            }
        }
        
        UpdateNavigationButtons();
    }
    
    private void UpdateSelectedCard()
    {
        if (selectedCardImage != null)
        {
            Sprite selectedSprite = GetSelectedCard();
            if (selectedSprite != null)
            {
                selectedCardImage.sprite = selectedSprite;
                selectedCardImage.gameObject.SetActive(true);
            }
            else
            {
                selectedCardImage.gameObject.SetActive(false);
            }
        }
    }
    
    private void UpdateNavigationButtons()
    {
        if (previousButton != null)
            previousButton.interactable = handCards.Count > 1;
        
        if (nextButton != null)
            nextButton.interactable = handCards.Count > 1;
        
        if (selectButton != null)
            selectButton.interactable = handCards.Count > 0;
    }
    
    private void OnPreviousCardClicked()
    {
        if (handCards.Count <= 1) return;
        
        currentSelectedIndex--;
        if (currentSelectedIndex < 0)
            currentSelectedIndex = handCards.Count - 1;
        
        UpdateSelectedCard();
        
        Debug.Log($"Previous card selected. Current index: {currentSelectedIndex}");
    }
    
    private void OnNextCardClicked()
    {
        if (handCards.Count <= 1) return;
        
        currentSelectedIndex++;
        if (currentSelectedIndex >= handCards.Count)
            currentSelectedIndex = 0;
        
        UpdateSelectedCard();
        
        Debug.Log($"Next card selected. Current index: {currentSelectedIndex}");
    }
    
    private void OnSelectCardClicked()
    {
        if (handCards.Count == 0) return;
        
        Sprite selectedCard = GetSelectedCard();
        if (selectedCard != null)
        {
            Debug.Log($"Card selected at index: {currentSelectedIndex}");
            OnCardSelected(selectedCard, currentSelectedIndex);
        }
    }
    
    protected virtual void OnCardSelected(Sprite selectedCard, int cardIndex)
    {
        // Override this method in derived classes or use events to handle card selection
        Debug.Log($"Card selected: {selectedCard.name} at index {cardIndex}");
    }
    
    public Button GetPreviousButton()
    {
        return previousButton;
    }
    
    public Button GetNextButton()
    {
        return nextButton;
    }
    
    public Button GetSelectButton()
    {
        return selectButton;
    }
    
    public GameObject GetPlayerHandPanel()
    {
        return playerHandPanel;
    }
}