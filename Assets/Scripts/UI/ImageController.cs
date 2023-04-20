using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageController : MonoBehaviour
{
    public void SetRawImageToSprite(Sprite image)
    {
        try
        {
            var rawImage = GetComponent<RawImage>();
            rawImage.texture = image.texture;
        }
        catch (Exception e)
        {
            Console.WriteLine(e + ": There is not a raw image component to set");
            throw;
        }
    }

    public void SetImageToSprite(Sprite newImage)
    {
        try
        {
            var image = GetComponent<Image>();
            image.sprite = newImage;
        }
        catch (Exception e)
        {
            Console.WriteLine(e + ": There is not an image component to set");
            throw;
        }
    }
}
