using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowdownTransition : MonoBehaviour
{

  public List<GameObject> players = new List<GameObject>();
  public Camera[] cameras = new Camera[4];

  void Start()
  {
    GameManager.Instance.ShowdownBegin.AddListener(TransitionToShowdown);
  }

  private void TransitionToShowdown()
	{
    
    //get players    
    players = GameManager.Instance.players;
    foreach(GameObject player in players)
		{
      DontDestroyOnLoad(player);
		}


    //get player cameras (FindObjectsOfType is gross but it works)
    cameras = FindObjectsOfType<Camera>();
    foreach (Camera camera in cameras)
    {
      DontDestroyOnLoad(camera);
    }

    //get the GameManager
    DontDestroyOnLoad(GameManager.Instance.gameObject);


    //THIS is where you would place any code for a fancy transition between the labyrinth and the final showdown arena



    SceneManager.LoadScene(ChooseArena());

  }

  //For choosing between different arenas (if there are multiple)
  private string ChooseArena()
	{
    return "ShowdownArena1";
	}






}
