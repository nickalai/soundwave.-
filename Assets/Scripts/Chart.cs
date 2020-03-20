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

public class Chart : MonoBehaviour
{
    public int ID { get; set; }
    public string songName { get; set; }
    public Koreography koreo { get; set; }
    public bool completed { get; set; }
    public int difficulty { get; set; }
    public int highScore { get; set; }
    public bool locked { get; set; }

    public Chart(int id, string song, Koreography track, bool comp, int diff, int score, bool lockedStatus)
    {
        this.ID = id;
        this.songName = song;
        this.koreo = track;
        this.completed = comp;
        this.difficulty = diff;
        this.highScore = score;
        this.locked = lockedStatus;
    }

    public void Complete()
    {
        this.completed = true;
    }

    public void Complete(int score)
    {
        this.completed = true;
        this.highScore = score;
    }

    public void Lock()
    {
        this.locked = true;
    }

    public void Unlock()
    {
        this.locked = false;
    }
}
