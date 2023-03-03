using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }




  public void GoToArena()
	{
    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    foreach(GameObject player in players)
		{
      player.transform.position = Vector3.zero;
		}
    SceneManager.LoadScene("Arena");
	}


}
