using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewSongMap", menuName = "Rhythm/SongMap")]
public class SongMap : ScriptableObject
{
    public string songName;
    public float bpm;
    public List<float> beatPositionTracker;
}