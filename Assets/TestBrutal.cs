using UnityEngine;
using UnityEngine.Audio;

public class TestBrutal : MonoBehaviour
{
    public AudioMixer mixer;
    public string pitchParam = "ViolinPitch";
    public string distWetParam = "ViolinDistWet";
    public string flangeWetParam = "ViolinFlangeWet";

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            mixer.SetFloat(pitchParam, -10f);       // obviously out of tune
            mixer.SetFloat(distWetParam, 60f);     // crunchy
            mixer.SetFloat(flangeWetParam, 60f);   // metallic
            Debug.Log("BRUTAL ON");
        }
        if (Input.GetKey(KeyCode.H))
        {
            mixer.SetFloat(pitchParam, 0f);
            mixer.SetFloat(distWetParam, 0f);
            mixer.SetFloat(flangeWetParam, 0f);
            Debug.Log("Healed");
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Press SPACE for brutal wack, H for heal");
    }
}