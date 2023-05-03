using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCamera : MonoBehaviour
{
    private Camera _camera;
    // Start is called before the first frame update
    void Awake()
    {
        _camera = transform.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateRenderTexture(int position)
    {
        var texture = Resources.Load<RenderTexture>("Textures/Player" + position);
        Debug.Log(_camera);
        _camera.targetTexture = texture;
    }
}
