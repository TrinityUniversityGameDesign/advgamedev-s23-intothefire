using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniCam : MonoBehaviour
{
    [SerializeField] private float height = 150f;
    public void UpdatePosition(Transform player)
    {
        var playerPosition = player.transform.position;
        transform.position = new Vector3(playerPosition.x, height, playerPosition.z);
        transform.eulerAngles = new Vector3(90f, 0f, 0f) ;
    }

    public void SetIndexTexture(int index)
    {
        GetComponent<Camera>().targetTexture = Resources.Load<RenderTexture>($"Textures/Minimap{index}");
    }
}
