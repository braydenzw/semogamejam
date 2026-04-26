using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("操你妈")]
    public SpriteRenderer spriteRenderer;

    public Animator animator;
    [SerializeField] private Sprite[] PlayerSprite;
    [SerializeField] private GameObject beatManager;
    [SerializeField] private float velocityAdder;
    [SerializeField] public float maxVelocity;

    [SerializeField] private SongMap StringsSong;
    [SerializeField] private SongMap BrassSong;
    [SerializeField] private SongMap PercussionSong;
    [SerializeField] private SongMap WoodwindSong;

    private Rigidbody2D rigidbody;
    public int score = 0;
    public Collider2D circleCollider;
    public GameObject collided;
    public GameObject player;
    private int sectionNumber;
    public bool collideMaybe = true;

    private SongMap currentSong;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collideMaybe = true;
        StringsSong.beats.Sort((a, b) => a.timestamp.CompareTo(b.timestamp));
        BrassSong.beats.Sort((a, b) => a.timestamp.CompareTo(b.timestamp));
        PercussionSong.beats.Sort((a, b) => a.timestamp.CompareTo(b.timestamp));
        WoodwindSong.beats.Sort((a, b) => a.timestamp.CompareTo(b.timestamp));
    }

    void Update()
    {
        beatManager.transform.position = player.transform.position;

        // --- Tap attacks (key down) ---
        if (Input.GetKeyDown(KeyCode.W)) Attack(NoteDirection.up);
        if (Input.GetKeyDown(KeyCode.A)) Attack(NoteDirection.left);
        if (Input.GetKeyDown(KeyCode.S)) Attack(NoteDirection.down);
        if (Input.GetKeyDown(KeyCode.D)) Attack(NoteDirection.right);

        // --- Release (key up) — needed for hold notes in Strings ---
        if (Input.GetKeyUp(KeyCode.W)) Release(NoteDirection.up);
        if (Input.GetKeyUp(KeyCode.A)) Release(NoteDirection.left);
        if (Input.GetKeyUp(KeyCode.S)) Release(NoteDirection.down);
        if (Input.GetKeyUp(KeyCode.D)) Release(NoteDirection.right);

        // --- Movement ---
        if (Input.GetKey(KeyCode.UpArrow))
            rigidbody.velocity = Vector3.MoveTowards(rigidbody.velocity, rigidbody.velocity + Vector2.up    * maxVelocity, velocityAdder * Time.deltaTime);
        if (Input.GetKey(KeyCode.DownArrow))
            rigidbody.velocity = Vector3.MoveTowards(rigidbody.velocity, rigidbody.velocity + Vector2.down  * maxVelocity, velocityAdder * Time.deltaTime);
        if (Input.GetKey(KeyCode.LeftArrow))
            rigidbody.velocity = Vector3.MoveTowards(rigidbody.velocity, rigidbody.velocity + Vector2.left  * maxVelocity, velocityAdder * Time.deltaTime);
        if (Input.GetKey(KeyCode.RightArrow))
            rigidbody.velocity = Vector3.MoveTowards(rigidbody.velocity, rigidbody.velocity + Vector2.right * maxVelocity, velocityAdder * Time.deltaTime);
    }

    private void Attack(NoteDirection noteDirection)
    {
        int dir = DirectionToDir(noteDirection);
        animator.SetFloat("Direction", (float)dir);
        animator.SetTrigger("Attack");
        spriteRenderer.flipX = (dir == 0);
        beatManager.GetComponent<BeatManagerScript>().OnTap(noteDirection);
    }

    private void Release(NoteDirection noteDirection)
    {
        beatManager.GetComponent<BeatManagerScript>().OnRelease(noteDirection);
    }

    private int DirectionToDir(NoteDirection noteDirection)
    {
        switch (noteDirection)
        {
            case NoteDirection.left:  return 0;
            case NoteDirection.up:    return 1;
            case NoteDirection.down:  return 2;
            case NoteDirection.right: return 3;
        }
        return 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Orchestra")
        {
            rigidbody.velocity = Vector2.zero;
            Debug.Log("COLLISION");
            beatManager.GetComponent<BeatManagerScript>().minigameOn = true;
            collided = collision.gameObject;
            beatManager.GetComponent<BeatManagerScript>().section = collided;
            beatManager.GetComponent<BeatManagerScript>().InitiateSong(collided.GetComponent<SectionHealth>().GetSong());
            collided.GetComponent<SectionHealth>().inUse = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collided != null) && (collision.tag == "Orchestra"))
        {
            collided.GetComponent<SectionHealth>().inUse = false;
            beatManager.GetComponent<BeatManagerScript>().minigameOn = false;
            collideMaybe = false;
        }
    }

    public void getSection()
    {
        sectionNumber = Random.Range(0, 4);
        switch (sectionNumber)
        {
            case 0: circleCollider = GameObject.Find("Brass Section").GetComponent<CircleCollider2D>();     currentSong = BrassSong;      break;
            case 1: circleCollider = GameObject.Find("Woodwinds Section").GetComponent<CircleCollider2D>(); currentSong = WoodwindSong;   break;
            case 2: circleCollider = GameObject.Find("Strings Section").GetComponent<CircleCollider2D>();   currentSong = StringsSong;    break;
            case 3: circleCollider = GameObject.Find("Percussion Section").GetComponent<CircleCollider2D>();currentSong = PercussionSong; break;
        }
    }
}