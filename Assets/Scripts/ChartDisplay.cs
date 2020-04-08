using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChartDisplay : MonoBehaviour
{
    public Chart chart;
    public Text chartName;
    public Text chartDifficulty;
    public Text chartDifficultyNum;
    public Text chartHighScore;
    public GameObject completedChart;

    // Start is called before the first frame update
    void Start()
    {
        DisplayChart();
    }

    public void DisplayChart()
    {
        chartName.text = chart.name;
        chartDifficulty.text = chart.difficulty;
        chartDifficultyNum.text = chart.difficultyNum.ToString();
        chartHighScore.text = chart.highScore.ToString();
        if (chart.completed == true)
        {
            completedChart.SetActive(true);
        }

    }
}
