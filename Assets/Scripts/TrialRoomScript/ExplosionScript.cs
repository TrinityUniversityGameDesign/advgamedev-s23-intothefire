using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    // Start is called before the first frame update
    private SphereCollider trigger;
    public float timeUntilExplosion = 1.3f;
    public float explosionSize = 7f;
    public float damage = 25f;

    void Start()
    {
        gameObject.GetComponent<DamageScript>().SetDamage(damage);
        trigger = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerExplode(){
        Invoke("Explode", timeUntilExplosion);
    }

    private void Explode(){
        //Debug.Log("boom");
        GetComponent<MeshRenderer>().enabled = true;
        trigger.enabled = true;
        trigger.isTrigger = true;
        transform.localScale += new Vector3(explosionSize, explosionSize, explosionSize);
        
    }

    void OnTriggerEnter(Collider target){
        if(target.transform.tag == "Player"){
            Debug.Log("exploded player");
        }
    }
}
