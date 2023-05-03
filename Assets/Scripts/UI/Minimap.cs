using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    /// <summary>
    /// Toggle visibility of minimap. Generally call this through the HUD Controller
    /// </summary>
    public void ToggleMinimap()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void SetIndexTexture(int index)
    {
        Debug.Log(index);
        GetComponentInChildren<RawImage>().texture = Resources.Load<RenderTexture>($"Textures/Minimap{index}");
    }
}
