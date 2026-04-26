using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FletcherPeakabooScript : MonoBehaviour
{
    // Start is called before the first frame update
    private float rotation_speed = 20;
    void Start()
    {
        transform.Rotate(new Vector3(60,0,0),Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(rotation_speed*Time.deltaTime,0,0),Space.Self);
    }

    void OnEnable()
    {
        rotation_speed = 20;
    }
}
