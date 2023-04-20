using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuickView : MonoBehaviour
{
    [SerializeField] private float itemSize = 60f;
    public void InitializeItems(List<Item> items, GameObject itemPrefab)
    {
        for (int i = 0; i < items.Count; i++)
        {
            GameObject newObject = Instantiate(itemPrefab, transform);
            newObject.gameObject.GetComponentInChildren<ImageController>().SetImageToSprite(items[i].icon);
        }
    }
}
