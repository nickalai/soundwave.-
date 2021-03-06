﻿/*
Name: Nick Lai
Student ID#: 2282417
Chapman email: lai137@mail.chapman.edu
Course Number and Section: 440-01
Project: soundwave.-
Contains logic for controlling the hit zone
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitzoneController : MonoBehaviour
{
    #region Variables
    public Transform centerPivot;
    public float rotationSensitivity = 200;

    #endregion
    #region Methods
    // Update is called once per frame
    void Update()
    {
        RotateZone();
    }

    // Rotates the indicator
    void RotateZone()
    {
        if (Input.GetAxis("Mouse X") < 0)
        {
            transform.Rotate(Vector3.forward * rotationSensitivity * Time.deltaTime);
        }

        else if (Input.GetAxis("Mouse X") > 0)
        {
            transform.Rotate(-Vector3.forward * rotationSensitivity * Time.deltaTime);
        }
    }

    #endregion
}

