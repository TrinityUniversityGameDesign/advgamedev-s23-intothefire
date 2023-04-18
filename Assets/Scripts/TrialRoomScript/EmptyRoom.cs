using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyRoom : MonoBehaviour
{
    public GameObject roomPrefab;
    private GameObject room;
    
    public enum EmptyRoomState
    { empty, spawned, complete }

    private EmptyRoomState state;
    // Start is called before the first frame update
    void Start()
    {
        state = EmptyRoomState.empty;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void depsawnRoom(){
        Destroy(room, 1f);
        state = EmptyRoomState.complete;
    }

    void OnTriggerEnter(Collider target){
        if(target.transform.tag == "Player" && state == EmptyRoomState.empty){
            if(roomPrefab){
                room = Instantiate(roomPrefab, transform.position, transform.rotation);
                room.GetComponent<TrialRoomScript>().hostEmpty = this;
                state = EmptyRoomState.spawned;
            }
            
        }
    }


}
