using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    public static int numPumpkins;

    [SerializeField] Vector2 movementVector;
    private Rigidbody2D rb;
    [SerializeField] int jumpsFromGround;
    [SerializeField] int score;
    private SpriteRenderer sr;
    private AudioSource audioSource;
    private Animator animator;
    [SerializeField] int speed;
    [SerializeField] int jumpForce;
    // [SerializeField] float dashDuration; 

    private AudioClip jumpSFX;
    private AudioClip moveSFX;
    private AudioClip dashSFX;
    private AudioClip landingSFX;
    private AudioClip collectSFX;
    private AudioClip warningSFX;
    private bool dashHeld;

    private GameObject D;
    private Transform cameraTransform;
    [SerializeField]  bool inLevel;

    public int getScore(){
        return score;
    }


    void Start()
    {
        instance = this;

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        dashHeld = false;
        jumpsFromGround = 0;

        moveSFX = Resources.Load <AudioClip> ("PlayerSFX/walk");
        dashSFX = Resources.Load <AudioClip> ("PlayerSFX/dash");
        jumpSFX = Resources.Load <AudioClip> ("PlayerSFX/jump");
        collectSFX = Resources.Load <AudioClip> ("PlayerSFX/collect");
        landingSFX = Resources.Load <AudioClip> ("PlayerSFX/landing");
        warningSFX = Resources.Load <AudioClip> ("PlayerSFX/warning");

        cameraTransform = GameObject.Find("Main Camera").transform;

        score = 0;

        var allCollectibles = FindObjectsByType<Collectible>(FindObjectsSortMode.None);
        numPumpkins = (int)allCollectibles.Length;

        Debug.Log(numPumpkins);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            animator.SetBool("isJumping", false);
            jumpsFromGround = 0;
            audioSource.PlayOneShot(landingSFX, 0.5F);
        }
        if(collision.gameObject.CompareTag("Enemy")){
            SceneManager.LoadScene("DeathScreen");
        }
    }

    void OnMove(InputValue value)
    {
        movementVector = value.Get<Vector2>();

        if(movementVector.y > 0 && jumpsFromGround == 0){
            jumpsFromGround++;
            animator.SetBool("isJumping", true);
            rb.AddForce(new Vector2(0, jumpForce));
            audioSource.PlayOneShot(jumpSFX);
        }

        if(movementVector.x * transform.localScale.x < 0){
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
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

            if(score >= numPumpkins){
                SceneManager.LoadScene("EndScene");
            }
        }

        if(other.gameObject.CompareTag("Enemy")){
            SceneManager.LoadScene("DeathScreen");
        }
    }

    void Update()
    {

        if(inLevel && D == null && score < numPumpkins && transform.position.x - Enemy.MapTransform.position.x > 530){
            D = Instantiate(Resources.Load<GameObject>("D/d"), new Vector3(15, transform.position.y, -5), Quaternion.identity) as GameObject;
        }

        if(Mathf.Abs(transform.position.x - cameraTransform.position.x) > 9.1F || transform.position.y < -5.5F){
            SceneManager.LoadScene("DeathScreen");
        }

        if(animator.GetBool("isDashing")){
            rb.velocity = new Vector2(2* speed * movementVector.x, rb.velocity.y);            
        }
        else{
            rb.velocity = new Vector2(speed * movementVector.x, rb.velocity.y);
        }

        if(inLevel){
            rb.velocity = new Vector2(rb.velocity.x - MapMover.offset, rb.velocity.y);
        }

        if(movementVector.x != 0 && !animator.GetBool("isJumping")){
            animator.SetBool("isWalking", true);
            if(dashHeld && !animator.GetBool("isDashing")){
                animator.SetBool("isDashing", true);
                audioSource.PlayOneShot(dashSFX); 
            }            
            if(!audioSource.isPlaying && jumpsFromGround == 0 && !animator.GetBool("isDashing")){
                audioSource.PlayOneShot(moveSFX);
            }
        }
        else{
            animator.SetBool("isWalking", false); 
        }
    }

    public Vector3 GetPosition(){
        return transform.position;
    }
}
