using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Enums;
using TMPro;

/*TODO GENERAL:

REMEMBER DIRECTIONAL QUEUE

*/
public class BeatManagerScript : MonoBehaviour
{
    //// TEST VARIABLES
    [SerializeField] float timeInterval;
    private float time = 0;
    private int count = 0;

    
    [SerializeField] float timeToClick;
    [SerializeField] float timingWindow;
    [SerializeField] float spawnDistanceMultiplier;
    [SerializeField] float targetPositionMultiplier;

    [SerializeField] GameObject HorizontalLine;
    [SerializeField] GameObject VerticalLine;

    private Stack<GameObject> inactiveBeatsStack; // Handles object pooling

    // QUEUES
    private Queue<GameObject> leftQueue; // left object queue
    private Queue<GameObject> rightQueue; // right object queue
    private Queue<GameObject> upQueue; // upward object queue
    private Queue<GameObject> downQueue; // down object queue

    public GameObject beatPrefab;
    public GameObject player;
    public TMP_Text scoreText;

    //bool so code can stop and start
    public bool onOrOff = false;
    public float timeToPlay;





    // Start is called before the first frame update
    void Start()
    {
        inactiveBeatsStack = new Stack<GameObject>();

        leftQueue = new Queue<GameObject>();
        rightQueue = new Queue<GameObject>();
        upQueue = new Queue<GameObject>();
        downQueue = new Queue<GameObject>();

        // Evil 3AM Code
        foreach (NoteDirection noteDirection in Enum.GetValues(typeof(NoteDirection)))
        {
            GameObject lineToSpawn;
            if(noteDirection==NoteDirection.left || noteDirection==NoteDirection.right) lineToSpawn = VerticalLine;
            else lineToSpawn = HorizontalLine;
            Instantiate(lineToSpawn,this.transform.position + GetDirectionVector(noteDirection)*targetPositionMultiplier,this.transform.rotation,this.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(onOrOff)
        {
            time += Time.deltaTime;
            timeToPlay -= Time.deltaTime;
            if (time >= timeInterval)
            {
                if (count % 4 == 0) SpawnObject(NoteDirection.up);
                if (count % 4 == 1) SpawnObject(NoteDirection.down);
                if (count % 4 == 2) SpawnObject(NoteDirection.left);
                if (count % 4 == 3) SpawnObject(NoteDirection.right);
                time -= timeInterval;
                count++;
            }
            //keeping track of time left to fix this section
            if(timeToPlay <= 0)
            {
                onOrOff = false;
                player.GetComponent<Player>().maxVelocity = 5;
                player.GetComponent<Player>().nextSection();
            }
        }
    }

    private GameObject SpawnObject(NoteDirection noteDirection)
    {
        GameObject newBeat;
        Queue<GameObject> currentQueue = GetQueue(noteDirection);
        if (inactiveBeatsStack.Count > 0)
        {
            newBeat = inactiveBeatsStack.Pop();
            newBeat.transform.position = this.transform.position + GetDirectionVector(noteDirection) * spawnDistanceMultiplier;
        }
        else
        {
            newBeat = Instantiate(beatPrefab, this.transform.position + GetDirectionVector(noteDirection) * spawnDistanceMultiplier, this.transform.rotation, this.transform);

        }
        currentQueue.Enqueue(newBeat);
        newBeat.GetComponent<BeatScript>().Initialize(
            1, 0.1f, this.transform.position + GetDirectionVector(noteDirection) * targetPositionMultiplier, this.gameObject, noteDirection);
        newBeat.SetActive(true);
        return newBeat;
    }

    public void OnTap(NoteDirection noteDirection)
    {
        Queue<GameObject> currentQueue = GetQueue(noteDirection);
        GameObject objectHit = currentQueue.Peek();
        if (objectHit != null)
        {
            objectHit.GetComponent<BeatScript>().OnHit();
        }
    }

    private Queue<GameObject> GetQueue(NoteDirection noteDirection)
    {
        switch (noteDirection)
        {
            case NoteDirection.left:
                return leftQueue;
            case NoteDirection.right:
                return rightQueue;
            case NoteDirection.up:
                return downQueue;
            case NoteDirection.down:
                return upQueue;
        }
        return null;
    }

    private Vector3 GetDirectionVector(NoteDirection noteDirection)
    {
        switch (noteDirection)
        {
            case NoteDirection.left:
                return Vector3.left;
            case NoteDirection.right:
                return Vector3.right;
            case NoteDirection.up:
                return Vector3.up;
            case NoteDirection.down:
                return Vector3.down;
        }
        return Vector3.zero;
    }

    public void BeatDeath(GameObject toDie, BeatScore beatScore, NoteDirection noteDirection)
    {
        // Handle Death
        toDie.SetActive(false);
        inactiveBeatsStack.Push(toDie);

        Queue<GameObject> currentQueue = GetQueue(noteDirection);
        currentQueue.Dequeue();

        // Handle Scoring
        if (beatScore == BeatScore.Failure)
        {
           // Debug.Log("TEMP: FAILURE");
            player.GetComponent<Health>().Damage();
        }
        else if (beatScore == BeatScore.Success)
        {
           // Debug.Log("TEMP: SUCCESS");
            player.GetComponent<PlayerScript>().score++;
            scoreText.text = "Score: " + player.GetComponent<PlayerScript>().score;
        }
    }
}
