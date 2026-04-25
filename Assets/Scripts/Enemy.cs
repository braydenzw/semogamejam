using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health;

    public void Damage(int damage)
    {
        health -= damage;
    }
}
