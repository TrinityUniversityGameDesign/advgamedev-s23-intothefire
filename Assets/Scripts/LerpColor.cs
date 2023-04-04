using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))] 
public class LerpColor : MonoBehaviour
{
    public Color color1;
    public Color color2;
    
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    /// <summary>
    /// Updates color of image based on slider value?
    /// </summary>
    /// <param name="value">Should be a dynamic float pulled from slider...</param>
    public void OnSliderValueChange(float value)
    {
        // Color lerpColor = Color.Lerp(color1, color2, value);
        // _image.color = lerpColor;
    }
}
