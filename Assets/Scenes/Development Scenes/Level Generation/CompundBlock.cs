﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CompundBlock : MonoBehaviour {

    [SerializeField] GameObject unitShape;
    [SerializeField] Vector3 blocks;

    private Vector3 prevDimensions = Vector3.zero;
    private GameObject prevUnit;

    private void OnValidate()
    {

        if (unitShape == null || (ReferenceEquals(prevUnit, unitShape) && blocks.Equals(prevDimensions))) return;

        foreach (Transform child in transform)
            child.gameObject.SetActive(false);

        for(float x = transform.position.x - blocks.x / 2; x < transform.position.x + blocks.x / 2; x++)
        {
            for (float y = transform.position.y - blocks.y / 2; y < transform.position.y + blocks.y / 2; y++)
            {
                for (float z = transform.position.z - blocks.z / 2; z < transform.position.z + blocks.z / 2; z++)
                {
                    GameObject shapeClone = Instantiate(unitShape, transform);
                    shapeClone.transform.position = new Vector3(x, y, z);
                }
            }
        }

        prevDimensions = blocks;

    }

    
}
