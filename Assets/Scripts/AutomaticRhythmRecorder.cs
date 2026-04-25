using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
// this one will automap every note for you BUT you must manually map suspended notesssss
public class AutomaticRhythmRecorder : MonoBehaviour
{
    [Header("settings")]
    public SongMap songMap;
    public AudioSource audioSource;

    [Header("mapping visualizer")]
    public float visualSpacing = 10f;

    [Header("detection stuff")]
    [Tooltip("beat sensitivity: lower = more detection, higher = less detection")]
    [Range(0.1f, 100f)]
    public float threshold = 0.1f; // i found that generally 0.1 works the best

    [Tooltip("minimum seconds between notes. this is just to prevent two duplicate notes")]
    public float minTimeBetweenNotes = 0.15f;

    private float[] spectrum = new float[1024];
    private float lastNoteTime;
    private float previousEnergy;
    private bool isRecording = false;
    private double startTime;

    void Start()
    {
        Debug.Log("click spacebar to start mapping");
    }
    void Update()
    {
        // Press Space to start the analysis pass
        if (Input.GetKeyDown(KeyCode.Space) && !isRecording)
        {
            StartRecording();
        }

        if (!isRecording) return;

        // automatically stops when the song ends
        if (!audioSource.isPlaying)
        {
            FinishRecording();
            return;
        }

        RecordingFrame();
    }

    void StartRecording()
    {
        songMap.beats.Clear(); // Clear old data
        startTime = AudioSettings.dspTime;
        audioSource.Play();
        isRecording = true;
        Debug.Log("recording started");
    }

    void RecordingFrame()
    {
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        float currentEnergy = 0;
        foreach (float sample in spectrum)
        {
            currentEnergy += sample;
        }

        float flux = currentEnergy - previousEnergy;
        float currentTime = (float)(AudioSettings.dspTime - startTime);

        // just more stuff to prevent doubled up notes
        if (flux > threshold && (currentTime - lastNoteTime) > minTimeBetweenNotes)
        {
            RecordBeat(currentTime);
            lastNoteTime = currentTime;
        }

        previousEnergy = currentEnergy;
    }

    void RecordBeat(float time)
    {
        // basically just 0-3 random integer to determine a random direction for mapping
        NoteDirection randomDirection = (NoteDirection)Random.Range(0, 4);

        BeatData newBeat = new BeatData
        {
            timestamp = time,
            duration = 0f,
            noteDirection = randomDirection
        };

        songMap.beats.Add(newBeat);
    }

    void FinishRecording()
    {
        isRecording = false;
        Debug.Log($"recording stopped: mapped {songMap.beats.Count} notes");
    }

    private void OnDrawGizmos() // this will actually display the beats as colored blocks rather than just a numbered list
    {
        if (songMap == null || songMap.beats == null)
        {
            return;
        }

        foreach (var beat in songMap.beats)
        {
            // the positions represent note direction on x-axis and time on z-axis
            Vector3 startPos = new Vector3((int)beat.noteDirection * 2f, 0, beat.timestamp * visualSpacing);

            // color is set depending on direction
            Gizmos.color = VisualMappingColor(beat.noteDirection);

            if (beat.duration > 0)
            {
                // this draws a line connecting the start and end of a hold note
                Vector3 endPos = startPos + new Vector3(0, 0, beat.duration * visualSpacing);
                Gizmos.DrawLine(startPos, endPos);
                Gizmos.DrawWireCube(endPos, Vector3.one * 0.5f);
            }

            Gizmos.DrawCube(startPos, Vector3.one * 0.7f);
        }
    }

    Color VisualMappingColor(NoteDirection dir)
    {
        switch (dir)
        {
            case NoteDirection.left: return Color.red;
            case NoteDirection.down: return Color.blue;
            case NoteDirection.up: return Color.green;
            case NoteDirection.right: return Color.yellow;
            default: return Color.white;
        }
    }
}