using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To create subclasses of items (healing, shock, etc), maybe make subclasses
[CreateAssetMenu(menuName ="Items/Item")]
public class ItemData : ScriptableObject
{
    // Possible way to implement item effects
    public enum ItemEffects
    {
        Health, AttackSpeed, MoveSpeed, AttackDamage
    }
    
    // Description field for item description
    [Header("Item Details")]
    [TextArea(3,5)]
    public string description;
    
    [Header("Item Model")]
    //Icon to be displayed in UI
    public Sprite icon;

    //GameObject to be shown in the scene
    public GameObject gameModel;
}

// ScriptableObjects used: https://blog.terresquall.com/2021/09/creating-farming-rpg-in-unity-part-4/
// Possible UI Manager: https://gamedev-resources.com/create-an-item-management-editor-tool-with-ui-toolkit/
// Possible Inventory UI: https://gamedev-resources.com/create-an-in-game-inventory-ui-with-ui-toolkit/