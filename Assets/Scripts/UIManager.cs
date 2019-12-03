using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text scoreText;
    public Text comboText;

    GameManager gm;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    // Update is called once per frame
    void Update()
    {
        scoreText.text = gm.currentScore.ToString();
        comboText.text = gm.comboCounter.ToString() + "x";
    }
}
