using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /// <summary>
    /// Initialize items in the inventory window
    /// </summary>
    /// <param name="items">list of items from player</param>
    /// <param name="itemPrefab">prefab to use for item rows</param>
    public void InitializeItems(List<Item> items, GameObject itemPrefab)
    {
        for (int i = 0; i < items.Count; i++)
        {
            GameObject newObject = Instantiate(itemPrefab, transform);
            Debug.Log("Made object from prefab: " + newObject.name);
            var imageComponent = newObject.gameObject.GetComponentInChildren<ImageController>();
            Debug.Log("Got the image component: " + (imageComponent != null));
            imageComponent.SetImageToSprite(items[i].icon);
            newObject.transform.Find("name").GetComponent<TMP_Text>().text = items[i].name;
            newObject.transform.Find("desc").GetComponent<TMP_Text>().text = items[i].description;
        }
    }
    
    public void InitializeItems(List<ItemData> items, GameObject itemPrefab)
    {
        for (int i = 0; i < items.Count; i++)
        {
            GameObject newObject = Instantiate(itemPrefab, transform);
            newObject.gameObject.GetComponentInChildren<ImageController>().SetImageToSprite(items[i].icon);
            newObject.transform.Find("name").GetComponent<TMP_Text>().text = items[i].name;
            newObject.transform.Find("desc").GetComponent<TMP_Text>().text = items[i].description;
        }
    }
}
