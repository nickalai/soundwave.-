/*
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

    LoadChart lc;

    void Start()
    {
        lc = GameObject.FindGameObjectWithTag("ChartLoader").GetComponent<LoadChart>();
    }
<<<<<<< HEAD

    void Update() {
        if (Input.GetKeyDown("space")) {
            Play();
        }

    }
=======
>>>>>>> be89d22419e5954e78d5cf603884a0c72643df92
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

    public void LoadChartToPlay(Chart chart)
    {
        lc.chartToLoad = chart.koreo;
        SceneManager.LoadScene("Circle Sample");
    }
}
