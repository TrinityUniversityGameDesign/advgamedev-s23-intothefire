using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifiedTeleporter : MonoBehaviour
{
    public List<ModifiedTeleporter> portals;

    private bool playerIsOverlapping = false;

    // Start is called before the first frame update
    void Start()
    {
        // Add this teleporter to the portals list of other teleporters
        foreach (ModifiedTeleporter portal in portals)
        {
            if (!portal.portals.Contains(this))
            {
                portal.portals.Add(this);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsOverlapping)
        {
            // Iterate through the portals list and check if the player is overlapping with any of them
            foreach (ModifiedTeleporter portal in portals)
            {
                if (portal.playerIsOverlapping)
                {
                    continue;
                }

                // Teleport the player to a random portal
                if (portal.CompareTag("Teleporter"))
                {
                    ModifiedTeleporter randomPortal = portals[Random.Range(0, portals.Count)];
                    Vector3 positionOffset = randomPortal.transform.position - transform.position;
                    GameObject.FindGameObjectWithTag("Player").transform.position = randomPortal.transform.position + positionOffset;

                    float angle = Quaternion.Angle(randomPortal.transform.rotation, transform.rotation);
                    Quaternion rotationDiff = Quaternion.AngleAxis(angle, Vector3.up);
                    GameObject.FindGameObjectWithTag("Player").transform.rotation = rotationDiff * GameObject.FindGameObjectWithTag("Player").transform.rotation;

                    playerIsOverlapping = false;
                    randomPortal.playerIsOverlapping = true;
                    Debug.Log("Transport!");

                    break;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsOverlapping = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsOverlapping = false;
        }
    }
}
