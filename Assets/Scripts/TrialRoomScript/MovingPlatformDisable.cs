using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformDisable : SpecialDisable
{
    // Start is called before the first frame update
    float timer = 10;
    Transform plat;
    Transform children;
    Transform player;

    public override void disable(){
        transform.parent = null;
        plat = transform.Find("Platform");
        children = transform.Find("Children");
        plat.GetComponent<MovingPlatform>().enabled = false;
        
        //plat.GetComponent<BoxCollider>().enabled = false;
        //Destroy(plat.GetComponent<BoxCollider>());
        foreach(Transform ch in children){
            if(ch.tag == "Player"){
                player = ch;
            }
        }
        if(player != null){
            //nullParent(player);
            for(int i = 1000; i >= 0; i--){
                player.parent = null;
                DontDestroyOnLoad(player.gameObject);
            }
            //Debug.Log(player);
        }

        plat.GetComponent<MeshRenderer>().enabled = false;
        Collider[] colls = plat.GetComponents<Collider>();
        foreach (Collider thing in colls)
        {
            thing.enabled = false;
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
                DontDestroyOnLoad(player.gameObject);
            }
            if(i == 1)
            {
                if(t && t.parent){
                    t.parent = null;
                    DontDestroyOnLoad(player.gameObject);
                }
                //gameObject.SetActive(false);
                Destroy(gameObject);
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
