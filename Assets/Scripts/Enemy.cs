using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health;
    private SpriteRenderer spriteRenderer;
    private string neutralColor = "#FFFFFF"; // the color of the enemy at hurt (not getting hurt) 
    [SerializeField] HurtEffectSpawner hurtEffectSpawner;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //hurtEffectSpawner = GetComponent<HurtEffectSpawner>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("enemy health: " + health);

        Debug.Log("spawned effect");
        hurtEffectSpawner.SpawnEffect();

        if (spriteRenderer != null)
        {
            StartCoroutine(HurtEffect());
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator HurtEffect()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        if (ColorUtility.TryParseHtmlString(neutralColor, out Color color))
        {
            spriteRenderer.color = color;
        }
    }
}
