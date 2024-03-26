using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class AnimationController : MonoBehaviour
{
    public Animator Player_Animation;
    public FixedJoystick fixedJoystick;
    public Rigidbody2D Player;
    public bool IsGrounded; 
  

    // Start is called before the first frame update
    void Start()
    {
        Player_Animation = GetComponent<Animator>();
        Player = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
      
         
       if(fixedJoystick.Horizontal != 0) 
        {
            Player_Animation.SetBool("Walk", true);
        }
        else
        {
            Player_Animation.SetBool("Walk", false);  
        }
    }

    public void PlayerJump()
    {
        if (IsGrounded)
        {
            Player_Animation.SetTrigger("Jump");
            Player.velocity = new Vector3(0f, 10f, 0);
            IsGrounded = false;
        }
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Base"))
        {
            IsGrounded=true;
        }
    }
}
