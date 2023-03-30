using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryView : MonoBehaviour
{
    [SerializeField]
    VisualTreeAsset ListEntryTemplate;

    void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();

        // Initialize the character list controller
        var inventoryListController = new InventoryListController();
        inventoryListController.InitializeInventoryList(uiDocument.rootVisualElement, ListEntryTemplate);
    }
}
}
