using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerExplode(){
        Invoke("Explode", 1.3f);
    }

    private void Explode(){
        Debug.Log("boom");
        transform.localScale += new Vector3(7,7,7);
    }

    void OnTriggerEnter(Collider target){
        if(target.transform.tag == "Player"){
            Debug.Log("exploded player");
        }
    }
}
