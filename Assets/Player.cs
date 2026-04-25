using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("操你妈")]
    public SpriteRenderer spriteRenderer;

    public Animator animator;
    [SerializeField] private Sprite[] PlayerSprite;

    
        
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            attack(1);
        } else if (Input.GetKeyDown(KeyCode.A))
        {
            attack(0);
        } else if (Input.GetKeyDown(KeyCode.S))
        {
            attack(2);
        } else if (Input.GetKeyDown(KeyCode.D))
        {
            attack(3);
        }
            
    }

    void attack(int dir)
    {
        animator.SetFloat("Direction", (float)dir);
        spriteRenderer.flipX = (dir == 2);
        //animator.SetTrigger("Attack");
        //check on beat
    }

}
