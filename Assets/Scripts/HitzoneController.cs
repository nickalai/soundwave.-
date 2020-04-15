/*
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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RotateZone();
    }

    void RotateZone()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.RotateAround(centerPivot.position, Vector3.right, rotationSensitivity * Time.deltaTime);
        }

        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            transform.RotateAround(centerPivot.position, Vector3.left, rotationSensitivity * Time.deltaTime);
        }
    }

    #endregion
}

// https://en.wikipedia.org/wiki/Tempest_(video_game) casual plug for free boomer points
