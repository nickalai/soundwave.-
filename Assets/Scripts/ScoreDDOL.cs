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
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        ComboCheck();
        currentCombo = gm.comboCounter;
        totalMisses = gm.misses;
        score = gm.currentScore;
    }

    void ComboCheck()
    {
        if (currentCombo > highestCombo)
            highestCombo = currentCombo;
    }

}