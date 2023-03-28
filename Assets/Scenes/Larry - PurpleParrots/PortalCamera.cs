using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    public Transform playerCamera;
	public Transform portal;
	public Transform otherPortal;
	
	// Update is called once per frame
	void Update () {
        // playerOffsetFromPortal is the vector of the distance bewteen the player and a portal away from the one close to the 
        // Player.
		Vector3 playerOffsetFromPortal = playerCamera.position - otherPortal.position;

        // Move the camera according to the portal nearest to the Player to the offset. This makes the camera of the particualar
        // portal move along the player's movements.
		transform.position = portal.position + playerOffsetFromPortal;

        // This float helps with caclulating area of the angles of each portal;
		float angularDifferenceBetweenPortalRotations = Quaternion.Angle(portal.rotation, otherPortal.rotation);

        // The takes the quaterion that creates a rotation which rotates angle degrees around axis.
		Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);

        // This gives the camera the angles from the difference as is faces forward to the otherPortal.
		Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;

        // This handles the rotation of the newCameraDirection towards the portal
		transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
	}

}
