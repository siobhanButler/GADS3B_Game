using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MultiplayerUI : MonoBehaviour
{
    [Header("CurrentPlayer Components")]
    [SerializeField] private Image player0Image;

    [Header("Player 1 Components")]
    [SerializeField] private GameObject player1Panel;
    [SerializeField] private TextMeshProUGUI player1Text;
    [SerializeField] private Image player1Image;
    
    [Header("Player 2 Components")]
    [SerializeField] private GameObject player2Panel;
    [SerializeField] private TextMeshProUGUI player2Text;
    [SerializeField] private Image player2Image;

    [Header("Player 3 Components")]
    [SerializeField] private GameObject player3Panel;
    [SerializeField] private TextMeshProUGUI player3Text;
    [SerializeField] private Image player3Image;

    private GameObject[] playerPanels = new GameObject[4];
    private TextMeshProUGUI[] playerTexts = new TextMeshProUGUI[4];
    private Image[] playerImages = new Image[4];
    private PlayerManager[] players;
    private int totalPlayers;

    private void Awake()
    {
        InitializeReferences();
    }

    public void Setup(PlayerManager[] pPlayers)
    {
        players = pPlayers;
        totalPlayers = players.Length;

        // Panels 1..3 correspond to "next" players after current (player0 uses player0Image only)
        for (int i = 1; i < playerPanels.Length; i++)
        {
            bool shouldBeActive = totalPlayers > i; // show if there is a player for this slot
            if (playerPanels[i] != null) playerPanels[i].SetActive(shouldBeActive);
        }

        UpdatePlayers(0);
    }
    
    public void UpdatePlayers(int currentPlayerIndex)
    {
        if (players == null || totalPlayers <= 0) return;

        PlayerManager[] orderedPlayers = new PlayerManager[totalPlayers];
        currentPlayerIndex = ((currentPlayerIndex % totalPlayers) + totalPlayers) % totalPlayers; // wrap into [0,totalPlayers)
        if(totalPlayers > 0) orderedPlayers[0] = players[currentPlayerIndex];
        if(totalPlayers > 1) orderedPlayers[1] = players[(currentPlayerIndex + 1) % totalPlayers];   //% is a modulo operator that will wrap around the array
        if(totalPlayers > 2) orderedPlayers[2] = players[(currentPlayerIndex + 2) % totalPlayers];
        if(totalPlayers > 3) orderedPlayers[3] = players[(currentPlayerIndex + 3) % totalPlayers];

        Debug.Log($"MultiplayerUI.UpdatePlayer(): Updating players ui");

        // Update current player image (slot 0) if available
        if (player0Image != null && orderedPlayers[0] != null)
        {
            player0Image.color = orderedPlayers[0].playerColor;
        }

        // Update panels 1..3 with next players
        int panelsToFill = Mathf.Min(Mathf.Max(totalPlayers - 1, 0), 3);
        for (int k = 1; k <= 3; k++)
        {
            bool within = k <= panelsToFill;
            GameObject panel = playerPanels[k];
            TextMeshProUGUI txt = playerTexts[k];
            Image img = playerImages[k];

            if (panel != null) panel.SetActive(within);
            if (!within) continue;

            PlayerManager pm = orderedPlayers[k];
            bool hasPlayer = pm != null;
            if (panel != null) panel.SetActive(hasPlayer);
            if (!hasPlayer) continue;
            if (txt != null) txt.text = pm.playerName;
            if (img != null) img.color = pm.playerColor;
        }
    }


    private void InitializeReferences()
    {
        player1Panel = transform.Find("Player1_Panel")?.gameObject;
        player2Panel = transform.Find("Player2_Panel")?.gameObject;
        player3Panel = transform.Find("Player3_Panel")?.gameObject;
        
        if (player1Panel != null)
        {
            if(player1Text == null) player1Text = player1Panel.transform.Find("Player1_Text")?.GetComponent<TextMeshProUGUI>();
            if(player1Image == null) player1Image = player1Panel.transform.Find("Player1Status_Image")?.GetComponent<Image>();
        }
        
        if (player2Panel != null)
        {
            if(player2Text == null) player2Text = player2Panel.transform.Find("Player2_Text")?.GetComponent<TextMeshProUGUI>();
            if(player2Image == null) player2Image = player2Panel.transform.Find("Player2Status_Image")?.GetComponent<Image>();
        }

        if (player3Panel != null)
        {
            if(player3Text == null) player3Text = player3Panel.transform.Find("Player3_Text")?.GetComponent<TextMeshProUGUI>();
            if(player3Image == null) player3Image = player3Panel.transform.Find("Player3Status_Image")?.GetComponent<Image>();
        }

        // Map into arrays: index 1->Player1, 2->Player2, 3->Player3
        playerPanels[1] = player1Panel;
        playerPanels[2] = player2Panel;
        playerPanels[3] = player3Panel;
        playerTexts[1] = player1Text;
        playerTexts[2] = player2Text;
        playerTexts[3] = player3Text;
        playerImages[1] = player1Image;
        playerImages[2] = player2Image;
        playerImages[3] = player3Image;
    }
    
}