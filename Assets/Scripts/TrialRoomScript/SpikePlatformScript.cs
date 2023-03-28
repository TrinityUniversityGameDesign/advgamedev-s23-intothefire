using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikePlatformScript : MonoBehaviour
{
    private Collider trigger;
    private MeshRenderer mesh;
    private Material[] mats;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<DamageScript>().SetDamage(100f);
        trigger = GetComponent<Collider>();
        trigger.isTrigger = false;
        mesh = GetComponent<MeshRenderer>();
        mats = mesh.materials;
        Invoke("Activate", 2f);
    }

    private void Activate(){
        trigger.isTrigger = true;
        mesh.material = mats[1];
        gameObject.tag = "Damage";
        Invoke("Deactivate", 2f);
    }

    private void Deactivate(){
        trigger.isTrigger = false;
        mesh.material = mats[0];
        gameObject.tag = "Untagged";
        Invoke("Activate", 2f);
    }

    void OnTriggerEnter(Collider target){
        if(target.transform.tag == "Player"){
            Debug.Log("spiked");
        }
    }

    
}
