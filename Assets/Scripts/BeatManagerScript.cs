using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Enums;
using TMPro;

/*TODO GENERAL:

REMEMBER DIRECTIONAL QUEUE

*/
public class BeatManagerScript : MonoBehaviour
{
    // TEST VARIABLES
    private double timeInterval = 1;
    private double time = 0;
    private Stack<GameObject> inactiveBeatsStack; // Handles object pooling
    private Queue<GameObject> upQueue; // upward object queue
    public GameObject beatPrefab;
    public GameObject player;
    public TMP_Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        inactiveBeatsStack = new Stack<GameObject>();
        upQueue = new Queue<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        time+=Time.deltaTime;
        if(time>=timeInterval)
        {
            SpawnObject();
            time-=timeInterval;
        }
        if(Input.anyKeyDown)
        {
            OnTap();
        }
    }

    private GameObject SpawnObject()
    {
        GameObject newBeat;
        if(inactiveBeatsStack.Count>0) 
        {
            newBeat = inactiveBeatsStack.Pop();
            newBeat.transform.position = this.transform.position + new Vector3(1,0,0);
        }
        else
        {
            newBeat = Instantiate(beatPrefab,this.transform.position + new Vector3(1,0,0),this.transform.rotation);
        }
        upQueue.Enqueue(newBeat);
        newBeat.GetComponent<BeatScript>().Initialize(1,0.1f,this.transform.position,this.gameObject); // TODO: CHANGE POSITION AND GIVE DIRECTION
        newBeat.SetActive(true);
        return newBeat;
    }

    private void OnTap()
    {
        GameObject objectHit = upQueue.Peek();
        objectHit.GetComponent<BeatScript>().OnHit();
    }

    public void BeatDeath(GameObject toDie, BeatScore beatScore)
    {
        // Handle Death
        toDie.SetActive(false);
        inactiveBeatsStack.Push(toDie);

        //TEMP:
        upQueue.Dequeue();
        
        // Handle Scoring
        if(beatScore == BeatScore.Failure)
        {
            Debug.Log("TEMP: FAILURE");
            player.GetComponent<Health>().Damage();
        }
        else if(beatScore == BeatScore.Success)
        {
            Debug.Log("TEMP: SUCCESS");
            player.GetComponent<PlayerScript>().score++;
            scoreText.text = "Score: " + player.GetComponent<PlayerScript>().score;
        }
    }
}
