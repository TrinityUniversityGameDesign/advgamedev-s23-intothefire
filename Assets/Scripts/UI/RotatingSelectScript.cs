using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RotatingSelectScript : MonoBehaviour
{

    int index = 0;
    int childCount;
    int player;

    public bool isCharacter;

    // Start is called before the first frame update
    void Start()
    {
        childCount = gameObject.transform.childCount;
        gameObject.transform.GetChild(0)?.gameObject.SetActive(true);
        player = GameManager.Instance.LastJoinedPlayer;
    }

    public void Rotator()
    {
        gameObject.transform.GetChild(index)?.gameObject.SetActive(false);
        if(index + 1 >= childCount)
        {
            index = 0;
        } else
        {
            index++;
        }
        gameObject.transform.GetChild(index)?.gameObject.SetActive(true);

        if(isCharacter) { GameManager.Instance.UpdatePlayerCharacter(player, index); } else { GameManager.Instance.UpdatePlayerWeapon(player, index); }
    }
}
