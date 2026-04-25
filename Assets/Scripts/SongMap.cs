using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSongMap", menuName = "RhythmGame/SongMap")]
public class SongMap : ScriptableObject
{
    public string songName;
    public float songBpm;
    public List<BeatData> beats = new List<BeatData>();
}