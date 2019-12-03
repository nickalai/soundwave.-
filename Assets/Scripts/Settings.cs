using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings instance;
    public int framerate = 60;

    // Singleton; used to ensure only one instance of DDOL values exists at any given time
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
