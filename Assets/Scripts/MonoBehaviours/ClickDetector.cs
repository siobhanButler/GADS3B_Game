using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// MonoBehaviour component that detects mouse clicks on GameObjects and calls OnClick() on IClickable components
/// </summary>
public class ClickDetector : MonoBehaviour
{
    [Header("Click Detection Settings")]
    [SerializeField] private LayerMask clickableLayerMask = -1; // All layers by default
    [SerializeField] private Camera targetCamera; // If null, will use Camera.main
    
    private Camera cam;
    
    void Start()
    {
        // Get camera reference
        if (targetCamera != null)
            cam = targetCamera;
        else
            cam = Camera.main;
            
        if (cam == null)
        {
            Debug.LogError("ClickDetector Start(): No camera found! Please assign a camera or ensure Camera.main exists.");
        }
    }
    
    void Update()
    {
        // Check for mouse click using new Input System
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleClick();
        }
    }
    
    /// <summary>
    /// Handles mouse click detection using raycasting
    /// </summary>
    private void HandleClick()
    {
        if (cam == null) return;
        
        // Create ray from camera through mouse position using new Input System
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        
        // Perform raycast
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayerMask))
        {
            GameObject clickedObject = hit.collider.gameObject;
            
            // Get all IClickable components on the clicked object
            IClickable[] clickableComponents = clickedObject.GetComponents<IClickable>();
            
            // Call OnClick() on all IClickable components on the clicked object first
            foreach (IClickable clickable in clickableComponents)
            {
                clickable.OnClick(this);
            }
            
            // If no IClickable found on the clicked object, check parent objects
            if (clickableComponents.Length == 0)
            {
                Transform parent = clickedObject.transform.parent;
                while (parent != null)
                {
                    IClickable[] parentClickables = parent.GetComponents<IClickable>();
                    foreach (IClickable clickable in parentClickables)
                    {
                        clickable.OnClick(this);
                    }
                    parent = parent.parent;
                }
            }
        }
    }
    
    /// <summary>
    /// Alternative method for UI elements (if using Unity UI)
    /// </summary>
    public void HandleUIClick(GameObject clickedObject)
    {
        IClickable[] clickableComponents = clickedObject.GetComponents<IClickable>();
        
        foreach (IClickable clickable in clickableComponents)
        {
            clickable.OnClick(this);
        }
    }
}
