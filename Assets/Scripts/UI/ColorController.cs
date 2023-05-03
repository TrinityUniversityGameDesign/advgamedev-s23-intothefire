using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorController : MonoBehaviour
{
    public bool darker = false;
    public void SetImageColor(Color color)
    {
        try
        {
            var image = GetComponent<Image>();
            if (darker)
            {
                const float amt = 0.4f;
                image.color = new Color(color.r - amt, color.g - amt, color.b - amt, 0.5f);
            }
            else image.color = color;
        }
        catch (Exception e)
        {
            Console.WriteLine(e + ": There is not an image component to set");
            throw;
        }
    }
}
