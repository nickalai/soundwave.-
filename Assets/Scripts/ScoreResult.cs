using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreResult : MonoBehaviour
{
    public static ScoreDDOL sDDOL;
    public Text score;
    public Text combo;
    // Start is called before the first frame update
    void Start()
    {
        sDDOL = GameObject.FindGameObjectWithTag("ScoreDDOL").GetComponent<ScoreDDOL>();
        assignText();
    }   

    void assignText() {
        score.text = sDDOL.score.ToString();
        combo.text = sDDOL.currentCombo.ToString();
    }
}
