using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryListController
{
    // UXML template for list entries
    VisualTreeAsset ItemEntryTemplate;

    // UI element references
    ListView InventoryList;
    
    // List of items from inventory
    private List<Item> _items;
    
    public void InitializeInventoryList(VisualElement root, VisualTreeAsset itemEntryTemplate, JacksonPlayerMovement player)
    {
        _items = player.GetInventory();

        // Store a reference to the template for the list entries
        ItemEntryTemplate = itemEntryTemplate;

        // Store a reference to the item list element
        InventoryList = root.Q<ListView>("Inventory-List");

        FillInventoryList();

        // Register to get a callback when an item is selected
        // InventoryList.onSelectionChange += OnCharacterSelected;
    }
    
    // Fill the inventory with items
    void FillInventoryList()
    {
        // Set up a make item function for a list entry
        InventoryList.makeItem = () =>
        {
            // Instantiate the UXML template for the entry
            var newListEntry = ItemEntryTemplate.Instantiate();

            // Instantiate a controller for the data
            var newListEntryLogic = new ItemListEntryController();

            // Assign the controller script to the visual element
            newListEntry.userData = newListEntryLogic;

            // Initialize the controller script
            newListEntryLogic.SetVisualElement(newListEntry);

            // Return the root of the instantiated visual tree
            return newListEntry;
        };

        // Set up bind function for a specific list entry
        InventoryList.bindItem = (item, index) =>
        {
            // Debug.Log("Item name in list controller: " + _items[index].name);
            (item.userData as ItemListEntryController).SetItemData(_items[index]);
        };

        // Set a fixed item height
        InventoryList.fixedItemHeight = 100;

        // Set the actual item's source list/array
        InventoryList.itemsSource = _items;
    }
}