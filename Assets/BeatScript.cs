using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BeatScript : MonoBehaviour
{
    private float timeToClick;
    private float timingWindow;
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

    public void Initialize(float timeToClick, float timingWindow, Vector3 targetPosition, GameObject scoringHandler)
    {
        this.timeToClick = timeToClick;
        this.timingWindow = timingWindow;
        this.targetPosition = targetPosition;
        this.scoringHandler = scoringHandler;

        this.startingPosition = this.transform.position;
        this.timeAlive = 0;
        
    }

    private void OnMiss()
    {
        
    }

    private void OnHit()
    {
        if(Math.Abs(timeAlive-timeToClick)<=timingWindow)
        {
            // SUCCESS
            // Perhaps grading ?
            
        }
        else
        {
            OnMiss();
        }
    }

    public void Clicked()
    {
        OnHit();
    }
}
