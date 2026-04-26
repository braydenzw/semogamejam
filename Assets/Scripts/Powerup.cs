using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using static Enums;

public class Powerup : MonoBehaviour
{
    private PowerupType powerupType;
    private float lifetime = 15f;

    private void Start()
    {
        // choose a random powerup value on spawn
        Array values = Enum.GetValues(typeof(PowerupType));
        int ran = UnityEngine.Random.Range(0, values.Length);
        powerupType = (PowerupType)values.GetValue(ran);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (powerupType)
            {
                case PowerupType.DoubleDamage:
                    ComboManager.instance.multiplier = 2;
                    break;
                default:
                    Debug.Log("no powerup");
                    break;
            }

            StartCoroutine(PowerupReset());

            Destroy(gameObject);
        }
    }

    IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    IEnumerator PowerupReset()
    {
        yield return new WaitForSeconds(5f);
        ComboManager.instance.multiplier = 1;
        Debug.Log("powerup wore off");
    }
}
