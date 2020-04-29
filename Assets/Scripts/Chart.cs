/*
Name: Nick Lai
Student ID#: 2282417
Chapman email: lai137@mail.chapman.edu
Course Number and Section: 440-01
Project: soundwave.-
Stores information on each chart
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

[CreateAssetMenu(fileName = "New Chart", menuName = "Charts")]
public class Chart : ScriptableObject
{
    public int ID;
    public string songName;
    public Koreography koreo;
    public string eventID;

    public string difficulty;
    public int difficultyNum;
    public int highScore;
    public bool completed;
}