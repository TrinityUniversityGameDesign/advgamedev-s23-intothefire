using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkScript : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject woo;
    GameObject walk;
    float timer = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if(woo != null)
        {
            //transform.position = woo.transform.position;
        }
        if (walk != null)
        {
            //transform.LookAt(walk.transform);
        }
        if(woo != null && walk != null)
        {
            float blarg = Vector3.Distance(woo.transform.position, walk.transform.position);
            blarg = blarg / 2f;
            //transform.localScale = new Vector3(blarg, blarg, blarg);
        }
        timer++;
        if(timer > 25)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetParent(GameObject g)
    {
        woo = g;
    }
    public void SetSender(GameObject s)
    {
        walk = s;
    }
}
