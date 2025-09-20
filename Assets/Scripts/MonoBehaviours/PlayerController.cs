using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Simple PlayerController that handles input and delegates to ClickDetector
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ClickDetector clickDetector;
    
    void Start()
    {
        // Find ClickDetector if not assigned
        if (clickDetector == null)
        {
            clickDetector = GetComponent<ClickDetector>();
        }
        
        if (clickDetector == null)
        {
            Debug.LogError("PlayerController: No ClickDetector found! Please assign one or add it to the scene.");
        }
    }
    
    void Update()
    {
        HandleInput();
    }
    
    /// <summary>
    /// Handles player input using new Input System
    /// </summary>
    private void HandleInput()
    {
        // Left click - delegate to ClickDetector
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // ClickDetector already handles this in its Update()
            // No need to duplicate the logic
        }
        
        // Right click - could add different behavior here
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            HandleRightClick();
        }
    }
    
    /// <summary>
    /// Handle right click (different from left click)
    /// </summary>
    private void HandleRightClick()
    {
        Debug.Log("Right click - could be used for context menu or different action");
        // Add your right-click specific logic here
    }
}
