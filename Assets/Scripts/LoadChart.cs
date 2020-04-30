using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class LoadChart : MonoBehaviour
{
    public static LoadChart instance;
    public Koreography chartToLoad;
    public string eventIDToLoad;
    public Chart current;

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

    public void setChart(Chart chart) {
        chartToLoad = chart.koreo;
        eventIDToLoad = chart.eventID;
    }
}
