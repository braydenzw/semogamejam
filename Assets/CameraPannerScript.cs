using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPannerScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject player;
    [SerializeField] private float panMultiplier;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 difference = player.transform.position - this.transform.position;
        difference.z = -100;
        camera.transform.localPosition = difference*panMultiplier;
    }
}
