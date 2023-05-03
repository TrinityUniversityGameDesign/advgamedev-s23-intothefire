using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ShowdownVictorScript : MonoBehaviour
{
  List<GameObject> survivors = new List<GameObject>();
  public List<GameObject> playersInOrderOfLoss = new List<GameObject>();
  public bool showdownOn = true;
  public UnityEvent ShowdownWinnerDecided = new UnityEvent();



  private void Start()
  {
    survivors = GameManager.Instance.players;
    Debug.Log(survivors);
    //ShowdownWinnerDecided.AddListener(LoadVictoryScene);
    DontDestroyOnLoad(gameObject);
  }

  public GameObject getVictor()
  {
    if (survivors.Count == 1)
    {
      return survivors[0];
    }
    else
    {
      Debug.Log("ERROR: ShowdownVictorScript.getVictor() was used in an incorrect location. It should only be called once the showdown has ended - ie the EndScreen.");
      return null;
    }
  }

  private void Update()
  {
    if (survivors.Count == 1)
    {
      if (showdownOn)
      {
        playersInOrderOfLoss.Add(survivors[0]);
        GameManager.Instance.ExternalShowdownEnd.Invoke();
        showdownOn = false;
        LoadVictoryScene();
      }
    }

		for(int i = 0; i < survivors.Count; i++)
		{
			if(survivors[i].transform.position.y == 350f)
			{
				survivors.Remove(survivors[i]);
			}
		}
	}
  public void LoadVictoryScene()
  {
    SceneManager.LoadScene("Victory", LoadSceneMode.Single);
  }
}
