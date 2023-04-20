using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifiedTeleporter : MonoBehaviour
{
    public Transform player;
    public List<ModifiedTeleporter> portals;

    private bool playerIsOverlapping = false;

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

                Vector3 portalToPlayer = player.position - portal.transform.position;
                float dotProduct = Vector3.Dot(portal.transform.up, portalToPlayer);

                // If this is true: The player has moved across the portal
                if (dotProduct < 9f)
                {
                    // Teleport him!
                    Vector3 positionOffset = portal.transform.position - transform.position;
                    player.position = transform.position + positionOffset;

                    float angle = Quaternion.Angle(portal.transform.rotation, transform.rotation);
                    Quaternion rotationDiff = Quaternion.AngleAxis(angle, Vector3.up);
                    player.rotation = rotationDiff * player.rotation;

                    playerIsOverlapping = false;
                    portal.playerIsOverlapping = true;
                    Debug.Log("Transport!");

                    break;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsOverlapping = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsOverlapping = false;
        }
    }
}
