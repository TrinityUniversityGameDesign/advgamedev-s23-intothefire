using Unity.VisualScripting;
using UnityEngine.UIElements;

public class ItemListEntryController
{
    Label NameLabel;
    Label DescriptionLabel;
    private VisualElement ItemIcon;

    //This function retrieves a reference to the 
    //character name label inside the UI element.

    public void SetVisualElement(VisualElement visualElement)
    {
        NameLabel = visualElement.Q<Label>("Item-Name");
        DescriptionLabel = visualElement.Q<Label>("Item-Description");
        ItemIcon = visualElement.Q<VisualElement>("Item-Icon");
    }

    //This function receives the character whose name this list 
    //element displays. Since the elements listed 
    //in a `ListView` are pooled and reused, it's necessary to 
    //have a `Set` function to change which character's data to display.

    public void SetItemData(Item item)
    {
        NameLabel.text = item.name;
        DescriptionLabel.text = item.desc;
        ItemIcon.style.backgroundImage = new StyleBackground(item.icon);

    }
}