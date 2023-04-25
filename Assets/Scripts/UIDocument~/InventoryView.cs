using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryView : MonoBehaviour
{
    [SerializeField]
    private VisualTreeAsset invListEntryTemplate;
    [SerializeField]
    private VisualTreeAsset statListEntryTemplate;
    private JacksonCharacterMovement _player;
    private UIDocument _uiDocument;
    private InventoryListController _inventoryListController;

    // Open by default. Will eventually open/toggle on key press
    void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        _uiDocument = GetComponent<UIDocument>();
        
        // Initialize the character list controller
        _inventoryListController = new InventoryListController();
        
        // Retrieve the player from the parent
        _player = transform.parent.GetComponentInChildren<JacksonCharacterMovement>();
        ToggleUI();
    }
    public void UpdateUI()
    {
        _inventoryListController.InitializeInventoryList(_uiDocument.rootVisualElement, invListEntryTemplate, statListEntryTemplate, _player);
    }

    public void ToggleUI()
    {
        _uiDocument.rootVisualElement.visible = !_uiDocument.rootVisualElement.visible;
    }
}
