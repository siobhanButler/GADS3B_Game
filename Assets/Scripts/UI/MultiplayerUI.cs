using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MultiplayerUI : MonoBehaviour
{
    [Header("Player 1 Components")]
    [SerializeField] private GameObject player1Panel;
    [SerializeField] private TextMeshProUGUI player1Text;
    [SerializeField] private Image player1Image;
    
    [Header("Player 2 Components")]
    [SerializeField] private GameObject player2Panel;
    [SerializeField] private TextMeshProUGUI player2Text;
    [SerializeField] private Image player2Image;
    
    [Header("Player Status")]
    [SerializeField] private Color activePlayerColor = Color.green;
    [SerializeField] private Color inactivePlayerColor = Color.gray;
    [SerializeField] private bool player1Active = true;
    [SerializeField] private bool player2Active = false;
    
    private void Awake()
    {
        InitializeReferences();
        UpdatePlayerStates();
    }
    
    private void InitializeReferences()
    {
        player1Panel = transform.Find("Player1_Panel")?.gameObject;
        player2Panel = transform.Find("Player2_Panel")?.gameObject;
        
        if (player1Panel != null)
        {
            player1Text = player1Panel.transform.Find("Player1_Text")?.GetComponent<TextMeshProUGUI>();
            player1Image = player1Panel.transform.Find("Player1_Image")?.GetComponent<Image>();
        }
        
        if (player2Panel != null)
        {
            player2Text = player2Panel.transform.Find("Player2_Text")?.GetComponent<TextMeshProUGUI>();
            player2Image = player2Panel.transform.Find("Player2_Image")?.GetComponent<Image>();
        }
    }
    
    public void SetPlayer1Name(string playerName)
    {
        if (player1Text != null)
            player1Text.text = playerName;
    }
    
    public void SetPlayer2Name(string playerName)
    {
        if (player2Text != null)
            player2Text.text = playerName;
    }
    
    public void SetPlayer1Image(Sprite playerSprite)
    {
        if (player1Image != null)
            player1Image.sprite = playerSprite;
    }
    
    public void SetPlayer2Image(Sprite playerSprite)
    {
        if (player2Image != null)
            player2Image.sprite = playerSprite;
    }
    
    public void SetPlayer1Active(bool isActive)
    {
        player1Active = isActive;
        UpdatePlayerStates();
    }
    
    public void SetPlayer2Active(bool isActive)
    {
        player2Active = isActive;
        UpdatePlayerStates();
    }
    
    public void SetActivePlayer(int playerNumber)
    {
        switch (playerNumber)
        {
            case 1:
                SetPlayer1Active(true);
                SetPlayer2Active(false);
                break;
            case 2:
                SetPlayer1Active(false);
                SetPlayer2Active(true);
                break;
            default:
                Debug.LogWarning("Invalid player number. Use 1 or 2.");
                break;
        }
    }
    
    public void ToggleActivePlayer()
    {
        if (player1Active)
        {
            SetActivePlayer(2);
        }
        else
        {
            SetActivePlayer(1);
        }
    }
    
    public int GetActivePlayer()
    {
        if (player1Active) return 1;
        if (player2Active) return 2;
        return 0;
    }
    
    public bool IsPlayer1Active()
    {
        return player1Active;
    }
    
    public bool IsPlayer2Active()
    {
        return player2Active;
    }
    
    public void ShowPlayer1Panel(bool show)
    {
        if (player1Panel != null)
            player1Panel.SetActive(show);
    }
    
    public void ShowPlayer2Panel(bool show)
    {
        if (player2Panel != null)
            player2Panel.SetActive(show);
    }
    
    public void ShowBothPlayerPanels(bool show)
    {
        ShowPlayer1Panel(show);
        ShowPlayer2Panel(show);
    }
    
    private void UpdatePlayerStates()
    {
        UpdatePlayerVisualState(player1Image, player1Active);
        UpdatePlayerVisualState(player2Image, player2Active);
    }
    
    private void UpdatePlayerVisualState(Image playerImage, bool isActive)
    {
        if (playerImage != null)
        {
            playerImage.color = isActive ? activePlayerColor : inactivePlayerColor;
        }
    }
    
    public void SetPlayerColors(Color activeColor, Color inactiveColor)
    {
        activePlayerColor = activeColor;
        inactivePlayerColor = inactiveColor;
        UpdatePlayerStates();
    }
    
    public GameObject GetPlayer1Panel()
    {
        return player1Panel;
    }
    
    public GameObject GetPlayer2Panel()
    {
        return player2Panel;
    }
    
    public TextMeshProUGUI GetPlayer1Text()
    {
        return player1Text;
    }
    
    public TextMeshProUGUI GetPlayer2Text()
    {
        return player2Text;
    }
    
    public Image GetPlayer1Image()
    {
        return player1Image;
    }
    
    public Image GetPlayer2Image()
    {
        return player2Image;
    }
}