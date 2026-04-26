using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

// handles combos and attacking the enemy
// call ComboManager.instance to use these functions/variables
public class ComboManager : MonoBehaviour
{
    public static ComboManager instance;

    // the no. of successful notes hit in a streak
    private int combo;
    // the player's damage value for each note hit,
    // which increases as they grow their combo
    private int damage = 0;
    public int multiplier = 1;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip failure;
    [SerializeField] TMP_Text comboText;
    [SerializeField] GameObject currentEnemy;
    private Enemy currentEnemyScript;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        UpdateEnemy();
    }

    private void UpdateEnemy()
    {
        currentEnemyScript = currentEnemy.GetComponent<Enemy>();
    }

    // on hit, extend combo
    public void ExtendCombo()
    {
        combo++;
        Debug.Log("Extendcombo");
        SoundManager.instance.PlaySound(success, transform, 0.5f);
        comboText.text = "Combo: " + combo;
        CalculateDamage();
        DoDamage();
    }

    // on miss, reset combo to 0
    public void EndCombo()
    {
        combo = 0;
        SoundManager.instance.PlaySound(failure, transform, 0.5f);
        comboText.text = "Combo: " + combo;
        CalculateDamage();
    }

    private void CalculateDamage()
    {
        if (combo <= 10)
        {
            comboText.color = Color.white;
            damage = 1;
        }
        else if (combo <= 20)
        {
            comboText.color = Color.yellow;
            damage = 2;
        } else if (combo <= 30)
        {
            comboText.color = Color.yellow;
            damage = 3;
        } else if (combo <= 40)
        {
            comboText.color = Color.red;
            damage = 4;
        } else if (combo <= 50)
        {
            comboText.color = Color.red;
            damage = 5;
        }
    }

    private void DoDamage()
    {
        if (currentEnemyScript != null)
        {
            currentEnemyScript.TakeDamage(damage * multiplier);
        } else
        {
            Debug.Log("CURRENT ENEMY UNASSIGNED IN COMBOMANAGER");
        }
    }
}
