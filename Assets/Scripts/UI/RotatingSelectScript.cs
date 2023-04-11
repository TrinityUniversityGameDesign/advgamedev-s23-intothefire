using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RotatingSelectScript : MonoBehaviour
{

    public int playerIndex = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void PlayerMeshRotator()
    {
        GameManager.Instance.ChangePlayerMesh(playerIndex);
    }

    public void PlayerWeaponRotator()
    {
        
    }
}
