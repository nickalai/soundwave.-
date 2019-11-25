using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string levelToLoad;
    void Start()
    {
  
    }

    // Start is called before the first frame update
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
