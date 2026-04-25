using UnityEngine;
// DEFUNCT NOW OR SMTH IDK LOL
public class RhythmRecorder : MonoBehaviour
{
    [Header("settings")]
    public SongMap songMap;
    public AudioSource audioSource;

    [Header("bpm syncing stuff")]
    public bool useQuantization = true;

    [Header("mapping visualizer")]
    public float visualSpacing = 10f;

    [Header("controls")]
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
        if (Input.GetKeyDown(KeyCode.Space) && !isRecording)
        {
            StartMapping();
        }

        if (!isRecording) return;

        float currentSongTime = (float)(AudioSettings.dspTime - songStartDspTime);

        for (int i = 0; i < keyArray.Length; i++)
        {
            if (Input.GetKeyDown(keyArray[i]))
            {
                startTimes[i] = currentSongTime;
                isHolding[i] = true;
            }

            if (Input.GetKeyUp(keyArray[i]) && isHolding[i])
            {
                float duration = currentSongTime - startTimes[i];
                SaveBeat((NoteDirection)i, startTimes[i], duration);
                isHolding[i] = false;
            }
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
        Debug.Log("mapping starting... use WASD (hold for long notes)");
    }

    void SaveBeat(NoteDirection direction, float startTime, float duration)
    {
        if (useQuantization && songMap.songBpm > 0)
        {
            // this will calculate the time interval in relation to bpm -> 60 seconds in a min / (bpm * time interval)
            // for instance, a quarter note @ 120bpm is 60 / (120 * (4/4)) = 0.5 sec
            float beatInterval = 60f / (songMap.songBpm * ((float)songMap.notesPerMeasure / songMap.noteDivision));

            // this is so held notes do not cause the rest of song to be offset
            float rawEndTime = startTime + duration;
            float snappedStart = Mathf.Round(startTime / beatInterval) * beatInterval;
            float snappedEnd = Mathf.Round(rawEndTime / beatInterval) * beatInterval;

            startTime = snappedStart;
            duration = snappedEnd - snappedStart;
        }

        // this just ensures that short taps are not counted as holds and that there are not negative times
        float noteDuration = (duration < 0.15f) ? 0f : duration;

        BeatData newBeat = new BeatData
        {
            timestamp = startTime,
            duration = noteDuration,
            noteDirection = direction
        };

        songMap.beats.Add(newBeat);
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
            Vector3 startPos = new Vector3((int) beat.noteDirection * 2f, 0, beat.timestamp * visualSpacing);

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