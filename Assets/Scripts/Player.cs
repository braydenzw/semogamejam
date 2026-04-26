using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

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

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody2D>();
        collideMaybe = true;
        StringsSong.beats.Sort((beat1, beat2) => beat1.timestamp.CompareTo(beat2.timestamp));
        BrassSong.beats.Sort((beat1, beat2) => beat1.timestamp.CompareTo(beat2.timestamp));
        PercussionSong.beats.Sort((beat1, beat2) => beat1.timestamp.CompareTo(beat2.timestamp));
        WoodwindSong.beats.Sort((beat1, beat2) => beat1.timestamp.CompareTo(beat2.timestamp));

    }

    // Update is called once per frame
    void Update()
    {
        beatManager.transform.position = player.transform.position;
        if (Input.GetKeyDown(KeyCode.W))
        {
            Attack(NoteDirection.up);
        } else if (Input.GetKeyDown(KeyCode.A))
        {
            Attack(NoteDirection.left);
        } else if (Input.GetKeyDown(KeyCode.S))
        {
            Attack(NoteDirection.down);
        } else if (Input.GetKeyDown(KeyCode.D))
        {
            Attack(NoteDirection.right);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
           this.rigidbody.velocity = Vector3.MoveTowards(this.rigidbody.velocity,this.rigidbody.velocity+Vector2.up*maxVelocity,velocityAdder*Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
           this.rigidbody.velocity = Vector3.MoveTowards(this.rigidbody.velocity,this.rigidbody.velocity+Vector2.down*maxVelocity,velocityAdder*Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
           this.rigidbody.velocity = Vector3.MoveTowards(this.rigidbody.velocity,this.rigidbody.velocity+Vector2.left*maxVelocity,velocityAdder*Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
           this.rigidbody.velocity = Vector3.MoveTowards(this.rigidbody.velocity,this.rigidbody.velocity+Vector2.right*maxVelocity,velocityAdder*Time.deltaTime);
        }
    }

    private void Attack(NoteDirection noteDirection)
    {
        int dir = DirectionToDir(noteDirection);
        
        animator.SetFloat("Direction", (float)dir);
        Debug.Log(dir);
        animator.SetTrigger("Attack");
        spriteRenderer.flipX = (dir == 0);
        // animator.SetFloat("Direction", (float)dir);
        // spriteRenderer.flipX = (dir == 2);
        //animator.SetTrigger("Attack");
        //check on beat

        beatManager.GetComponent<BeatManagerScript>().OnTap(noteDirection);
        
    }

    private int DirectionToDir(NoteDirection noteDirection)
    {
        switch(noteDirection)
        {
            case NoteDirection.left:
                return 0;
            case NoteDirection.up:
                return 1;
            case NoteDirection.down:
                return 2;
            case NoteDirection.right:
                return 3;
        }
        return 0;
    }

    //when entering an area, make things happen (should be used for setting up areas later)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GameObject().tag == "Orchestra")
        {
            this.rigidbody.velocity = Vector2.zero;
            Debug.Log("COLLISION");
            beatManager.GetComponent<BeatManagerScript>().minigameOn = true;
            collided = collision.GameObject();
            beatManager.GetComponent<BeatManagerScript>().section = collided;
            beatManager.GetComponent<BeatManagerScript>().InitiateSong(collided.GetComponent<SectionHealth>().GetSong());
            collided.GetComponent<SectionHealth>().inUse = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if((collided != null) && (collision.tag == "Orchestra"))
        {
            collided.GetComponent<SectionHealth>().inUse = false;
            beatManager.GetComponent<BeatManagerScript>().minigameOn = false;
            collideMaybe = false;
        }
    }
    
    //returns a game object based on a randomly generated value
    
    
    public void getSection()
    {
        sectionNumber = (Random.Range(0,4));
        switch(sectionNumber)
        {
            case 0:
                circleCollider = GameObject.Find("Brass Section").GetComponent<CircleCollider2D>();
                Debug.Log("BRASS SELECTED!!!");
                currentSong = BrassSong;
                break;
            case 1:
                circleCollider =  GameObject.Find("Woodwinds Section").GetComponent<CircleCollider2D>();
                Debug.Log("WOOD SELECTED!!!");
                currentSong = WoodwindSong;
                break;
            case 2:
                circleCollider =  GameObject.Find("Strings Section").GetComponent<CircleCollider2D>();
                Debug.Log("STRINGS SELECTED!!!");
                currentSong = StringsSong;
                break;
            case 3:
                circleCollider =  GameObject.Find("Percussion Section").GetComponent<CircleCollider2D>();
                Debug.Log("PERCUSSION SELECTED!!!");
                currentSong = PercussionSong;
                break;
        }
    }
}
