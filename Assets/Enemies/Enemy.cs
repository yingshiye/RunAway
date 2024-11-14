using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected AudioSource audioSource;
    protected float direction;
    [SerializeField] protected float speed;
    [SerializeField] protected bool isInLevel;
    protected Vector3 distanceToPlayer;
    protected Vector3 PlayerPosition;

    protected float initialX;
    protected bool isPlayerInRange;
    [SerializeField] protected float range;
    [SerializeField] protected float distanceFromSpawn;
    // Start is called before the first frame update
    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        direction = 1;

        initialX = transform.position.x;
        isPlayerInRange = false;
    }

    protected void FixedUpdate(){

        PlayerPosition = PlayerMovement.instance.GetPosition();
        distanceToPlayer = PlayerPosition - transform.position;
        distanceFromSpawn = transform.position.x - initialX;

        if(direction * transform.localScale.x < 0){
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        bool isPlayerActuallyInRange = Mathf.Abs(distanceToPlayer.x) < range && Mathf.Abs(distanceToPlayer.y) < 1;

        if(isPlayerActuallyInRange && !isPlayerInRange){
            isPlayerInRange = true;
            direction = distanceToPlayer.x/Mathf.Abs(distanceToPlayer.x);
        }

        else if(!isPlayerActuallyInRange && isPlayerInRange){
            isPlayerInRange = false;
        }

        if(Mathf.Abs(distanceFromSpawn) > range && direction * distanceFromSpawn > 0){
            direction = -1 * direction;
        }

        if(isInLevel){
            rb.velocity = new Vector2(direction * speed - 1, rb.velocity.y);
        }else{
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        }

    }
}
