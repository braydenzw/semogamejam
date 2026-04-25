using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSongMap", menuName = "RhythmGame/SongMap")]
public class SongMap : ScriptableObject
{
    public string songName;
    public float songBpm;
    [Tooltip("this is just 4 = quarter notes, 8 = eighth notes, 16 = sixteenth notes, etc.")]
    public int notesPerMeasure = 4;
    public int noteDivision = 4;

    public List<BeatData> beats = new List<BeatData>();
}