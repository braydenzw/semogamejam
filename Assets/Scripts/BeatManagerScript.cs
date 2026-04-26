using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static Enums;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SectionSpawnSettings
{
    [Tooltip("Which section this applies to")]
    public SectionType sectionType;

    [Tooltip("Minimum time between any note spawn for this section")]
    public float minSpawnInterval = 0.4f;

    [Tooltip("Max notes allowed per direction lane at once")]
    public int maxNotesPerDirection = 3;

    // You can add more overrides later – e.g. a speed multiplier, a damage modifier, etc.
}

public class BeatManagerScript : MonoBehaviour
{
    [Header("Per‑Section Spawn Settings")]
    [SerializeField] private List<SectionSpawnSettings> sectionSettings = new List<SectionSpawnSettings>();
    private float lastSpawnTime = -999f;
    
    [SerializeField] float timeInterval;
    private float currentTime = 0;

    [SerializeField] float timeToClick;
    [SerializeField] float timingWindow;
    [SerializeField] float spawnDistanceMultiplier;
    [SerializeField] float targetPositionMultiplier;

    [SerializeField] GameObject HorizontalLine;
    [SerializeField] GameObject VerticalLine;

    private SongMap currentSong;
    private Stack<GameObject> inactiveBeatsStack;

    private Queue<GameObject> leftQueue;
    private Queue<GameObject> rightQueue;
    private Queue<GameObject> upQueue;
    private Queue<GameObject> downQueue;

    public GameObject beatPrefab;
    public GameObject player;
    public GameObject section;
    public TMP_Text scoreText;

    public bool minigameOn = false;

    List<BeatData> beatsList;
    private int beatIndex = 0;
    [SerializeField] float newSongWait;

    private bool exitKeyPressed = false;

    // what section type
    private SectionType currentSectionType = SectionType.Brass;
    // brass hurts more; multiplier is 2x for brass, 1x otherwise
    private float missHealthMultiplier = 1f;

    void Start()
    {
        minigameOn = false;
        exitKeyPressed = false;
        inactiveBeatsStack = new Stack<GameObject>();
        leftQueue  = new Queue<GameObject>();
        rightQueue = new Queue<GameObject>();
        upQueue    = new Queue<GameObject>();
        downQueue  = new Queue<GameObject>();

        foreach (NoteDirection noteDirection in Enum.GetValues(typeof(NoteDirection)))
        {
            GameObject lineToSpawn = (noteDirection == NoteDirection.left || noteDirection == NoteDirection.right)
                ? VerticalLine : HorizontalLine;
            Instantiate(lineToSpawn, transform.position + GetDirectionVector(noteDirection) * targetPositionMultiplier, transform.rotation, transform);
        }
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && SceneManager.GetActiveScene() != SceneManager.GetSceneByName("FletcherBossFight"))
        {
            exitKeyPressed = true;

        if (!exitKeyPressed && minigameOn)
        {
            // Fetch the spawn rules for the section we're currently in
            SectionSpawnSettings spawnSettings = GetCurrentSectionSettings();

            while (beatIndex < beatsList.Count && beatsList[beatIndex].timestamp - timeToClick < currentTime)
            {
                // respect the per‑section spawn cooldown
                if (currentTime - lastSpawnTime < spawnSettings.minSpawnInterval)
                    break;

                //  lne‑cap is handled inside SpawnNoteForSection so we can just call it. 他妈的
                SpawnNoteForSection(beatsList[beatIndex]);
                beatIndex++;
            }
        }

        if (minigameOn && CheckQueuesEmpty() && exitKeyPressed)
        {
            player.GetComponent<Player>().maxVelocity = 5;
            player.GetComponent<Player>().collideMaybe = true;
            minigameOn = false;
        }
        
    }

    private void SpawnNoteForSection(BeatData beatData)
    {
        NoteDirection dir = beatData.noteDirection;
        Queue<GameObject> queue;
        int hits = 1;
        float holdDur = 0f;
        NoteType type = NoteType.Standard;

        // Get the current section's spawn rules
        SectionSpawnSettings settings = GetCurrentSectionSettings();
        int maxPerLane = settings.maxNotesPerDirection;

        switch (currentSectionType)
        {
            case SectionType.Percussion:
                dir = (UnityEngine.Random.value > 0.5f) ? NoteDirection.left : NoteDirection.right;
                hits = UnityEngine.Random.Range(2, 4);
                type = NoteType.MultiHit;
                break;

            case SectionType.Strings:
                holdDur = Mathf.Max(beatData.duration, 0.3f);
                type = NoteType.Hold;
                break;

            case SectionType.Woodwinds:
                // Two notes simultaneously, different directions
                NoteDirection secondDir = GetDifferentDirection(dir);
                // Check lane cap for BOTH directions before spawning
                if (GetQueue(dir).Count >= maxPerLane || GetQueue(secondDir).Count >= maxPerLane)
                    return;

                SpawnObject(dir, NoteType.Standard, 1, 0f);
                SpawnObject(secondDir, NoteType.Standard, 1, 0f);
                lastSpawnTime = currentTime;
                return;

            default: // Brass
                type = NoteType.Standard;
                break;
        }

        // For Brass, Percussion, Strings
        queue = GetQueue(dir);
        if (queue.Count >= maxPerLane)
            return;   // lane full, skip this note

        SpawnObject(dir, type, hits, holdDur);
        lastSpawnTime = currentTime;
    }

    private GameObject SpawnObject(NoteDirection noteDirection, NoteType noteType = NoteType.Standard,
        int hitsRequired = 1, float holdDuration = 0.5f)
    {
        GameObject newBeat;
        Queue<GameObject> currentQueue = GetQueue(noteDirection);

        if (inactiveBeatsStack.Count > 0)
        {
            newBeat = inactiveBeatsStack.Pop();
            newBeat.transform.position = transform.position + GetDirectionVector(noteDirection) * spawnDistanceMultiplier;
        }
        else
        {
            newBeat = Instantiate(beatPrefab,
                transform.position + GetDirectionVector(noteDirection) * spawnDistanceMultiplier,
                transform.rotation, transform);
        }

        currentQueue.Enqueue(newBeat);
        newBeat.GetComponent<BeatScript>().Initialize(
            timeToClick, timingWindow,
            transform.position + GetDirectionVector(noteDirection) * targetPositionMultiplier,
            gameObject, noteDirection, noteType, hitsRequired, holdDuration);
        newBeat.SetActive(true);
        return newBeat;
    }

    public void OnTap(NoteDirection noteDirection)
    {
        Queue<GameObject> currentQueue = GetQueue(noteDirection);
        Debug.Log($"Tap {noteDirection}, queue count: {currentQueue.Count}");
        if (currentQueue.Count > 0)
            currentQueue.Peek().GetComponent<BeatScript>().OnHit();
    }

    // Called by Player when a direction key is released (for hold notes)
    public void OnRelease(NoteDirection noteDirection)
    {
        Queue<GameObject> currentQueue = GetQueue(noteDirection);
        if (currentQueue.Count > 0)
            currentQueue.Peek().GetComponent<BeatScript>().OnRelease();
    }

    public void InitiateSong(SongMap newSong)
    {
        beatsList = newSong.beats;
        beatIndex = 0;

        //should be section type from the section we just entered
        currentSectionType = section.GetComponent<SectionHealth>().sectionType;
        Debug.Log($"Entered section: {section.name}, type: {currentSectionType}");
        missHealthMultiplier  = (currentSectionType == SectionType.Brass) ? 2f : 1f;

        while (beatsList[beatIndex].timestamp - timeToClick + newSongWait < currentTime)
            beatIndex++;

        exitKeyPressed = false;
        minigameOn = true;
        player.GetComponent<Player>().maxVelocity = 0;
    }

    public void BeatDeath(GameObject toDie, BeatScore beatScore, NoteDirection noteDirection)
    {
        Debug.Log($"BeatDeath called | Score: {beatScore} | Dir: {noteDirection} | Section: {(section != null ? section.name : "NULL")}");        if (beatScore == BeatScore.Failure)
        {
            if (section != null)
            {
            section.GetComponent<SectionHealth>().ChangeHealth(-5);
            }
            section.GetComponent<SectionHealth>().ChangeHealth((int)(-5 * missHealthMultiplier));
            ComboManager.instance.EndCombo();
        }
        else if (beatScore == BeatScore.Success)
        {
            if (section != null)
            {
                section.GetComponent<SectionHealth>().ChangeHealth(5);
                if (section.GetComponent<SectionHealth>().sectionHealth >= 100)
                {
                    section.GetComponent<SectionHealth>().sectionHealth = 100;
                }
            }
            section.GetComponent<SectionHealth>().ChangeHealth(5);
            if (section.GetComponent<SectionHealth>().sectionHealth >= 100)
                section.GetComponent<SectionHealth>().sectionHealth = 100;
            ComboManager.instance.ExtendCombo();
        }

        toDie.SetActive(false);
        inactiveBeatsStack.Push(toDie);
        GetQueue(noteDirection).Dequeue();
    }

    // --- Helpers ---

    private NoteDirection GetDifferentDirection(NoteDirection original)
    {
        NoteDirection[] all = { NoteDirection.left, NoteDirection.right, NoteDirection.up, NoteDirection.down };
        NoteDirection picked;
        do { picked = all[UnityEngine.Random.Range(0, 4)]; }
        while (picked == original);
        return picked;
    }

    private Queue<GameObject> GetQueue(NoteDirection noteDirection)
    {
        switch (noteDirection)
        {
            case NoteDirection.left:  return leftQueue;
            case NoteDirection.right: return rightQueue;
            case NoteDirection.up:    return downQueue;
            case NoteDirection.down:  return upQueue;
        }
        return null;
    }

    private Vector3 GetDirectionVector(NoteDirection noteDirection)
    {
        switch (noteDirection)
        {
            case NoteDirection.left:  return Vector3.left;
            case NoteDirection.right: return Vector3.right;
            case NoteDirection.up:    return Vector3.up;
            case NoteDirection.down:  return Vector3.down;
        }
        return Vector3.zero;
    }

    private bool CheckQueuesEmpty()
    {
        return leftQueue.Count + rightQueue.Count + upQueue.Count + downQueue.Count == 0;
    }
    private SectionSpawnSettings GetCurrentSectionSettings()
    {
        foreach (var s in sectionSettings)
        {
            if (s.sectionType == currentSectionType)
                return s;
        }
        // use defaults if nothing fonid
        Debug.LogWarning($"No spawn settings found for {currentSectionType}, using defaults.");
        return new SectionSpawnSettings { minSpawnInterval = 0.4f, maxNotesPerDirection = 2 };
    }
}