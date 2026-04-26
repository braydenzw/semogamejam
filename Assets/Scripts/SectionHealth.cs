using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SectionHealth : MonoBehaviour
{
    public int sectionHealth;
    private float timeToDecrease;
    public bool inUse;
    private bool isActive;
    public TMP_Text healthText;
    [SerializeField] SongMap sectionSong;

    [Header("Section Identity")]
    public SectionType sectionType;  // set this in the Inspector for each section GameObject

    [Header("Mixer & Parameters")]
    public AudioMixer mixer;
    [SerializeField] private string pitchParam;
    [SerializeField] private string distWetParam;
    [SerializeField] private string flangeWetParam;

    [Header("Broken Effect Amounts (Health = 0)")]
    private float brokenPitch = 8f;
    private float brokenDistortionWet = 1f;
    private float brokenFlangeWet = 1f;

    [Header("Health Decay")]
    [SerializeField] private float decayIntervalMin = 0.5f;
    [SerializeField] private float decayIntervalMax = 4.0f;
    [SerializeField] private int decayMin = 1;
    [SerializeField] private int decayMax = 6;
    [Tooltip("1 in X chance of a catastrophic health drop each tick. 100 = 1% chance.")]
    [SerializeField] private int catastropheOdds = 100;
    [Tooltip("How much health is lost in a catastrophic drop.")]
    [SerializeField] private int catastropheDamage = 50;

    [Header("Effect Curve")]
    [Tooltip("Higher = effects stay subtle longer, only peaking at very low health. Try 5-10.")]
    [SerializeField] private float effectCurveExponent = 6f;
    [Tooltip("How slowly the audio effects blend in/out. Lower = slower/smoother. 0.02 = very gradual.")]
    [SerializeField] private float effectSmoothSpeed = 0.02f;

    private float smoothedEffectAmount = 0f;

    void Start()
    {
        sectionHealth = 100;
        isActive = true;
        inUse = false;
        timeToDecrease = Random.Range(decayIntervalMin, decayIntervalMax);
        smoothedEffectAmount = 0f;

        ResetMixer();
        healthText.text = "Health: " + sectionHealth;
    }

    void Update()
    {
        if (!isActive) return;

        timeToDecrease -= Time.deltaTime;
        if (timeToDecrease <= 0)
        {
            if (!inUse && Random.Range(0, catastropheOdds) == 0)
                ChangeHealth(-catastropheDamage);
            else
                ChangeHealth(-Random.Range(decayMin, decayMax));

            timeToDecrease = Random.Range(decayIntervalMin, decayIntervalMax);
        }

        healthText.text = "Health: " + sectionHealth;

        float damage01 = 1f - (sectionHealth / 100f);
        float targetEffectAmount = Mathf.Pow(damage01, effectCurveExponent);

        smoothedEffectAmount = Mathf.Lerp(smoothedEffectAmount, targetEffectAmount, effectSmoothSpeed * Time.deltaTime);

        mixer.SetFloat(pitchParam,     Mathf.Lerp(0f, brokenPitch,         smoothedEffectAmount));
        mixer.SetFloat(distWetParam,   Mathf.Lerp(0f, brokenDistortionWet, smoothedEffectAmount));
        mixer.SetFloat(flangeWetParam, Mathf.Lerp(0f, brokenFlangeWet,     smoothedEffectAmount));
    }

    public void ChangeHealth(int delta)
    {
        Debug.Log($"ChangeHealth delta: {delta} | Current health: {sectionHealth}");        sectionHealth += delta;
        sectionHealth = Mathf.Clamp(sectionHealth, 0, 100);

        healthText.text = "Health: " + sectionHealth;

        if (sectionHealth >= 100)
        {
            isActive = false;
            healthText.color = Color.yellow;
            ResetMixer();
        }
        else
        {
            isActive = true;
            healthText.color = Color.white;
        }
    }

    private void ResetMixer()
    {
        if (mixer == null) return;
        smoothedEffectAmount = 0f;
        mixer.SetFloat(pitchParam,     0f);
        mixer.SetFloat(distWetParam,   0f);
        mixer.SetFloat(flangeWetParam, 0f);
    }

    public SongMap GetSong()
    {
        return sectionSong;
    }
}