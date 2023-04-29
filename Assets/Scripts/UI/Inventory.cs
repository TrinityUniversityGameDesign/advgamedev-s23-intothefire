using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private bool quickView = false;
    private bool _initialized = false;
    private GameObject _rowPrefab;
    
    /// <summary>
    /// Initialize items in the inventory window
    /// </summary>
    /// <param name="items">list of items from player</param>
    /// <param name="rowPrefab">prefab to use for item rows</param>
    /// /// <param name="iconPrefab">prefab to use for item icons</param>
    public void Initialize(List<Item> items, GameObject rowPrefab)
    {
        _rowPrefab = rowPrefab;
        _initialized = true;
        LoadItems(items);
    }
    
    private void LoadItems(List<Item> items)
    {
        foreach (var t in items)
        {
            Add(t);
        }
    }

    public void AddItem(Item item)
    {
        if (!_initialized) Debug.LogError("You must initialize inventory before adding to it");
        else Add(item);
    }

    private void Add(Item t)
    {
        GameObject newObject = Instantiate(_rowPrefab, transform);
        var imageComponent = newObject.gameObject.GetComponentInChildren<ImageController>();
        imageComponent.SetImageToSprite(t.icon);
        newObject.transform.Find("Container").transform.Find("name").GetComponent<TMP_Text>().text = t.name;
        newObject.transform.Find("Container").transform.Find("desc").GetComponent<TMP_Text>().text = t.description;
    }
}
