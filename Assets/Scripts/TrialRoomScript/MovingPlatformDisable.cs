using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformDisable : SpecialDisable
{
    // Start is called before the first frame update
    float timer = 10;

    public override void disable(){
        Transform plat = transform.GetChild(0);
        plat.GetComponent<MovingPlatform>().enabled = false;
        plat.GetComponent<BoxCollider>().enabled = false;
        //Destroy(plat.GetComponent<BoxCollider>());
        Transform player = null;
        foreach(Transform ch in plat){
            if(ch.tag == "Player"){
                player = ch;
            }
        }
        if(player != null){
            //nullParent(player);
            player.parent = null;
            //Debug.Log(player);
        }

        StartCoroutine(TurnOff(player));
        //Invoke("disablePlatform", 0f);
    }

    IEnumerator TurnOff(Transform t)
    {
        for (int i = 1000; i >= 0; i--)
        {
            if(t && t.parent){
                t.parent = null;
            }
            if(i == 1)
            {
                gameObject.SetActive(false);
            }
            yield return null;
        }
        
    }
        
    
    // async void disablePlatform(){
    //     Transform plat = transform.Find("Platform");
    //     await disablePlatformComponent(plat);

    //     Transform player = plat.Find("PlayerAndCamera");
    //     if(player){
    //         await nullParent(player);
    //         //Debug.Log("somehow parent is being overridden again after this");
    //     }
    //     gameObject.SetActive(false);
        
    // }

    private void nullParent(Transform t){
        //t.SetParent(null);
        t.parent = null;
        if(!t.parent){
            //Debug.Log("somehow parent is being overridden again after this");
        }
    }

    // async void disablePlatformComponent(Transform t){
    //     t.GetComponent<MovingPlatform>().enabled = false;
    //     t.GetComponent<BoxCollider>().enabled = false;
    // }
}
