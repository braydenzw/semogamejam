using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [Header("操你妈")]
    public SpriteRenderer spriteRenderer;

    public Animator animator;
    [SerializeField] private Sprite[] PlayerSprite;
    [SerializeField] private GameObject beatManager;
    [SerializeField] private float velocityAdder;
    [SerializeField] public float maxVelocity;

    private Rigidbody2D rigidbody;
    public Collider2D circleCollider;
    public GameObject player;
    private int sectionNumber;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody2D>();
        nextSection();
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
        if(collision == circleCollider)
        {
            this.rigidbody.velocity = Vector2.zero;
            Debug.Log("COLLISION");
            beatManager.GetComponent<BeatManagerScript>().onOrOff = true;
            beatManager.GetComponent<BeatManagerScript>().timeToPlay = Random.value + 10;
            maxVelocity = 0;
        }
    }
    
    //returns a game object based on a randomly generated value
    public void nextSection()
    {
        sectionNumber = (int)(Random.value*3);
        switch(sectionNumber)
        {
            case 0:
                circleCollider = GameObject.Find("Brass Section").GetComponent<CircleCollider2D>();
                Debug.Log("BRASS SELECTED!!!");
                break;
            case 1:
                circleCollider =  GameObject.Find("Woodwind Section").GetComponent<CircleCollider2D>();
                Debug.Log("WOOD SELECTED!!!");
                break;
            case 2:
                circleCollider =  GameObject.Find("Strings Section").GetComponent<CircleCollider2D>();
                Debug.Log("STRINGS SELECTED!!!");
                break;
        }
    }
}
