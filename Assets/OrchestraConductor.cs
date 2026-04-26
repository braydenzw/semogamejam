using UnityEngine;

public class OrchestraConductor : MonoBehaviour
{
    [Header("All stems that need to stay in perfect sync")]
    public AudioSource[] allStems;   // drag every section's AudioSource here

    [Header("Conductor track (optional but recommended)")]
    public AudioSource conductor;    // a dedicated beat/pulse track

    void Start()
    {
        double startTime = AudioSettings.dspTime + 0.5; // small pre-delay

        if (conductor != null)
            conductor.PlayScheduled(startTime);

        foreach (var source in allStems)
        {
            if (source != null)
                source.PlayScheduled(startTime);
        }
    }
}