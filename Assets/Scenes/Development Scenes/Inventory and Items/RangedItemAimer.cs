﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A component to attach to the player that allows them to aim a ranged item when they use it.
/// </summary>
public class RangedItemAimer : MonoBehaviour {

    private AimableItem itemBeingAimed;
    private Transform origin; //From where the item is being aimed.

    [SerializeField] MouseRotatedFocusingCamera cam;

    private void Start()
    {
        origin = GetComponent<Transform>();
        if (origin == null)
            throw new System.Exception(gameObject.name + " should have a transform attatched to it to use a RangedItemAimer.");
    }

    void Update () {

        if (itemBeingAimed == null) return; //If there is no item being aimed, don't do anything.

        //Try to find a point on the map the player is aiming at, and do nothing if it does not exist.
        Vector3 aimPoint = GetAimPoint();
        if (aimPoint.Equals(Vector3.positiveInfinity)) return;

        itemBeingAimed.VisualiseAim(origin.position, aimPoint);

        //When the user releases the mouse button again, fire the item and unlock the camera.
        if (Input.GetMouseButtonUp(0)) { 
            itemBeingAimed.Fire(origin.position, aimPoint);
            cam.lockRotation = false;
        }

	}

    /// <summary>
    /// Initilaise the aiming of the given item.
    /// </summary>
    /// <param name="itemBeingAimed"></param>
    public void AimItem(AimableItem itemBeingAimed)
    {
        this.itemBeingAimed = itemBeingAimed;
        cam.lockRotation = true;
    }

    /// <summary>
    /// Gets the point on the map that the user is aiming at with the mouse.
    /// </summary>
    /// <returns>The point on the map the mouse is over, or infinity if it is over nothing.</returns>
    private Vector3 GetAimPoint()
    {

        //Generate a ray from the camera to the mouse point.
        Ray cameraToMouseRay = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        //Cast the ray and determine the point on the map that it hits, if it hits anything.
        RaycastHit hit;
        if(!Physics.Raycast(cameraToMouseRay, out hit)) return Vector3.positiveInfinity;
        return hit.point;

    }

}
