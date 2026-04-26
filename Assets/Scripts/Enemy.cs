using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health;
    private SpriteRenderer spriteRenderer;
    private string neutralColor = "#FFFFFF"; // the color of the enemy at hurt (not getting hurt) 
    [SerializeField] HurtEffectSpawner hurtEffectSpawner;
    [SerializeField] TMP_Text healthText;
    [SerializeField] BeatManagerScript beatManager;
    [SerializeField] SongMap whippingMyLash;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        beatManager.InitiateSong(whippingMyLash);
        
        //hurtEffectSpawner = GetComponent<HurtEffectSpawner>();
    }

    private void Update()
    {
        if(health>0)
            healthText.text = "Fletcher Health: " + health;
        else
        {
            healthText.text = "Fletcher has been defeated... glory to the band of us";
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
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
