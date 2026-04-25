using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int health;
    // Start is called before the first frame update
    void Start()
    {
        health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Check for colliding objects
    void OnCollissionEnter(Collision collision)
    {
        health--;
        if(health<=0)
        {
            //death animation then switch scene to death menu
        }
    }
}
