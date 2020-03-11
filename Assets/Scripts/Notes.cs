/*
Name: Nick Lai
Student ID#: 2282417
Chapman email: lai137@mail.chapman.edu
Course Number and Section: 440-01
Project: soundwave.-
Contains logic for hit notes
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class Notes : MonoBehaviour
{
    #region Variables
    [Tooltip("Visuals for notes")]
    public GameObject visuals;
    KoreographyEvent trackedEvent;
    LaneManager lm;
    GameManager gm;
    public GameObject HitEffect_1;

    public float scoreAmt;

    #endregion
    #region Return Statements
    // Unclamped Lerp, same as Vector3.lerp without [0.0~1.0] clamping
    static Vector3 Lerp(Vector3 from, Vector3 to, float t)
    {
        return new Vector3(from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t, from.z + (to.z - from.z) * t);
    }

    #endregion
    #region Methods
    // Prepares note for use
    public void InitializeNote(KoreographyEvent evt, LaneManager laneManager, GameManager gameManager)
    {
        trackedEvent = evt;
        lm = laneManager;
        gm = gameManager;

        UpdateNotePosition();
    }

    // Resets note to default state
    void Reset()
    {
        trackedEvent = null;
        lm = null;
        gm = null;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateNoteLength();
        UpdateNotePosition();

        if (transform.position.z <= lm.DespawnZ)
        {
            gm.ReturnNoteToPool(this);
            Reset();
        }
    }

    public bool IsOneOff()
    {
        return trackedEvent.IsOneOff();
    }

    void UpdateNoteLength()
    {

    }

    // Updates the note's position along the lane based on the current audio position
    void UpdateNotePosition()
    {
        float samplesPerUnit = gm.SampleRate / gm.noteSpeed;

        Vector3 pos = lm.TargetPosition;
        pos.z -= (gm.DelayedSampleTime - trackedEvent.StartSample) / samplesPerUnit;
        transform.position = pos;
        transform.rotation = lm.TargetRotation;
    }

    // Checks whether or not you can hit a note based on the current audio sample
    public bool IsNoteHittable()
    {
        int noteTime = trackedEvent.StartSample;
        int curTime = gm.DelayedSampleTime;
        int hitWindow = gm.HitWindowSampleWidth;

        return (Mathf.Abs(noteTime - curTime) <= hitWindow);
    }

    public bool IsSpanNoteHittable()
    {
        int noteTime = trackedEvent.StartSample;
        int curTime = gm.DelayedSampleTime;
        int hitWindow = gm.HitWindowSampleWidth;

        return (noteTime - curTime) <= hitWindow;
    }

    public bool IsNoteEndHittable()
    {
        bool isHittable = false;
        if (!trackedEvent.IsOneOff())
        {
            int noteTime = trackedEvent.EndSample;
            int curTime = gm.DelayedSampleTime;
            int hitWindow = gm.HitWindowSampleWidth;
            isHittable = (Mathf.Abs(noteTime - curTime) <= hitWindow);
        }
        return isHittable;
    }

    // Checks if a note is no longer able to be hit based on the given hit window
    public bool IsNoteMissed()
    {
        bool isMissed = true;

        if(enabled)
        {
            int noteTime = trackedEvent.StartSample;
            int curTime = gm.DelayedSampleTime;
            int hitWindow = gm.HitWindowSampleWidth;

            isMissed = (curTime - noteTime > hitWindow);
        }

        return isMissed;
    }

    public bool IsSpanNoteMissed()
    {
        bool isMissed = true;
        if (enabled)
        {
            int noteTime = trackedEvent.EndSample;
            int curTime = gm.DelayedSampleTime;
            int hitWindow = gm.HitWindowSampleWidth;

            isMissed = (curTime - noteTime > hitWindow);
        }
        return isMissed;
    }

    // Returns note to the pool which is controlled by the GameManager. Used to reduce runtime allocations
    void ReturnToPool()
    {
        gm.ReturnNoteToPool(this);
        Reset();
    }

    // Performs action when a note is hit
    public void OnHit()
    {
        if (gm.comboCounter == 0)
        {
            gm.currentScore += gm.scorePerNote;
        }
        else if (gm.comboCounter > 0)
        {
            gm.currentScore += gm.scorePerNote * gm.comboCounter;
        }
        gm.comboCounter++;

        Instantiate(HitEffect_1, new Vector3(transform.position.x, 1, -11), Quaternion.identity);

        ReturnToPool();
    }

    // Performs action when a note is cleared
    public void OnClear()
    {
        ReturnToPool();
    }

    // Span note scoring
    public void SpanNoteScore()
    {
        scoreAmt += 100 * Time.deltaTime;
    }

    #endregion
}


//https://www.tapatalk.com/groups/koreographer/rhythm-game-note-length-t57-s20.html resource for getting hold notes in im a failure
