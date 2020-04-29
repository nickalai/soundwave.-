﻿/*
Name: Nick Lai
Student ID#: 2282417
Chapman email: lai137@mail.chapman.edu
Course Number and Section: 440-01
Project: soundwave.-
Contains logic for menu objects
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string levelToLoad;

    //scene transition variables 

        //get animator component
    public Animator transition;
        //set transition time 
    public float transitionTime = 1f;

    LoadChart lc;

    void Start()
    {
        lc = GameObject.FindGameObjectWithTag("ChartLoader").GetComponent<LoadChart>();
    }
    void Update() {
        if (Input.GetKeyDown("space")) {
            Play();
        }

    }

    // Quits the game
    public void Quit()
    {
        Application.Quit();
    }

    // Starts the game by loading the first level
    public void Play()
    {
        StartCoroutine(LoadScene(levelToLoad));
    }

    //load scene and start transition animation
    IEnumerator LoadScene(string levelToLoad) {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelToLoad);
    }

    public void LoadChartToPlay(Chart chart)
    {
        lc.chartToLoad = chart.koreo;
        lc.eventIDToLoad = chart.eventID;
    }
}
