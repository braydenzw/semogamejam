using UnityEngine;
using UnityEngine.Audio;

public class TestAudioBreak : MonoBehaviour
{
    public AudioMixer mixer;
    public string pitchParam = "ViolinPitch";
    public string distWetParam = "ViolinDistWet";
    public string flangeWetParam = "ViolinFlangeWet";

    [Header("Broken Settings (Health = 0)")]
    public float brokenPitch = -0.5f;       // semitones
    public float brokenDistWet = 30f;       // %
    public float brokenFlangeWet = 20f;

    [Header("Curve Power (2 = gentle delay, 4 = very delayed)")]
    public float curvePower = 2.5f;

    [Range(0f, 1f)]
    public float health = 1f;

    [Header("Test Controls")]
    public KeyCode breakKey = KeyCode.Space;
    public KeyCode healKey = KeyCode.H;
    public float changeSpeed = 0.5f;        // slower

    void Update()
    {
        if (Input.GetKey(breakKey))
            health = Mathf.Clamp01(health - changeSpeed * Time.deltaTime);
        if (Input.GetKey(healKey))
            health = Mathf.Clamp01(health + changeSpeed * Time.deltaTime);

        float h = Mathf.Clamp01(health);
        float effectAmount = 1f - Mathf.Pow(h, curvePower);

        mixer.SetFloat(pitchParam, Mathf.Lerp(0f, brokenPitch, effectAmount));
        mixer.SetFloat(distWetParam, Mathf.Lerp(0f, brokenDistWet, effectAmount));
        mixer.SetFloat(flangeWetParam, Mathf.Lerp(0f, brokenFlangeWet, effectAmount));
    }

    void OnGUI()
    {
        GUILayout.Label($"Health: {health:F2}");
        GUILayout.Label($"Effect Amount: {1f - Mathf.Pow(health, curvePower):F2}");
        GUILayout.Label("Hold SPACE to break | Hold H to heal");
    }
}