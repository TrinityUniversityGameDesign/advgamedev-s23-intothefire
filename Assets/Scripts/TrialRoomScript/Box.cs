using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : ScavengeEntity
{
    //public TrialRoomScript hostRoom;
    //public bool hasTreasure;

    [SerializeField]
    private GameObject treasurePrefab;

    [SerializeField]
    private GameObject bombPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    void OnCollisionEnter(Collision target){
        if(target.transform.tag == "Player"){
            Break();
        }
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Damage")
        {
            Break();
        }
    }

    private void Break(){
        Vector3 spawnLocation = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        if(hasTreasure){
            //Debug.Log("has treasure");
            GameObject treasure = Instantiate(treasurePrefab, spawnLocation, new Quaternion(0,0,0,0));
            treasure.GetComponent<TreasureScript>().hostRoom = hostRoom;
        }
        else{
            if(Random.value > 0.8f){
                Instantiate(bombPrefab, spawnLocation, new Quaternion(0,0,0,0));
            }
        }
        Destroy(gameObject);
    }
}
