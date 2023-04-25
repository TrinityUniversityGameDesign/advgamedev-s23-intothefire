using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryContainer : MonoBehaviour
{
    /// <summary>
    /// Toggle visibility of inventory window
    /// </summary>
    public void ToggleInventory()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
