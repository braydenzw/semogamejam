using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    [SerializeField] GameObject powerup;
    private float spawnInterval = 5f;
    private float timer;
    private float radius = 20f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            Vector2 point = (Random.insideUnitCircle * radius) + (Vector2)transform.position;
            Instantiate(powerup, point, Quaternion.identity);
            timer = 0f;
        }
    }
}
