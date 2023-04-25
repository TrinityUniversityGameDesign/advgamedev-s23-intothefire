using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpleefPlatform : MonoBehaviour
{

	bool touched = false;
	Vector3 destination;
	private Renderer thisRenderer;

	private void Start()
	{
		thisRenderer = gameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.tag == "Player")
		{
			touched= true;
		}
	}

	float duration = 1.5f;
	private float time = 0;

	void Update()
	{
		if(touched) ColorChanger();
	}


	void ColorChanger()
	{
		if (touched)
		{
			thisRenderer.material.color = Color.Lerp(Color.green, Color.red, time);

			if (time < duration)
			{
				time += Time.deltaTime / duration;
			}
			else Destroy(gameObject);
		}
	}


}
