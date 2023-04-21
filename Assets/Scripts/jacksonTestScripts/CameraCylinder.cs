using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCylinder : MonoBehaviour
{
    // Start is called before the first frame update
    OnCameraCameraController cam;
    GameObject noGo;
    Quaternion rot;
    private void Start()
    {
        cam = transform.parent.gameObject.GetComponent<OnCameraCameraController>();
        noGo = cam.GetTarget();
        rot = Quaternion.identity;   
    }
    private void FixedUpdate()
    {
        transform.rotation = rot;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("cylinder exit");
        if(other.gameObject.tag == "Player" && noGo != other.gameObject)
        {

            cam.RemoveTarget(other.gameObject);
        }
    }

}
