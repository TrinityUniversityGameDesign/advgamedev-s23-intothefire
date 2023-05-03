using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShowdownVictorScript : MonoBehaviour
{
	List<GameObject> survivors = new List<GameObject>();
	List<GameObject> playersInOrderOfLoss = new List<GameObject>();


	public UnityEvent ShowdownWinnerDecided = new UnityEvent();


	private void Start()
	{
		survivors = GameManager.Instance.players;
		Debug.Log(survivors);
	}

	public GameObject getVictor()
	{
		if (survivors.Count == 1)
		{
			Debug.Log("The victor is: " + survivors[0]);
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
		if(survivors.Count == 1)
		{
			playersInOrderOfLoss.Add(survivors[0]);
			GameManager.Instance.ExternalShowdownEnd.Invoke();
		}

		for(int i = 0; i < survivors.Count; i++)
		{
			if(survivors[i].transform.position.y == 350f)
			{
				playersInOrderOfLoss.Add(survivors[i]);
				survivors.Remove(survivors[i]);
			}
		}
	}
}
