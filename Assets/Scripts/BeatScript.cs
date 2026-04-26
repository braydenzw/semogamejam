using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class BeatScript : MonoBehaviour
{
    private float timeToClick;
    private float timingWindow;
    private NoteDirection noteDirection;
    private Vector3 targetPosition;
    private Vector3 startingPosition;
    private double timeAlive;
    private NoteType noteType;

    // --- Multi-hit (Percussion) ---
    private int hitsRequired;
    private int hitsLanded;
    private bool multiHitStarted;
    private float multiHitTimer;
    [SerializeField] private float multiHitWindowDuration = 0.5f; // seconds to land all taps

    // --- Hold (Strings) ---
    private float holdDuration;
    private float holdTime;
    private bool isBeingHeld;

    // --- Visuals ---
    // Assign these in the Beat prefab Inspector.
    // Each is a child GameObject with its own Animator/SpriteRenderer.
    [Header("Note Visuals")]
    [SerializeField] private GameObject standardVisual;   // quarter note  — Brass & Woodwinds
    [SerializeField] private GameObject multiHitVisual;   // eighth note pair — Percussion
    [SerializeField] private GameObject holdVisual;       // half/whole note — Strings
    [SerializeField] private Transform holdProgressBar;   // optional: child Transform scaled on X (0→1) as hold fills

    void Update()
    {
        timeAlive += Time.deltaTime;

        // Note moves toward target unless being held (hold notes freeze at target)
        if (!isBeingHeld)
            transform.position += Time.deltaTime * (targetPosition - startingPosition) / timeToClick;

        // Hold progress
        if (isBeingHeld)
        {
            holdTime += Time.deltaTime;
            if (holdProgressBar != null)
                holdProgressBar.localScale = new Vector3(
                    Mathf.Clamp01(holdTime / holdDuration), 1f, 1f);
            if (holdTime >= holdDuration)
                HoldSuccess();
        }

        // Multi-hit window timeout — if player didn't land all taps in time, miss
        if (multiHitStarted)
        {
            multiHitTimer += Time.deltaTime;
            if (multiHitTimer >= multiHitWindowDuration)
                OnMiss();
        }
        if(timeAlive>timeToClick+0.02)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void OnMiss()
    {
        GetComponentInParent<BeatManagerScript>().BeatDeath(this.gameObject,BeatScore.Failure,noteDirection);
    }

        // Standard timeout — miss if past window and not in hold or multi-hit phase
        if (!isBeingHeld && !multiHitStarted && timeAlive >= timeToClick + timingWindow)
            OnMiss();
    }

    // Called by BeatManagerScript when this direction's key is pressed
    public void OnHit()
    {
        switch (noteType)
        {
            case NoteType.Standard:
                if (Math.Abs(timeAlive - timeToClick) <= timingWindow)
                {
                    Debug.Log("Standard note hit successfully!");
                    GetComponentInParent<BeatManagerScript>().BeatDeath(gameObject, BeatScore.Success, noteDirection);
                }
                else
                    OnMiss();
                break;

            case NoteType.MultiHit:
                if (!multiHitStarted)
                {
                    // First tap must land within the normal timing window
                    if (Math.Abs(timeAlive - timeToClick) <= timingWindow)
                    {
                        hitsLanded = 1;
                        multiHitStarted = true;
                        multiHitTimer = 0f;
                        if (hitsLanded >= hitsRequired)
                            GetComponentInParent<BeatManagerScript>().BeatDeath(gameObject, BeatScore.Success, noteDirection);
                    }
                    else
                    {
                        OnMiss();
                    }
                }
                else
                {
                    // Each subsequent tap resets the window and counts toward the total
                    hitsLanded++;
                    multiHitTimer = 0f;
                    if (hitsLanded >= hitsRequired)
                        GetComponentInParent<BeatManagerScript>().BeatDeath(gameObject, BeatScore.Success, noteDirection);
                }
                break;

            case NoteType.Hold:
                if (Math.Abs(timeAlive - timeToClick) <= timingWindow)
                {
                    isBeingHeld = true;
                    holdTime = 1f;
                    transform.position = targetPosition; // freeze note
                    if (holdProgressBar != null)
                        holdProgressBar.localScale = new Vector3(0f, 1f, 1f); // reset bar
                }
                else
                    OnMiss();
                break;
        }
        
        
    }

    // Called by BeatManagerScript when this direction's key is released
    public void OnRelease()
    {
        if (noteType != NoteType.Hold || !isBeingHeld) return;

        isBeingHeld = false;
        // 80% leniency — releasing just before full duration still counts
        if (holdTime >= holdDuration * 0.8f)
            HoldSuccess();
        else
            OnMiss();
    }

    public void Initialize(float timeToClick, float timingWindow, Vector3 targetPosition,
        GameObject scoringHandler, NoteDirection noteDirection,
        NoteType noteType = NoteType.Standard, int hitsRequired = 1, float holdDuration = 0.5f)
    {
        this.timeToClick    = timeToClick;
        this.timingWindow   = timingWindow;
        this.targetPosition = targetPosition;
        this.noteDirection  = noteDirection;
        this.noteType       = noteType;
        this.hitsRequired   = hitsRequired;
        this.holdDuration   = holdDuration;

        startingPosition = transform.position;
        timeAlive        = 0;
        hitsLanded       = 0;
        multiHitStarted  = false;
        multiHitTimer    = 0f;
        holdTime         = 0f;
        isBeingHeld      = false;

        // Activate the correct visual, hide the others
        if (standardVisual) standardVisual.SetActive(false);
        if (multiHitVisual) multiHitVisual.SetActive(false);
        if (holdVisual)     holdVisual.SetActive(false);

        switch (noteType)
        {
            case NoteType.MultiHit: if (multiHitVisual) multiHitVisual.SetActive(true); break;
            case NoteType.Hold:
                if (holdVisual) holdVisual.SetActive(true);
                if (holdProgressBar) holdProgressBar.localScale = new Vector3(0f, 1f, 1f);
                break;
            default:
                if (standardVisual) standardVisual.SetActive(true);
                break;
        }
    }

    private void HoldSuccess()
    {
        Debug.Log("Hold Success");
        isBeingHeld = false;
        GetComponentInParent<BeatManagerScript>().BeatDeath(gameObject, BeatScore.Success, noteDirection);
    }

    private void OnMiss()
    {
        GetComponentInParent<BeatManagerScript>().BeatDeath(gameObject, BeatScore.Failure, noteDirection);
    }
} 