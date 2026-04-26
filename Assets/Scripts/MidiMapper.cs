using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MidiMapper : MonoBehaviour
{
    [Header("map data")]
    public SongMap songMap;

    [Tooltip("drag your raw .mid file directly into this slot!")]
    public Object midiAsset;

    [Header("editor")]
    public float visualSpacing = 10f;
    [HideInInspector] public int selectedNoteIndex = -1;

    [Header("make notes")]
    public KeyCode generateKey = KeyCode.Space;

    [Header("rhythm game filters")]
    [Tooltip("0 to 127 - ignores quiet background notes: set between 60-80 for main rhythm")]
    [Range(0, 127)] public int minVelocity = 64;

    [Tooltip("ignores grace notes and trills")]
    public float ignoreNotesShorterThan = 0.05f;

    [Tooltip("prevents chords from spawning 4 arrows at the exact same time")]
    public float minimumTimeBetweenArrows = 0.1f;

    void Update()
    {
        if (Input.GetKeyDown(generateKey))
        {
            ConvertMidiToMap();
        }
    }

    void ConvertMidiToMap()
    {
        if (midiAsset == null)
        {
            Debug.LogError("no midi file assigned so drag a .mid file into the slot.");
            return;
        }

#if UNITY_EDITOR
        string relativePath = AssetDatabase.GetAssetPath(midiAsset);
        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

        songMap.beats.Clear();
        Debug.Log($"mapping midi from: {relativePath}");

        MidiFile midiFile = MidiFile.Read(fullPath);
        TempoMap tempoMap = midiFile.GetTempoMap();

        var allNotes = midiFile.GetNotes();

        float lastSavedArrowTime = -999f;

        foreach (var midiNote in allNotes)
        {
            // if too quiet skips the note
            if (midiNote.Velocity < minVelocity) continue;

            MetricTimeSpan startTimeSpan = midiNote.TimeAs<MetricTimeSpan>(tempoMap);
            MetricTimeSpan lengthTimeSpan = midiNote.LengthAs<MetricTimeSpan>(tempoMap);

            float startTimeInSeconds = (float)startTimeSpan.TotalSeconds;
            float durationInSeconds = (float)lengthTimeSpan.TotalSeconds;

            // if trill note itll skip
            if (durationInSeconds < ignoreNotesShorterThan) continue;

            // if chord it skips 2 out of 3 notes
            if (startTimeInSeconds - lastSavedArrowTime < minimumTimeBetweenArrows) continue;
            if (durationInSeconds < 0.15f) durationInSeconds = 0f;

            NoteDirection dir = (NoteDirection)Random.Range(0, 4);

            BeatData newBeat = new BeatData
            {
                timestamp = startTimeInSeconds,
                duration = durationInSeconds,
                noteDirection = dir
            };

            songMap.beats.Add(newBeat);

            lastSavedArrowTime = startTimeInSeconds;
        }

        Debug.Log($"generated a rhythm map with {songMap.beats.Count} notes");
        EditorUtility.SetDirty(songMap);

#else
        Debug.LogError("this only works in the unity editor!");
#endif
    }
}