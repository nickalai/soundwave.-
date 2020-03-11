/*
Name: Nick Lai
Student ID#: 2282417
Chapman email: lai137@mail.chapman.edu
Course Number and Section: 440-01
Project: soundwave.-
Contains logic for managing the lanes
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using UnityEngine.UI;

public class LaneManager : MonoBehaviour
{
    #region Variables
    [Tooltip("A reference to the visuals for the \"target\" location.")]
    public GameObject targetVisuals;

    [Tooltip("The keyboard button used by this lane.")]
    public KeyCode keyboardButton;

    [Tooltip("The controller button used by this lane [NOT CURRENTLY USED].")]
    public string controllerButton;

    [Tooltip("A list of Payload strings that Koreography Events will contain for this lane.")]
    public List<string> matchedPayloads = new List<string>();

    // List that contains all events for a lane. Added by the GameManager.
    List<KoreographyEvent> laneEvents = new List<KoreographyEvent>();

    // Queue that contains all Notes currently on screen within this lane.
    Queue<Notes> trackedNotes = new Queue<Notes>();

    // Reference to the GameManager. Provides access to the Notes pool and other parameters.
    GameManager gm;

    // Lifetime boundaries.
    float spawnZ = 0f;
    public float despawnZ = 0f;

    // Index of the next event to check for spawn timing in this lane.
    int pendingEventIndex = 0;

    // Feedback Scales used for resizing buttons on press.
    Vector3 defaultScale;
    Quaternion defaultRotation;
    float scaleNormal = 1f;
    float scalePress = 1.4f;
    float scaleHold = 1.2f;

    // hit note materials
    public Material Note;
    public Material hitNote;

    Notes currentSpan = null;

    #endregion
    #region Return Statements
    // Position at which new Notes should spawn
    public Vector3 SpawnPosition
    {
        get
        {
            return new Vector3(transform.position.x, transform.position.y, spawnZ);
        }
    }

    // The position at which the timing target exists.
    public Vector3 TargetPosition
    {
        get
        {
            return new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
    }

    public Quaternion TargetRotation
    { 
        get
        {
            return transform.rotation;
        }
    }

    // The position at which Notes should despawn and return to the pool.
    public float DespawnZ
    {
        get
        {
            return despawnZ;
        }
    }
    #endregion
    #region Methods

    public void Initialize(GameManager manager)
    {
        gm = manager;
    }

    // Controls cleanup, resets internals to a fresh state.
    public void Restart(int newSampleTime = 0)
    {
        // Finds the index of the first event at or beyond the target sample time.
        for (int i = 0; i < laneEvents.Count; ++i)
        {
            if(laneEvents[i].StartSample >= newSampleTime)
            {
                pendingEventIndex = i;
                break;
            }
        }
        // Clears out tracked notes.
        int numToClear = trackedNotes.Count;
        for (int i = 0; i < numToClear; ++i)
        {
            trackedNotes.Dequeue().OnClear();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Gets the vertical bounds of the camera. Offset by a bit to allow for offscrean spawning/removal
        float cameraOffsetZ = -Camera.main.transform.position.z;
        spawnZ = 3f; // Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, cameraOffsetZ)).z + 3f; PROPER WAY, Currently hard coded.
        despawnZ = -9f;//Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, cameraOffsetZ)).z - 1f; PROPER WAY, Currently hard coded.

        defaultScale = targetVisuals.transform.localScale;
        defaultRotation = targetVisuals.transform.rotation;
        ResetMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        // Clears out invalid hit notes.
        while (trackedNotes.Count > 0 && trackedNotes.Peek().IsOneOff() && trackedNotes.Peek().IsNoteMissed())
        {
            gm.comboCounter = 0;
            trackedNotes.Dequeue();
        }

        while (trackedNotes.Count > 0 && !trackedNotes.Peek().IsOneOff() && trackedNotes.Peek().IsSpanNoteMissed())
        {
            gm.comboCounter = 0;
            trackedNotes.Dequeue();
        }

        // Checks for new spawns.
        CheckSpawnNext();

        // Checks if a span note is being hit
        if (currentSpan != null)
        {
            if(currentSpan.IsSpanNoteMissed())
            {
                currentSpan.CancelInvoke("SpanNoteScore");
                currentSpan.OnHit();
                currentSpan = null;
            }
        }

        // Checks for input
        if (Input.GetKeyDown(keyboardButton))
        {
            CheckNoteHit();
            //SetScalePress();
            OnHitMaterial();
        }

        else if (Input.GetKey(keyboardButton))
        {
            SetScaleHold();
        }

        else if (Input.GetKeyUp(keyboardButton))
        {
            SetScaleDefault();
            ResetMaterial();
            if (currentSpan != null)
            {
                if (currentSpan.IsNoteEndHittable())
                {
                    Debug.Log("Good timing bonus");
                }
                currentSpan.CancelInvoke("SpanNoteScore");
                currentSpan = null;
            }
        }
    }

    // Adjusts the scale with a multiplier against default scale.
    void AdjustScale(float multiplier)
    {
        targetVisuals.transform.localScale = defaultScale * multiplier;
    }

    // Uses the Target position and the current Note speed to determine audio sample "position" of the spawn location. Relative to the audio sample position at the "now" time.
    int GetSpawnSampleOffset()
    {
        // Determines the sample offset at the current speed.
        float spawnDistToTarget = spawnZ - transform.position.z;
        // Determines time to location at the current speed.
        double spawnSecsToTarget = (double)spawnDistToTarget / (double)gm.noteSpeed;
        // Returns samples to the target.
        return (int)(spawnSecsToTarget * gm.SampleRate);
    }

    // Checks if a Note is hit. If hit, will perform the Hit and remove the object from trackedNotes.
    public void CheckNoteHit()
    {
        // Ensure that there are notes to check
        if (trackedNotes.Count > 0)
        {
            Notes hitNote = trackedNotes.Peek();
            if (hitNote == null)
                return;
            else if (hitNote.IsOneOff())
            {
                if (hitNote.IsNoteHittable())
                {
                    Notes curNote = trackedNotes.Dequeue();
                    curNote.OnHit();
                }
            }
            else
            {
                if(hitNote.IsSpanNoteHittable())
                {
                    currentSpan = trackedNotes.Dequeue();
                    currentSpan.InvokeRepeating("SpanNoteScore", 0, 0.01f);
                    Debug.Log("Span hit");
                }
            }
        }
        /*
        // Always check only the first event as we clear out missed entries before.
        if (trackedNotes.Count > 0 && trackedNotes.Peek().IsNoteHittable())
        {
            Notes hitNote = trackedNotes.Dequeue();
            hitNote.OnHit();
        }
        */
    }

    // Checks if the next Note should be spawned. If true, spawns the Note and adds it to trackedNotes.
    void CheckSpawnNext()
    {
        int samplesToTarget = GetSpawnSampleOffset();
        int currentTime = gm.DelayedSampleTime;
        // Spawn for all events within range.
        while (pendingEventIndex < laneEvents.Count && laneEvents[pendingEventIndex].StartSample < currentTime + samplesToTarget)
        {
            KoreographyEvent evt = laneEvents[pendingEventIndex];
            string payload = evt.GetTextValue();

            Notes freshNote = gm.GetFreshNote();
            freshNote.InitializeNote(evt, this, gm);
            
            trackedNotes.Enqueue(freshNote);
        
            pendingEventIndex++;
        }
    }

    // Adds Koreography event to the Lane. Contains the timing information that defines when a Note should appear on screen.
    public void AddEventToLane(KoreographyEvent evt)
    {
        laneEvents.Add(evt);
    }

    // Checks to see if the string value passed in matches any of the configured values specified in the matchedPayloads list for HIT notes.
    public bool DoesMatchPayloadHit(string payload)
    {
        bool isMatched = false;

        for (int i = 0; i < matchedPayloads.Count; ++i)
        {
            if (payload == matchedPayloads[i])
            {
                isMatched = true;
                break;
            }
        }
        return isMatched;
    }

    // Checks to see if the string value passed in matches any of the configured values specified in the matchedPayloads list for HOLD notes.
    public bool DoesMatchPayloadHold(string payload)
    {
        bool isMatched = false;
        for (int i = 0; i < matchedPayloads.Count; ++i)
        {
            if (payload.Length == 2 && (payload.Substring(0, 1)) == matchedPayloads[i] && (payload.Substring(1, 1)) == "h")
            {
                isMatched = true;
                break;
            }
        }
        return isMatched;
    }

    // Sets the Target scale to the original default scale.
    public void SetScaleDefault()
    {
        AdjustScale(scaleNormal);
    }

    // Sets the Target scale to the specified "initially pressed" scale.
    public void SetScalePress()
    {
        AdjustScale(scalePress);
    }

    // SEts the Target scale to the specified "continuously held" scale.
    public void SetScaleHold()
    {
        AdjustScale(scaleHold);
    }
    #endregion

    // Sets target material to hit material
    public void OnHitMaterial() 
    {
        this.gameObject.GetComponent<MeshRenderer>().material = hitNote;
    }

    public void ResetMaterial() 
    {
        this.gameObject.GetComponent<MeshRenderer>().material = Note;
    }
}
