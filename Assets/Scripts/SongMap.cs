using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSongMap", menuName = "RhythmGame/SongMap")]
public class SongMap : ScriptableObject
{
    public string songName;
    public float songBpm;
    [Tooltip("this is top number of time signature shows how many beats in a measure: 4 = quarter notes, 8 = eighth notes, 16 = sixteenth notes, etc.")]
    public int notesPerMeasure = 4;

    [Tooltip("this is how split you want the grid, will def increase difficulty. smallest notes possible: 4 = quarters, 8 = eighths, 16 = sixteenths, use 12 to snap perfectly to both standard and triplet/swing rhythms.")]
    public int noteDivision = 4;

    public List<BeatData> beats = new List<BeatData>();
}