using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengeEnemy : ScavengeEntity
{
    //public TrialRoomScript hostRoom;
    //public bool hasTreasure;

    [SerializeField]
    private GameObject treasurePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision target){
        if(target.transform.tag == "Player"){
            Dead();
        }
    }

    private void Dead(){
        if(hasTreasure){
            Vector3 spawnLocation = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            GameObject treasure = Instantiate(treasurePrefab, spawnLocation, new Quaternion(0,0,0,0));
            treasure.GetComponent<TreasureScript>().hostRoom = hostRoom;
        }
        Destroy(gameObject);
    }
}
