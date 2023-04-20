using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleController : MonoBehaviour
{
    [SerializeField]
    GameObject teleportSystem;

    public void PlayTeleport()
    {
        teleportSystem.transform.GetComponentInChildren<ParticleSystem>().Play();
    }
}
