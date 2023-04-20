using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class StatListEntryController
{
    private Label _nameLabel;

    //This function retrieves a reference to the 
    //character name label inside the UI element.
    public void SetVisualElement(VisualElement visualElement)
    {
        _nameLabel = visualElement.Q<Label>("Stat-Name");
    }

    //This function receives the character whose name this list 
    //element displays. Since the elements listed 
    //in a `ListView` are pooled and reused, it's necessary to 
    //have a `Set` function to change which character's data to display.

    public void SetStatData(ValueTuple<string, float> stat)
    {
        _nameLabel.text = stat.Item1 + ": " + stat.Item2.ToString("0");
    }
}