using UnityEngine;
using System.Collections.Generic;
public enum NoteDirection { left, down, up, right }

[System.Serializable]
public class BeatData
{
    public float timestamp; // when exactly to hit the note (in sec)
    public float duration; // how long to hold a note (0 means tap)
    public NoteDirection noteDirection;  // where the notes will come from
}