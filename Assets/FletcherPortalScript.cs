using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FletcherPortalScript : MonoBehaviour
{
    // Start is called before the first frame update
    private float time;
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (time >= 3 * 60 + 42)
        {
            if (collision.tag == "Player")
            {
                SceneManager.LoadScene("FletcherBossFight");
            }
        }
    }
}
