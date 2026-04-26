using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SectionHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int sectionHealth;
    private float timeToDecrease;
    public bool inUse;
    private bool isActive; // is the decrementer currently running, can we change the sectionHealth
    public TMP_Text healthText;
    [SerializeField] SongMap sectionSong;

    [Header("Mixer & Parameters (hard‑coded for safety)")]
    public AudioMixer mixer; // drag OrchestraMixer here

    // These strings MUST match your exposed parameter names exactly
    [SerializeField] private string pitchParam;
    [SerializeField] private string distWetParam;
    [SerializeField] private string flangeWetParam;

    [Header("Broken Effect Amounts (Health = 0)")]
    private float brokenPitch = 8f;    // semitones
    private float brokenDistortionWet = 1f;
    private float brokenFlangeWet = 1f;

    [Header("Current Health")]
    [Range(0f, 1f)]
    [SerializeField] private float health = 1f;   // start each section fully fixed
    public float Health => health;                // other scripts can read it

    void Start()
    {
        sectionHealth = 0;
        timeToDecrease = (float)0.9;
        inUse = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            timeToDecrease -= Time.deltaTime;
            if (timeToDecrease <= 0)
            {
                ChangeHealth(Random.Range(0, 3));
                timeToDecrease = (float)0.9;
            }

            healthText.text = "Health: " + sectionHealth;

            float effectAmount = 1f - Mathf.Pow(health, 3f);

            mixer.SetFloat(pitchParam, Mathf.Lerp(0f, brokenPitch, effectAmount));
            mixer.SetFloat(distWetParam, Mathf.Lerp(0f, brokenDistortionWet, effectAmount));
            mixer.SetFloat(flangeWetParam, Mathf.Lerp(0f, brokenFlangeWet, effectAmount));
        }
    }

    public void ChangeHealth(int delta)
    {
        sectionHealth += delta;
        healthText.text = "Health: " + sectionHealth;
        if (sectionHealth > 100)
        {
            sectionHealth = 100;
            isActive = false;
            healthText.color = Color.yellow;
        } 

        if (sectionHealth < 0)
        {
            sectionHealth = 0;
        }
    }

    public SongMap GetSong()
    {
        return sectionSong;
    }
}
