using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class ScoreDDOL : MonoBehaviour
{
    public static ScoreDDOL instance;
    public int highestCombo = 0;
    public int currentCombo = 0;
    public int totalMisses = 0;
    public int score = 0;

    GameManager gm;
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

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ComboCheck()
    {
        if (currentCombo > highestCombo)
            highestCombo = currentCombo;
    }

}