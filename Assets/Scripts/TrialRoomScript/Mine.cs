using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public enum MineState
    {quiet, triggered}

    [SerializeField]
    private GameObject explosion;
    private ExplosionScript explosionScript;

    public MineState state;
    // Start is called before the first frame update
    void Start()
    {
        state = MineState.quiet;
        explosion = transform.Find("Explosion").gameObject;
        explosionScript = explosion.GetComponent<ExplosionScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider target){
        if(target.transform.tag == "Player" && state == MineState.quiet){
            state = MineState.triggered;
            explosionScript.TriggerExplode();
            Destroy(explosion, explosionScript.timeUntilExplosion + 0.5f);
            Destroy(gameObject, explosionScript.timeUntilExplosion + 0.6f);
        }
    }
}
