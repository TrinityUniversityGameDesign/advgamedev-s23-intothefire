using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuickView : MonoBehaviour
{
    private GameObject _itemPrefab;
    private bool _initialized = false;
    
    /// <summary>
    /// Initialize items in the inventory quickview
    /// </summary>
    /// <param name="items">list of items from player</param>
    /// <param name="itemPrefab">prefab to use for items; set in the HUD object</param>
    public void Initialize(List<Item> items, GameObject itemPrefab)
    {
        _itemPrefab = itemPrefab;
        _initialized = true;
        foreach (var t in items)
        {
            Add(t);
        }
    }

    public void AddItem(Item item)
    {
        if (!_initialized) Debug.LogError("Cannot add item to uninitialized inventory");
        else Add(item);
    }

    private void Add(Item item)
    {
        GameObject newObject = Instantiate(_itemPrefab, transform);
        newObject.gameObject.GetComponentInChildren<ImageController>().SetImageToSprite(item.icon);
    }
}
