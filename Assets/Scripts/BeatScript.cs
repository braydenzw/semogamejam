using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Enums;

public class BeatScript : MonoBehaviour
{
    private float timeToClick;
    private float timingWindow;
    private NoteDirection noteDirection;
    private Vector3 targetPosition;
    private Vector3 startingPosition;
    private double timeAlive;
    private GameObject scoringHandler;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;
        this.transform.position = this.transform.position + Time.deltaTime * (targetPosition-startingPosition) / timeToClick;
        if(timeAlive>=timeToClick+timingWindow)
        {
            OnMiss();
        }
    }

    private void OnMiss()
    {
        scoringHandler.GetComponent<BeatManagerScript>().BeatDeath(this.gameObject,BeatScore.Failure,noteDirection);
    }

    public void Initialize(float timeToClick, float timingWindow, Vector3 targetPosition, GameObject scoringHandler, NoteDirection noteDirection)
    {
        this.timeToClick = timeToClick;
        this.timingWindow = timingWindow;
        this.targetPosition = targetPosition;
        this.scoringHandler = scoringHandler;
        this.noteDirection = noteDirection;

        this.startingPosition = this.transform.position;
        this.timeAlive = 0; 
    }

    public void OnHit()
    {
        if(Math.Abs(timeAlive-timeToClick)<=timingWindow)
        {
            scoringHandler.GetComponent<BeatManagerScript>().BeatDeath(this.gameObject,BeatScore.Success,noteDirection);
        }
        else
        {
            OnMiss();
        }
    }
}
