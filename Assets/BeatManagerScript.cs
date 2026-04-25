using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManagerScript : MonoBehaviour
{
    // TEST VARIABLES
    private double timeInterval = 1;
    private double time = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time+=Time.deltaTime;
        if(time<=timeInterval)
        {
            
        }
    }

    private void SpawnObject()
    {
        
    }
}
