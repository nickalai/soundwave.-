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
    public KoreographyEvent trackedEvent;
    LaneManager lm;
    GameManager gm;
    public GameObject HitEffect_1;
    public GameObject HitEffect_2;

    public float scoreAmt;

    private float defaultNoteZ;

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
        defaultNoteZ = transform.localScale.z;

        UpdateNotePosition();
        UpdateNoteLength();
    }

    // Resets note to default state
    public void Reset()
    {
        trackedEvent = null;
        lm = null;
        gm = null;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, defaultNoteZ);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateNotePosition();
    }
    
    void UpdateNoteLength()
    {
        if (!trackedEvent.IsOneOff())
        {
            float baseUnitHeight = visuals.GetComponent<Renderer>().bounds.size.z;

            float fullHeight = gm.GetVerticalUnitOffsetForSampleTime(trackedEvent.StartSample) - gm.GetVerticalUnitOffsetForSampleTime(trackedEvent.EndSample);
            float hitWindowHeight = gm.WindowSizeInUnits * 2f;
            float targetUnitHeight = fullHeight + hitWindowHeight;

            Vector3 scale = transform.localScale;
            scale.z = targetUnitHeight / baseUnitHeight;
            transform.localScale = scale;
        }   
    }

    // Updates the note's position along the lane based on the current audio position
    void UpdateNotePosition()
    {
        float samplesPerUnit = 0.5f * (gm.GetVerticalUnitOffsetForSampleTime(trackedEvent.StartSample) - gm.GetVerticalUnitOffsetForSampleTime(trackedEvent.EndSample));

        Vector3 dir = lm.TargetPosition - lm.SpawnPosition;

        dir.Normalize();
        float remainingDist = gm.GetVerticalUnitOffsetForSampleTime(trackedEvent.StartSample) - samplesPerUnit;
        Vector3 offset = dir * remainingDist;

        transform.position = lm.TargetPosition + offset;
        transform.rotation = lm.TargetRotation;
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
        gm.comboCounter++;
        gm.currentScore += gm.scorePerNote * gm.comboCounter;

        Vector3 pos = lm.TargetPosition;
        GameObject effect = Instantiate(HitEffect_1, pos, lm.TargetRotation);

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
        gm.currentScore += (int)(10 * Time.deltaTime);
    }
    
    #endregion
}
