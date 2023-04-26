using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryListController
{
    // UXML template for list entries
    VisualTreeAsset InvItemEntryTemplate;
    VisualTreeAsset StatItemEntryTemplate;

    // UI element references
    private ListView InventoryList;
    private ListView StatList;
    
    // List of items from inventory
    private List<Item> _items;
    private List<(string, float)> _stats;
    
    public void InitializeInventoryList(VisualElement root, VisualTreeAsset invListEntryTemplate, VisualTreeAsset statListEntryTemplate, JacksonCharacterMovement player)
    {
        _items = player.GetInventory();
        _stats = player.GetInventoryStats();
        Debug.Log("Running");

        // Store a reference to the template for the list entries
        InvItemEntryTemplate = invListEntryTemplate;
        StatItemEntryTemplate = statListEntryTemplate;
        Debug.Log((InvItemEntryTemplate != null) + ", " + (StatItemEntryTemplate != null));

        // Store a reference to the item list element
        InventoryList = root.Q<ListView>("Inventory-List");
        StatList = root.Q<ListView>("Stat-List");

        FillInventoryList();
        FillStatList();

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
            var newListEntry = InvItemEntryTemplate.Instantiate();

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
    
    void FillStatList()
    {
        // Set up a make item function for a list entry
        StatList.makeItem = () =>
        {
            // Instantiate the UXML template for the entry
            var newListEntry = StatItemEntryTemplate.Instantiate();

            // Instantiate a controller for the data
            var newListEntryLogic = new StatListEntryController();

            // Assign the controller script to the visual element
            newListEntry.userData = newListEntryLogic;

            // Initialize the controller script
            newListEntryLogic.SetVisualElement(newListEntry);

            // Return the root of the instantiated visual tree
            return newListEntry;
        };

        // Set up bind function for a specific list entry
        StatList.bindItem = (item, index) =>
        {
            // Debug.Log("Item name in list controller: " + _items[index].name);
            (item.userData as StatListEntryController).SetStatData(_stats[index]);
        };

        // Set a fixed item height
        // StatList.fixedItemHeight = 100;

        // Set the actual item's source list/array
        StatList.itemsSource = _stats;
    }
}