using UnityEngine;
using UnityEngine.Audio;

public class SectionHealth : MonoBehaviour
{
    [Header("Mixer & Parameters (hard‑coded for safety)")]
    public AudioMixer mixer; // drag OrchestraMixer here

    // These strings MUST match your exposed parameter names exactly
    private const string pitchParam   = "ViolinPitch";
    private const string distWetParam = "ViolinDistWet";
    private const string flangeWetParam = "ViolinFlangeWet";

    [Header("Broken Effect Amounts (Health = 0)")]
    [SerializeField] private float brokenPitch = -2.5f;    // semitones
    [SerializeField] private float brokenDistortionWet = 40f;
    [SerializeField] private float brokenFlangeWet = 40f;

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
        // Calculate effect blend (1 at health=0, 0 at health=1)
        float effectAmount = 1f - health;

        // Apply to mixer – using hard‑coded names to avoid typos
        mixer.SetFloat(pitchParam, Mathf.Lerp(0f, brokenPitch, effectAmount));
        mixer.SetFloat(distWetParam, Mathf.Lerp(0f, brokenDistortionWet, effectAmount));
        mixer.SetFloat(flangeWetParam, Mathf.Lerp(0f, brokenFlangeWet, effectAmount));
    }
}