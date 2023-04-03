using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikePlatformScript : MonoBehaviour
{
    private Collider trigger;
    private MeshRenderer mesh;
    private Material[] mats;

    public float timeUntilFirstActive = 2f;
    public float timeUntilActive = 2f;
    public float timeUntilDeactive = 2f;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<DamageScript>().SetDamage(10f);
        trigger = GetComponent<Collider>();
        trigger.isTrigger = false;
        mesh = GetComponent<MeshRenderer>();
        mats = mesh.materials;
        Invoke("Activate", timeUntilFirstActive);
    }

    private void Activate(){
        trigger.isTrigger = true;
        mesh.material = mats[1];
        gameObject.tag = "Damage";
        Invoke("Deactivate", timeUntilDeactive);
    }

    private void Deactivate(){
        trigger.isTrigger = false;
        mesh.material = mats[0];
        gameObject.tag = "Untagged";
        Invoke("Activate", timeUntilActive);
    }

    void OnTriggerEnter(Collider target){
        if(target.transform.tag == "Player"){
            Debug.Log("spiked");
        }
    }

    
}
