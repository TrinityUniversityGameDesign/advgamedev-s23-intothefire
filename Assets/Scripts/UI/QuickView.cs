using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuickView : MonoBehaviour
{
    [SerializeField] private float itemSize = 60f;
    
    /// <summary>
    /// Initialize items in the inventory quickview
    /// </summary>
    /// <param name="items">list of items from player</param>
    /// <param name="itemPrefab">prefab to use for items; set in the HUD object</param>
    public void InitializeItems(List<Item> items, GameObject itemPrefab)
    {
        for (int i = 0; i < items.Count; i++)
        {
            GameObject newObject = Instantiate(itemPrefab, transform);
            newObject.gameObject.GetComponentInChildren<ImageController>().SetImageToSprite(items[i].icon);
        }
    }
    public void InitializeItems(List<ItemData> items, GameObject itemPrefab)
    {
        for (int i = 0; i < items.Count; i++)
        {
            GameObject newObject = Instantiate(itemPrefab, transform);
            newObject.gameObject.GetComponentInChildren<ImageController>().SetImageToSprite(items[i].icon);
        }
    }
}
