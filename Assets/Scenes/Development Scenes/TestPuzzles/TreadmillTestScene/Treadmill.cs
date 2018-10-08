﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))] 
public class Treadmill : ExternallyTriggerable {

    private Dictionary<Transform, Vector3> positionsOfUsers = new Dictionary<Transform, Vector3>();

    private Vector3 directionFacing;
    private bool powered = false; //Whether or not the treadmill is being powered.

    private Animator animator;

    [SerializeField] ToggleBehaviour action;
    [SerializeField] float resistance;
    [SerializeField] float directionOffset;

    private void Start()
    {
        //Find the direction in which the treadmill is facing, so we can send the user(s) in the opposite direction.
        Quaternion offsettedRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + directionOffset, transform.eulerAngles.z);
        directionFacing = (offsettedRotation * Vector3.forward).normalized;

        animator = GetComponent<Animator>();
    }

    public override void TriggerEntered(Collider other)
    {

        Debug.Log(other.gameObject.name + " entered treadmill!");

        if (!positionsOfUsers.ContainsKey(other.transform))
            positionsOfUsers[other.transform] = other.transform.position;
    }

    public override void TriggerExited(Collider other)
    {
        positionsOfUsers.Remove(other.transform);
    }

    private void Update()
    {

        Vector3 movement = Vector3.zero;

        //Check each user and see if they have moved, powering the treadmill and moving them.
        bool movingUser = false;
        Dictionary<Transform, Vector3> newPositions = new Dictionary<Transform, Vector3>();
        foreach(Transform t in positionsOfUsers.Keys)
        {

            if (t == null) return;
            Agent treadmillUser = t.GetComponent<Agent>();
            if (treadmillUser == null) continue;

            Debug.Log("Is moving: " + treadmillUser.IsMoving);

            Vector3 positionChange = positionsOfUsers[t] - t.position;
            if (treadmillUser.IsMoving)
            {
                movingUser = true;

                float directionAdjustMultiplier = Vector3.Angle(directionFacing, positionChange.normalized) > 90 ? 1 : -1;
                movement = directionFacing * resistance * directionAdjustMultiplier;

                animator.SetBool("Forward", directionAdjustMultiplier == -1);

            }

        }

        animator.SetBool("On", movingUser);

        foreach (Transform t in positionsOfUsers.Keys)
        {

            t.position = t.position - movement;
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
