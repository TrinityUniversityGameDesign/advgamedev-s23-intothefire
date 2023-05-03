using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterBossScript : MonoBehaviour
{

    List<float> damageList;

    float damageSum = 0;
    public float bossHealth = 200;
    int lastPlayerToDoDamage;

    // Start is called before the first frame update
    void Start()
    {
      damageList = transform.GetComponent<MiniBossAi>().damageTracker;
    }
    
    public void UpdateDamageSum(int player)
    {
    Debug.Log("Player: " + player + " did damage.");
      lastPlayerToDoDamage = player;
      float temp = 0;
      foreach(float num in damageList)
		  {
        temp += num/100;
		  }
      damageSum = temp;
      Debug.Log(damageSum);
    }


	  private void Update()
	  {
      Debug.Log("DamageSum: " + damageSum);
		  if(damageSum >= bossHealth)
		  {
        GameManager.Instance.AwardRandomItem(lastPlayerToDoDamage);
        GameManager.Instance.AwardRandomItem(lastPlayerToDoDamage);
        GameManager.Instance.AwardRandomItem(lastPlayerToDoDamage);
        GameManager.Instance.AwardRandomItem(lastPlayerToDoDamage);
        GameManager.Instance.AwardRandomItem(lastPlayerToDoDamage);
        Destroy(gameObject);  
		  }
	  }


}
