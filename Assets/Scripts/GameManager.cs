/*
Name: Nick Lai
Student ID#: 2282417
Chapman email: lai137@mail.chapman.edu
Course Number and Section: 440-01
Project: soundwave.-
Contains logic for note spawning and destruction
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class GameManager : MonoBehaviour
{
    #region Variables
    [Tooltip("The EventID of the track to use for target generation.")]
    [EventID]
    public string eventID;

    [Tooltip("The number of milliseconds (both early and late) within which input will be detected as a hit.")]
    [Range(8f, 150f)]
    public float hitWindowRangeInMS = 80f;

    [Tooltip("The number of units traversed per second by Notes.")]
    public float noteSpeed = 1f;

    [Tooltip("The archetype to use for generating notes. Can be a prefab.")]
    public Notes notePrefab;
    public HoldNotes holdNotePrefab;

    [Tooltip("The list of LaneManager objects that represent a lane for an event to travel down.")]
    public List<LaneManager> noteLanes = new List<LaneManager>();

    [Tooltip("The amount of time in seconds to provide before playback of the audio begins. Changes to this value are not immediately handled during the lead-in phase while playing in the editor.")]
    public float leadInTime;

    [Tooltip("The AudioSource through which the Koreographed audio will be played. Be sure to disable 'Play on Awake' in the Music Player.")]
    public AudioSource aus;

    // The amount of lead-in time left before the audio is audible.
    float leadInTimeLeft;

    // The amount of time left before audio is played (handles Event Delay).
    float timeLeftToPlay;

    // Local cache of the Koreography loaded into the Koreographer component.
    Koreography playingKoreo;

    // Sample range within which a viable event may be hit.
    int hitWindowRangeInSamples;

    // Score tracking variables
    public int currentScore = 0;
    public int scorePerNote = 300;
    public int comboCounter = 0;

    // Pool for containing Notes to reduce unnecessary Instatiation/Destruction.
    Stack<Notes> notePool = new Stack<Notes>();
    Stack<HoldNotes> holdNotePool = new Stack<HoldNotes>();

    #endregion
    #region Return Statements
    // Public access to the hit window
    public int HitWindowSampleWidth
    {
        get
        {
            return hitWindowRangeInSamples;
        }
    }

    // Access to the current hit window size in Unity units.
    public float WindowSizeInUnits
    {
        get
        {
            return noteSpeed * (hitWindowRangeInMS * 0.001f);
        }
    }

    // Sample Rate specified by Koreography
    public int SampleRate
    {
        get
        {
            return playingKoreo.SampleRate;
        }
    }

    // Current sample time, including any necessary delays.
    public int DelayedSampleTime
    {
        get
        {
            return playingKoreo.GetLatestSampleTime() - (int)(aus.pitch * leadInTimeLeft * SampleRate);
        }
    }

    #endregion
    #region Methods
    // Start is called before the first frame update
    void Start()
    {
        InitializeLeadIn();

        // Init all Lanes.
        for(int i = 0; i < noteLanes.Count; ++i)
        {
            noteLanes[i].Initialize(this);
        }

        // Init events.
        playingKoreo = Koreographer.Instance.GetKoreographyAtIndex(0);

        // Grab all events from Koreography.
        KoreographyTrackBase rhythmTrack = playingKoreo.GetTrackByID(eventID);
        List<KoreographyEvent> rawEvents = rhythmTrack.GetAllEvents();

        for(int i = 0; i < rawEvents.Count; ++i)
        {
            KoreographyEvent evt = rawEvents[i];
            string payload = evt.GetTextValue();

            // Finds correct Lane.
            for (int j = 0; j < noteLanes.Count; ++j)
            {
                LaneManager lane = noteLanes[j];
                if (lane.DoesMatchPayloadHit(payload))
                {
                    // Adds object for input tracking.
                    lane.AddEventToLane(evt);
                    break;
                }
                else if (lane.DoesMatchPayloadHold(payload))
                {
                    //Adds object for input traccking.
                    lane.AddEventToLane(evt);
                    break;
                }
            }
        }
    }

    // Sets up the lead-in time. Begins audio playback immediately if the specified lead-in time is 0.
    void InitializeLeadIn()
    {
        // Inits lead-in time only if specified.
        if (leadInTime > 0f)
        {
            // Sets up to delay the beginning of playback.
            leadInTimeLeft = leadInTime;
            timeLeftToPlay = leadInTime - Koreographer.Instance.EventDelayInSeconds;
        }

        else
        {
            aus.time = -leadInTime;
            aus.Play();
        }
    }

    // Update is called once per frame.
    void Update()
    {
        UpdateInternalValues();
        // Countdown some of lead-in time.
        if(leadInTimeLeft > 0f)
        {
            leadInTimeLeft = Mathf.Max(leadInTimeLeft - Time.unscaledDeltaTime, 0f);
        }

        // Countdown th time left to play if necessary.
        if(timeLeftToPlay > 0f)
        {
            timeLeftToPlay -= Time.unscaledDeltaTime;
            // Checks if it is time to begin playback.
            if(timeLeftToPlay <= 0f)
            {
                aus.time = -timeLeftToPlay;
                aus.Play();

                timeLeftToPlay = 0f;
            }
        }
    }

    // Update any internal values that depend on externally accessible fields (public/inspector).
    void UpdateInternalValues()
    {
        hitWindowRangeInSamples = (int)(0.001f * hitWindowRangeInMS * SampleRate);
    }

    // Gets a freshly activated Note from the pool.
    public Notes GetFreshNote()
    {
        Notes retObj;

        if(notePool.Count > 0)
        {
            retObj = notePool.Pop();
        }
        else
        {
            retObj = GameObject.Instantiate<Notes>(notePrefab);
        }

        retObj.gameObject.SetActive(true);
        retObj.enabled = true;

        return retObj;
    }
    
    // Gets a freshly activated HoldNote from the pool.
    public HoldNotes GetFreshHoldNote()
    {
        HoldNotes retObj;

        if (holdNotePool.Count > 0)
        {
            retObj = holdNotePool.Pop();
        }
        else
        {
            retObj = GameObject.Instantiate<HoldNotes>(holdNotePrefab);
        }

        retObj.gameObject.SetActive(true);
        retObj.enabled = true;

        return retObj;
    }

    // Deactivates and returns a Note to the pool.
    public void ReturnNoteToPool(Notes note)
    {
        if (note != null)
        {
            note.enabled = false;
            note.gameObject.SetActive(false);
            notePool.Push(note);
        }
    }

    // Deactivates and returns a HoldNote to the pool.
    public void ReturnHoldNoteToPool(HoldNotes holdNote)
    {
        if (holdNote != null)
        {
            holdNote.enabled = false;
            holdNote.gameObject.SetActive(false);
            holdNotePool.Push(holdNote);
        }
    }

    // Restarts the game, causing all Lanes and any active Notes to reset/clear.
    public void Restart()
    {
        currentScore = 0;
        comboCounter = 0;
        // Resets audio
        aus.Stop();
        aus.time = 0f;

        // Flushes the queue of delayed event updates. Resets Koreography and ensures delayed events that haven't been sent yet don't continue to be sent.
        Koreographer.Instance.FlushDelayQueue(playingKoreo);
        // Resets Koreography time.
        playingKoreo.ResetTimings();
        // Resets lanes so that tracking starts over.
        for (int i = 0; i < noteLanes.Count; ++i)
        {
            noteLanes[i].Restart();
        }
        //Reinit the lead-in timing.
        InitializeLeadIn();
    }

    #endregion
}
