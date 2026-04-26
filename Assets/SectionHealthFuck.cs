using UnityEngine;
using UnityEngine.Audio;

public class SectionHealthFuck : MonoBehaviour
{
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

    /// <summary>
    /// Call this to set the health from anywhere.
    /// 1 = perfectly fine, 0 = fully broken.
    /// </summary>
    public void SetHealth(float newHealth)
    {
        health = Mathf.Clamp01(newHealth);
    }

    void Update()
    {
        float effectAmount = 1f - Mathf.Pow(health, 3f);

        
        mixer.SetFloat(pitchParam, Mathf.Lerp(0f, brokenPitch, effectAmount));
        mixer.SetFloat(distWetParam, Mathf.Lerp(0f, brokenDistortionWet, effectAmount));
        mixer.SetFloat(flangeWetParam, Mathf.Lerp(0f, brokenFlangeWet, effectAmount));
    }
}