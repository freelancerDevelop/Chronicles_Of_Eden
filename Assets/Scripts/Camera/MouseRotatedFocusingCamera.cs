﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A focusing camera that can be made to rotate left and right with the mouse.
/// </summary>
public class MouseRotatedFocusingCamera : FocusingCamera {

    public bool lockRotation; //If true, the camera will not rotate.

    [SerializeField] MouseInputManager mouseInput;
    [SerializeField] float sensitivity; //How much the camera moves in response to the mouse.

    private void OnEnable()
    {
        mouseInput.mouseMoved += OnMouseMove;
    }

    private void OnDisable()
    {
        mouseInput.mouseMoved -= OnMouseMove;
    }

    private void OnMouseMove(float deltaX, float deltaY, float deltaZ)
    {
        if (lockRotation || deltaY == 0) return;

        angleAroundTarget -= sensitivity * deltaX;

    }

}
