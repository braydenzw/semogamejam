using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("enemy health: " + health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
