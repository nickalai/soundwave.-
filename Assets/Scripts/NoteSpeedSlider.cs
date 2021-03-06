﻿/*
Name: Nick Lai
Student ID#: 2282417
Chapman email: lai137@mail.chapman.edu
Course Number and Section: 440-01
Project: soundwave.-
Contains logic for changing note travel speed in game
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteSpeedSlider : MonoBehaviour
{
    [Tooltip("The Text Component that will display the Speed number.")]
    public Text readoutText;

    [Tooltip("The Slider that controls the Pitch number.")]
    public Slider slider;

    public GameManager gm;

    void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        float speed = gm.noteSpeed;
        slider.value = speed;
        SetNewNoteSpeed(slider.value);
    }

    public void SetNewNoteSpeed(float newNoteSpeed)
    {
        gm.noteSpeed = newNoteSpeed;
        readoutText.text = newNoteSpeed.ToString("0.####") + "x";
    }
}
