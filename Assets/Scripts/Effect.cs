using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Effect : MonoBehaviour
{
    private Vector2 target;
    private float time;
    private float destroyTime = 3f;
    private float speed = 10f;

    private void Start()
    {
        target = (Vector2)(transform.position) + (Random.insideUnitCircle.normalized * 100f);
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        time += Time.deltaTime;
        if (time >= destroyTime)
        {
            Destroy(gameObject);
            time = 0f;
        }
    }
}
