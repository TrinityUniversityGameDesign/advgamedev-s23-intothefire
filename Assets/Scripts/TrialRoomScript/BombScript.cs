using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject explosion;
    public float explodeTime = 2f;

    void Start()
    {
        //gameObject.GetComponent<DamageScript>().SetDamage(5f);
        explosion = GameObject.Find("Explosion");
        ExplosionScript explosionScript = explosion.GetComponent<ExplosionScript>();
        explosionScript.TriggerExplode();
        Destroy(gameObject, explosionScript.timeUntilExplosion + 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
