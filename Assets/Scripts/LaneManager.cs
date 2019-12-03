using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

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
    float despawnZ = 0f;

    // Index of the next event to check for spawn timing in this lane.
    int pendingEventIndex = 0;

    // Feedback Scales used for resizing buttons on press.
    Vector3 defaultScale;
    float scaleNormal = 1f;
    float scalePress = 1.4f;
    float scaleHold = 1.2f;

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
        spawnZ = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, cameraOffsetZ)).y + 1f;
        despawnZ = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, cameraOffsetZ)).z - 1f;

        defaultScale = targetVisuals.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Clears out invalid entries.
        while (trackedNotes.Count > 0 && trackedNotes.Peek().IsNoteMissed())
        {
            trackedNotes.Dequeue();
        }
        // Checks for new spawns.
        CheckSpawnNext();
        // Checks for input
        if (Input.GetKeyDown(keyboardButton))
        {
            CheckNoteHit();
            SetScalePress();
        }
        else if (Input.GetKey(keyboardButton))
        {
            SetScaleHold();
        }
        else if (Input.GetKeyUp(keyboardButton))
        {
            SetScaleDefault();
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
        // Always check only the first event as we clear out missed entries before.
        if (trackedNotes.Count > 0 && trackedNotes.Peek().IsNoteHittable())
        {
            Notes hitNote = trackedNotes.Dequeue();
            hitNote.OnHit();
        }
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

    // Checks to see if the string value passed in matches any of the configured values specified in the matchedPayloads list.
    public bool DoesMatchPayload(string payload)
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
}
