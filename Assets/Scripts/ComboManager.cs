using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using TMPro;

// call ComboManager.instance to use these functions/variables
public class ComboManager : MonoBehaviour
{
    public static ComboManager instance;

    // the no. of successful notes hit in a streak
    private int combo;
    // the player's damage value for each note hit,
    // which increases as they grow their combo
    private int damage = 0;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip failure;
    [SerializeField] TMP_Text comboText;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // on hit, extend combo
    public void ExtendCombo()
    {
        combo++;
        SoundManager.instance.PlaySound(success, transform, 1f);
        comboText.text = "Combo: " + combo;
        CalculateDamage();
    }

    // on miss, reset combo to 0
    public void EndCombo()
    {
        combo = 0;
        SoundManager.instance.PlaySound(failure, transform, 1f);
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
}
