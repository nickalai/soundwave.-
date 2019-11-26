/*
Name: Nick Lai
Student ID#: 2282417
Chapman email: lai137@mail.chapman.edu
Course Number and Section: 340-02
Project: soundwave.-
Contains logic for the pause menu
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject pauseMenuUI;

    // Update is called once per frame. Checks if the game is paused or not
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }

            else if (!isPaused)
            {
                Pause();
            }
        }
    }

    // Contains logic for pausing the game
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    // Contains logic for resuming the game
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    // Reloads the current active scene
    public void Retry()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Contains logic for returning to the Song Select screen
    public void Quit()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("SongSelect");
    }
}