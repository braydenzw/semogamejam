using UnityEngine;

public class RhythmRecorder : MonoBehaviour
{
    [Header("settings")]
    public SongMap songMap;
    public AudioSource audioSource;
    public float visualSpacing = 10f;
    [HideInInspector] public int selectedNoteIndex = -1;

    [Header("bpm sync")]
    public bool useQuantization = true;

    [Header("controls")]
    [Tooltip("press to start the song and begin mapping")]
    public KeyCode startKey = KeyCode.Return; // Enter Key

    [Tooltip("tap or hold spacebar to map: direction randomized")]
    public KeyCode mapKey = KeyCode.Space;

    private double songStartDspTime;
    private bool isRecording = false;

    private float currentHoldStartTime;
    private bool isHolding = false;

    void Start()
    {
        Debug.Log($"press {startKey} to start mapping");
    }

    void Update()
    {
        // Start the process
        if (Input.GetKeyDown(startKey) && !isRecording)
        {
            StartMapping();
        }

        if (!isRecording) return;

        float currentSongTime = (float)(AudioSettings.dspTime - songStartDspTime);

        if (Input.GetKeyDown(mapKey))
        {
            currentHoldStartTime = currentSongTime;
            isHolding = true;
        }

        if (Input.GetKeyUp(mapKey) && isHolding)
        {
            float duration = currentSongTime - currentHoldStartTime;
            SaveBeat(currentHoldStartTime, duration);
            isHolding = false;
        }

        if (!audioSource.isPlaying && isRecording)
        {
            isRecording = false;
            Debug.Log("mapping finished");
        }
    }

    void StartMapping()
    {
        songMap.beats.Clear();
        songStartDspTime = AudioSettings.dspTime;
        audioSource.Play();
        isRecording = true;
        Debug.Log($"mapping started: tap/hold '{mapKey}' to the beat.");
    }

    void SaveBeat(float startTime, float duration)
    {
        // grid snap stuff
        if (useQuantization && songMap.songBpm > 0)
        {
            float beatInterval = 60f / (songMap.songBpm * ((float)songMap.notesPerMeasure / songMap.noteDivision));
            float rawEndTime = startTime + duration;

            float snappedStart = Mathf.Round(startTime / beatInterval) * beatInterval;
            float snappedEnd = Mathf.Round(rawEndTime / beatInterval) * beatInterval;

            startTime = snappedStart;
            duration = snappedEnd - snappedStart;
        }

        float noteDuration = (duration < 0.15f) ? 0f : duration;

        // random direction maker
        NoteDirection randomDir = (NoteDirection)Random.Range(0, 4);

        BeatData newBeat = new BeatData
        {
            timestamp = startTime,
            duration = noteDuration,
            noteDirection = randomDir
        };

        songMap.beats.Add(newBeat);
    }
}