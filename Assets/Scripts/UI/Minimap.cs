using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    /// <summary>
    /// Toggle visibility of minimap. Generally call this through the HUD Controller
    /// </summary>
    public void ToggleMinimap()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
