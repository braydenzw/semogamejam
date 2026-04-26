using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    public enum BeatScore { Success, Failure }
    public enum NoteDirection { Left, Down, Up, Right }
    public enum PowerupType { DoubleDamage }
}

// Which orchestral section this zone belongs to
public enum SectionType { Brass, Percussion, Woodwinds, Strings }

// What kind of note mechanic this beat uses
public enum NoteType { Standard, MultiHit, Hold }