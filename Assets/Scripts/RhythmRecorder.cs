using UnityEngine;

public class RhythmRecorder : MonoBehaviour
{
    [Header("Settings")]
    public SongMap songMap;
    public AudioSource audioSource;

    [Header("Controls")]
    public KeyCode leftKey = KeyCode.A;
    public KeyCode downKey = KeyCode.S;
    public KeyCode upKey = KeyCode.W;
    public KeyCode rightKey = KeyCode.D;

    private KeyCode[] keyArray;
    private double songStartDspTime;
    private bool isRecording = false;

    private float[] startTimes = new float[4];
    private bool[] isHolding = new bool[4];

    void Start()
    {
        keyArray = new KeyCode[] { leftKey, downKey, upKey, rightKey };
    }

    void Update()
    {
        // you can start and reset recording with spacebar
        if (Input.GetKeyDown(KeyCode.Space) && !isRecording)
        {
            StartRecording();
        }

        if (!isRecording) return;

        float currentSongTime = (float)(AudioSettings.dspTime - songStartDspTime);

        for (int i = 0; i < keyArray.Length; i++)
        {
            // this records when WASD is pressed
            if (Input.GetKeyDown(keyArray[i]))
            {
                startTimes[i] = currentSongTime;
                isHolding[i] = true;
            }

            // this records when WASD is released and calculates hold time
            if (Input.GetKeyUp(keyArray[i]) && isHolding[i])
            {
                float duration = currentSongTime - startTimes[i];
                SaveNote((NoteDirection)i, startTimes[i], duration);
                isHolding[i] = false;
            }
        }

        // auto-stops song once finished
        if (!audioSource.isPlaying && isRecording)
        {
            isRecording = false;
            Debug.Log("mapping finished");
        }
    }

    void StartRecording()
    {
        songMap.beats.Clear();
        songStartDspTime = AudioSettings.dspTime;
        audioSource.Play();
        isRecording = true;
        Debug.Log("mapping started. use WASD (you can hold for long notes)");
    }

    void SaveNote(NoteDirection direction, float startTime, float duration)
    {
        // i just added this safeguard so it doesnt count short taps as like nearly impossible to hit
        float noteDuration = (duration < 0.15f) ? 0f : duration;

        BeatData newNote = new BeatData();
        newNote.timestamp = startTime;
        newNote.duration = noteDuration;
        newNote.noteDirection = direction;

        songMap.beats.Add(newNote);

        string noteType = noteDuration > 0 ? "hold" : "tap";
        Debug.Log($"recorded {noteType}, direction: {direction}, time: {startTime:F2}s, length: {noteDuration:F2}s");
    }
}