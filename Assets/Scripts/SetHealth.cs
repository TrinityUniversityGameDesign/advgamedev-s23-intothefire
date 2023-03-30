using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SetHealth : MonoBehaviour
{
    public JacksonPlayerMovement player;

    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        slider.value = player.GetPlayerHealth();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Testing SetHealth");
        Debug.Log(player.GetPlayerHealth());
        slider.value = player.GetPlayerHealth();
    }
}
