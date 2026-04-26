using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SectionHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int sectionHealth;
    private float timeToDecrease;
    public bool inUse;
    public TMP_Text healthText;
    void Start()
    {
        sectionHealth = 100;
        timeToDecrease = (float)0.5;
        inUse = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!inUse)
        {
            timeToDecrease -= Time.deltaTime;
            if(timeToDecrease <= 0)
            {
                sectionHealth -= Random.Range(0,11);
                timeToDecrease = (float)0.5;
            }
        }
        healthText.text = "Health: " + sectionHealth;
    }
}
