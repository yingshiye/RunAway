using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 movementVector;
    private Rigidbody2D rb;
    private bool isGrounded = true;
    private bool isDashing = false;
    private int score = 0;
    private SpriteRenderer sr;
    private AudioSource audiosource;
    private Animator animator;
    private Transform transform_;

    //[SerializeField] Animator animator;
    [SerializeField] int speed = 0;
    [SerializeField] int jumpForce = 400;
    [SerializeField] int dashForce = 1000; 
    [SerializeField] float dashDuration = 0.1f; 

    private AudioClip jumpSFX;
    private AudioClip moveSFX;
    private AudioClip dashSFX;

    private AudioClip collectSFX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audiosource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        transform_ = GetComponent<Transform>();

        moveSFX = Resources.Load <AudioClip> ("PlayerSFX/walk");
        dashSFX = Resources.Load <AudioClip> ("PlayerSFX/dash");
        jumpSFX = Resources.Load <AudioClip> ("PlayerSFX/jump");
        collectSFX = Resources.Load <AudioClip> ("PlayerSFX/collect");

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            animator.SetBool("isJumping", false);
            isGrounded = true;
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

        if(movementVector.y > 0 && isGrounded){
            animator.SetBool("isJumping", true);
            rb.AddForce(new Vector2(0, jumpForce));
            audiosource.PlayOneShot(jumpSFX);
        }

        if(movementVector.x * transform_.localScale.x < 0){
            transform_.localScale = new Vector3(transform_.localScale.x * -1, transform_.localScale.y, transform_.localScale.z);
        }

    }


    void OnDash(InputValue value)
    {
        if (!isDashing)
        {
            isDashing = true;

            rb.velocity = new Vector2(movementVector.x * dashForce, rb.velocity.y);

            Invoke("EndDash", dashDuration);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Collectable"))
        {
            other.gameObject.SetActive(false);
            score++;
            Debug.Log("My score is " + score);
            audiosource.PlayOneShot(collectSFX);
        }
    }

    void Update()
    {
       rb.velocity = new Vector2(speed * movementVector.x, rb.velocity.y);

        if(movementVector.x != 0 && !animator.GetBool("isJumping")){
            animator.SetBool("isWalking", true);            
            if(!audiosource.isPlaying && isGrounded){
                audiosource.PlayOneShot(moveSFX);
            }
        }
        else{
            animator.SetBool("isWalking", false); 
            audiosource.Pause();
        }
    }
}
