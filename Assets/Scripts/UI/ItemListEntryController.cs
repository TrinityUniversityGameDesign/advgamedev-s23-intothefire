using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemListEntryController
{
    private VisualElement _itemIcon;
    private Label _nameLabel;
    private Label _descLabel;

    //This function retrieves a reference to the 
    //character name label inside the UI element.
    public void SetVisualElement(VisualElement visualElement)
    {
        _itemIcon = visualElement.Q<VisualElement>("Item-Icon");
        _nameLabel = visualElement.Q<Label>("Item-Name");
        _descLabel = visualElement.Q<Label>("Item-Description");
    }

    //This function receives the character whose name this list 
    //element displays. Since the elements listed 
    //in a `ListView` are pooled and reused, it's necessary to 
    //have a `Set` function to change which character's data to display.

    public void SetItemData(Item item)
    {
        // if (item.icon != null && _itemIcon != null)
        _itemIcon.style.backgroundImage = new StyleBackground(item.icon);
        // if (item.name != null && _nameLabel != null)
        _nameLabel.text = item.name;
        // if (item.description != null && _descLabel != null)
        _descLabel.text = item.description;
    }
}