using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeletonController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private float initialX;
    private bool isPlayerInRange;
    [SerializeField] float speed;
    [SerializeField] float range;
    [SerializeField] float direction;
    [SerializeField] Vector3 distanceToPlayer;
    [SerializeField] float distanceFromSpawn;
    [SerializeField] bool isInLevel;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        direction = -1;

        initialX = transform.position.x;
        isPlayerInRange = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 PlayerPosition = PlayerMovement.instance.GetPosition();
        distanceToPlayer = PlayerPosition - transform.position;
        distanceFromSpawn = transform.position.x - initialX;

        if(Mathf.Abs(distanceToPlayer.x) < range && -1 < distanceToPlayer.y  && distanceToPlayer.y < 0 && !isPlayerInRange){
            isPlayerInRange = true;
            direction = distanceToPlayer.x/Mathf.Abs(distanceToPlayer.x);
        }

        if(!(Mathf.Abs(distanceToPlayer.x) < range && -1 < distanceToPlayer.y  && distanceToPlayer.y < 0 && isPlayerInRange) && isPlayerInRange){
            isPlayerInRange = false;
        }

        if(Mathf.Abs(distanceFromSpawn) > range && direction * distanceFromSpawn > 0){
            direction = -1 * direction;
        }

        if(direction * transform.localScale.x > 0){
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        if(isInLevel){
            rb.velocity = new Vector2(direction * speed - 1, rb.velocity.y);
        }else{
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        }
    }
}
