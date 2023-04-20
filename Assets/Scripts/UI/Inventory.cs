using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public void InitializeItems(List<Item> items, GameObject itemPrefab)
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
