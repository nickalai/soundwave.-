﻿/*
Name: Nick Lai
Student ID#: 2282417
Chapman email: lai137@mail.chapman.edu
Course Number and Section: 340-02
Project: soundwave.-
Contains logic for menu objects
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string levelToLoad;

    // Quits the game
    public void Quit()
    {
        Application.Quit();
    }

    // Starts the game by loading the first level
    public void Play()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
