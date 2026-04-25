using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    public Transform target; // The target to point to
    public Transform arrow;  // The arrow indicator

    private Vector3 positionVector = new Vector3(0, 1, 0);
    public Transform player;

    void Update()
    {
        arrow.position = player.position + positionVector;
        if(target != null && arrow != null)
        {
            // Calculate the direction
            Vector3 direction = target.position - Camera.main.transform.position;
            direction.z = 0; // Keep the arrow on the screen plane

            // Calculate the angle
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Rotate the arrow
            arrow.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
