using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowdownTransition : MonoBehaviour
{
  void Start()
  {
    GameManager.Instance.ShowdownBegin.AddListener(TransitionToShowdown);
  }

  private void TransitionToShowdown()
	{
    //THIS is where you would place any code for a fancy transition between the labyrinth and the final showdown arena
    SceneManager.LoadScene(ChooseArena());

  }

  //For choosing between different arenas (if there are multiple)
  private string ChooseArena()
	{
    return "ShowdownArenaReal";
	}






}
