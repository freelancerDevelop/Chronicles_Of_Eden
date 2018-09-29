﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treadmill : MonoBehaviour {

    private Dictionary<Transform, Vector3> positionsOfUsers = new Dictionary<Transform, Vector3>();

    private Vector3 directionFacing;
    private bool powered = false; //Whether or not the treadmill is being powered.

    [SerializeField] ToggleBehaviour action;
    [SerializeField] float resistance;

    private void Start()
    {
        //Find the direction in which the treadmill is facing, so we can send the user(s) in the opposite direction.
        directionFacing = (transform.rotation * Vector3.forward).normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!positionsOfUsers.ContainsKey(other.transform))
            positionsOfUsers[other.transform] = other.transform.position;
    }

    private void OnTriggerExit(Collider other)
    {
        positionsOfUsers.Remove(other.transform);
    }

    private void Update()
    {

        //Check each user and see if they have moved, powering the treadmill and moving them.
        bool movingUser = false;
        Dictionary<Transform, Vector3> newPositions = new Dictionary<Transform, Vector3>();
        foreach(Transform t in positionsOfUsers.Keys)
        {

            Vector3 positionChange = positionsOfUsers[t] - t.position;
            if (!positionChange.Equals(Vector3.zero))
            {
                movingUser = true;

                float directionAdjustMultiplier = Vector3.Angle(directionFacing, positionChange.normalized) > 90 ? 1 : -1;
                t.position = t.position - directionFacing * resistance * directionAdjustMultiplier;

            }     

            
            newPositions[t] = t.position;

        }

        positionsOfUsers = newPositions;
        SetPowered(movingUser);

    }

    private void SetPowered(bool p)
    {
        Debug.Log("p: " + p + " - " + "powered: " + powered);

        if (powered == !p) action.Toggle();
        powered = p;
    }

}
