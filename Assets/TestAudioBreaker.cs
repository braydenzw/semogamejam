using UnityEngine;
using UnityEngine.Audio;

public class TestAudioBreaker : MonoBehaviour
{
    [Header("Audio Mixer & Exposed Parameters")]
    public AudioMixer mixer;                         // Drag OrchestraMixer here
    public string pitchParam = "ViolinPitch";        // <-- keep exactly like this
    public string distWetParam = "ViolinDistWet";
    public string flangeWetParam = "ViolinFlangeWet";

    [Header("Effect Amounts at Health = 0 (Fully Broken)")]
    public float brokenPitch = -2.5f;                // semitones, don't go below -12
    public float brokenDistWet = 40f;                // 0-100%
    public float brokenFlangeWet = 40f;              // 0-100%

    [Header("Current Health")]
    [Range(0f, 1f)]
    public float health = 1f;

    [Header("Controls")]
    public KeyCode breakKey = KeyCode.Space;
    public KeyCode healKey = KeyCode.H;
    public float changeSpeed = 0.5f;                 // health per second

    void Update()
    {
        // ---- Manual health adjustment ----
        if (Input.GetKey(breakKey))
            health = Mathf.Clamp01(health - changeSpeed * Time.deltaTime);
        if (Input.GetKey(healKey))
            health = Mathf.Clamp01(health + changeSpeed * Time.deltaTime);

        // ---- Apply to mixer ----
        float h = Mathf.Clamp01(health);

        // Linear blend: 0 effect at health=1, full effect at health=0
        float effectAmount = 1f - h;

        float pitchVal = Mathf.Lerp(0f, brokenPitch, effectAmount);
        float distVal  = Mathf.Lerp(0f, brokenDistWet, effectAmount);
        float flangeVal = Mathf.Lerp(0f, brokenFlangeWet, effectAmount);

        mixer.SetFloat("ViolinPitch", pitchVal);
        mixer.SetFloat("ViolinDistWet", distVal);
        mixer.SetFloat("ViolinFlangeWet", flangeVal);

        // Debug: shows values in Console to confirm script is working
        Debug.Log($"Health: {h:F2} | Effect: {effectAmount:F2} | Pitch: {pitchVal:F2} | Dist: {distVal:F1} | Flange: {flangeVal:F1}");
    }

    void OnGUI()
    {
        GUILayout.Label($"Health: {health:F2}");
        GUILayout.Label($"Effect Amount: {1f - health:F2}");
        GUILayout.Label("Hold SPACE to break | Hold H to heal");
    }
}