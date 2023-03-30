using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryListController
{
    // UXML template for list entries
    VisualTreeAsset ItemEntryTemplate;

    // UI element references
    ListView InventoryList;
    Label ItemNameLabel;
    Label ItemDescLabel;
    VisualElement ItemIcon;

    public void InitializeInventoryList(VisualElement root, VisualTreeAsset itemEntryTemplate)
    {
        EnumerateAllItems();

        // Store a reference to the template for the list entries
        ItemEntryTemplate = itemEntryTemplate;

        // Store a reference to the item list element
        InventoryList = root.Q<ListView>("Inventory-List");

        // Store references to the selected item info elements
        ItemNameLabel = root.Q<Label>("Item-Name");
        ItemDescLabel = root.Q<Label>("Item-Description");
        ItemIcon = root.Q<VisualElement>("Item-Icon");

        FillInventoryList();

        // Register to get a callback when an item is selected
        InventoryList.onSelectionChange += OnCharacterSelected;
    }

    List<CharacterData> AllCharacters;

    void EnumerateAllStats()
    {
        AllCharacters = new List<Stat>();
        AllCharacters.AddRange(Resources.LoadAll<Stat>("Characters"));
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
            var newListEntryLogic = new InventoryListEntryController();

            // Assign the controller script to the visual element
            newListEntry.userData = newListEntryLogic;

            // Initialize the controller script
            newListEntryLogic.SetVisualElement(newListEntry);

            // Return the root of the instantiated visual tree
            return newListEntry;
        };

        // Set up bind function for a specific list entry
        CharacterList.bindItem = (item, index) =>
        {
            (item.userData as CharacterListEntryController).SetCharacterData(AllCharacters[index]);
        };

        // Set a fixed item height
        CharacterList.fixedItemHeight = 45;

        // Set the actual item's source list/array
        CharacterList.itemsSource = AllCharacters;
    }

    void OnCharacterSelected(IEnumerable<object> selectedItems)
    {
        // Get the currently selected item directly from the ListView
        var selectedCharacter = CharacterList.selectedItem as CharacterData;

        // Handle none-selection (Escape to deselect everything)
        if (selectedCharacter == null)
        {
            // Clear
            CharClassLabel.text = "";
            CharNameLabel.text = "";
            CharPortrait.style.backgroundImage = null;

            return;
        }

        // Fill in character details
        CharClassLabel.text = selectedCharacter.Class.ToString();
        CharNameLabel.text = selectedCharacter.CharacterName;
        CharPortrait.style.backgroundImage = new StyleBackground(selectedCharacter.PortraitImage);
    }
}