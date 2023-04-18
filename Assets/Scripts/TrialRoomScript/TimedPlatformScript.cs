using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedPlatformScript : MonoBehaviour
{
    public enum PlatformState
    {visible, disappearing}

    [SerializeField]
    private PlatformState state;

    public float timeUntilDisappear = 2f;
    public float timeUntilReappear = 2f;
    // Start is called before the first frame update
    void Start()
    {
        state = PlatformState.visible;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision target){
        if(target.transform.tag == "Player" && state == PlatformState.visible){
            state = PlatformState.disappearing;
            Invoke("disablePlatform", timeUntilDisappear);
        }
    }

    private void disablePlatform(){
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        Invoke("enablePlatform", timeUntilReappear);
    }

    private void enablePlatform(){
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        state = PlatformState.visible;
    }
}
