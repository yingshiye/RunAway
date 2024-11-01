using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 movementVector;
    private Rigidbody2D rb;
    private bool isGrounded = true;
    private int jumpsFromGround = 0;
    private int score = 0;
    private SpriteRenderer sr;
    private AudioSource audioSource;
    private Animator animator;
    private Transform transform_;

    //[SerializeField] Animator animator;
    [SerializeField] int speed;
    [SerializeField] int jumpForce;
    [SerializeField] float dashDuration = 0.1f; 

    private AudioClip jumpSFX;
    private AudioClip moveSFX;
    private AudioClip dashSFX;
    private AudioClip landingSFX;

    private AudioClip collectSFX;
    private bool dashHeld;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        transform_ = GetComponent<Transform>();

        moveSFX = Resources.Load <AudioClip> ("PlayerSFX/walk");
        dashSFX = Resources.Load <AudioClip> ("PlayerSFX/dash");
        jumpSFX = Resources.Load <AudioClip> ("PlayerSFX/jump");
        collectSFX = Resources.Load <AudioClip> ("PlayerSFX/collect");
        landingSFX = Resources.Load <AudioClip> ("PlayerSFX/landing");

        dashHeld = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground") 
        && transform_.position.y > collision.GetContact(0).point.y)
        {
            animator.SetBool("isJumping", false);
            isGrounded = true;
            jumpsFromGround = 0;
            audioSource.PlayOneShot(landingSFX, 0.5F);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = false;
        }
    }

    void OnMove(InputValue value)
    {
        movementVector = value.Get<Vector2>();

        if(movementVector.y > 0 && jumpsFromGround < 2){
            jumpsFromGround++;
            animator.SetBool("isJumping", true);
            rb.AddForce(new Vector2(0, jumpForce));
            audioSource.PlayOneShot(jumpSFX);
        }

        if(movementVector.x * transform_.localScale.x < 0){
            transform_.localScale = new Vector3(transform_.localScale.x * -1, transform_.localScale.y, transform_.localScale.z);
        }

    }

    void OnDash(InputValue value){
        if(!animator.GetBool("isJumping") && animator.GetBool("isWalking") && !animator.GetBool("isDashing") && value.Get<float>() == 1){
            animator.SetBool("isDashing", true);
            audioSource.PlayOneShot(dashSFX);            
        }

        dashHeld = value.Get<float>() == 1;

        if(animator.GetBool("isJumping") || !animator.GetBool("isWalking") ||  value.Get<float>() == 0){
            animator.SetBool("isDashing", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Collectible"))
        {
            other.gameObject.GetComponent<Collider2D>().enabled = false;
            score++;
            Debug.Log("My score is " + score);
            audioSource.PlayOneShot(collectSFX);
        }
    }

    void Update()
    {
        if(animator.GetBool("isDashing")){
            rb.velocity = new Vector2(2* speed * movementVector.x, rb.velocity.y);            
        }
        else{
            rb.velocity = new Vector2(speed * movementVector.x, rb.velocity.y);
        }

        if(movementVector.x != 0 && !animator.GetBool("isJumping")){
            animator.SetBool("isWalking", true);
            if(dashHeld && !animator.GetBool("isDashing")){
                animator.SetBool("isDashing", true);
                audioSource.PlayOneShot(dashSFX); 
            }            
            if(!audioSource.isPlaying && isGrounded && !animator.GetBool("isDashing")){
                audioSource.PlayOneShot(moveSFX);
            }
        }
        else{
            animator.SetBool("isWalking", false); 
        }
    }
}
