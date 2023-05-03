using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterBossScript : MonoBehaviour
{

  List<float> damageList;
  
  // Start is called before the first frame update
    void Start()
    {
      damageList = transform.GetComponent<MiniBossAi>().damageTracker;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
