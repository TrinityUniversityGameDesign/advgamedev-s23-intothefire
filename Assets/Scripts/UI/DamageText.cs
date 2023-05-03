using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public void Disable()
    {
        gameObject.transform.parent = GameManager.Instance.gameObject.transform;
        gameObject.SetActive(false);
    }
}
