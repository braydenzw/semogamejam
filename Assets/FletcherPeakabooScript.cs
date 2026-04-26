using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FletcherPeakabooScript : MonoBehaviour
{
    // Start is called before the first frame update
    private float rotation_speed = 20;
    float time = 0;
    void Start()
    {
        transform.Rotate(new Vector3(120, 0, 0), Space.Self);
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= 60 * 3 + 42)
        {
            transform.Rotate(new Vector3(rotation_speed * Time.deltaTime, 0, 0), Space.Self);
            GetComponent<SpriteRenderer>().enabled = true;
        }


    }
}
