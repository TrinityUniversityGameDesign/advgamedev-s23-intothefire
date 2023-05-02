using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCone : MonoBehaviour
{
    // Start is called before the first frame update
    OnCameraCameraController cam;
    GameObject noGo;
    private void Start()
    {
        cam = transform.parent.gameObject.GetComponent<OnCameraCameraController>();
        noGo = cam.GetTarget();
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("cone added");
        if((other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy") && noGo != other.gameObject && other.gameObject != null)
        {
            cam.AddTarget(other.gameObject);
        }
    }

}
