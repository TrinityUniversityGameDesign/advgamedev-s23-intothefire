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
        explosion = GameObject.Find("Explosion");
        ExplosionScript explosionScript = explosion.GetComponent<ExplosionScript>();
        explosionScript.TriggerExplode();
        Destroy(gameObject, explodeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
