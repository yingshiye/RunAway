using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostController : Enemy
{
    [SerializeField] float teleportRate;
    [SerializeField] float disappearanceTime;
    [SerializeField] Vector2 teleportXRange;
    [SerializeField] Vector2 teleportYRange;

    private float teleportInterval;

    private bool hasDisappeared;
    private bool hasReappeared;

    private AudioClip disappearSFX;
    private AudioClip appearSFX;

    private Animator exclamationMarkAnimator;
    private AudioSource exclamationMarkAudioSource;

    new void Start()
    {
        base.Start();

        GameObject exclamationMark = transform.GetChild(0).gameObject;

        exclamationMarkAnimator = exclamationMark.GetComponent<Animator>();
        exclamationMarkAnimator.Play("none");
        exclamationMarkAudioSource = exclamationMark.GetComponent<AudioSource>();
        
        direction = 1;

        teleportInterval = 0;
        hasDisappeared = false;
        hasReappeared = false;

        disappearSFX = Resources.Load<AudioClip>("EnemySFX/Ghost/GhostDisappear");
        appearSFX = Resources.Load<AudioClip>("EnemySFX/Ghost/GhostAppear");
    }

    // Update is called once per frame
    new void FixedUpdate()
    {           
        teleportInterval += Time.deltaTime;

        if(hasDisappeared){
            if(teleportInterval >= disappearanceTime){
                PlayerPosition = PlayerMovement.instance.GetPosition();
                if((Random.Range(0, 2) == 0 && PlayerPosition.x > teleportXRange.x + MapTransform.position.x) || PlayerPosition.x >= teleportXRange.y){
                    transform.position = new Vector3(Random.Range(teleportXRange.x + MapTransform.position.x, Mathf.Max(PlayerPosition.x - 2, teleportXRange.x + MapTransform.position.x)), Random.Range(teleportYRange.x, teleportYRange.y), transform.position.z);
                }else{
                    transform.position = new Vector3(Random.Range(Mathf.Min(PlayerPosition.x + 2, teleportXRange.y + MapTransform.position.x), teleportXRange.y + MapTransform.position.x), Random.Range(teleportYRange.x, teleportYRange.y), transform.position.z);
                }

                initialX = transform.position.x - MapTransform.position.x;

                if(distanceToPlayer.x != 0){
                    direction = distanceToPlayer.x/Mathf.Abs(distanceToPlayer.x);
                }

                animator.Play("ghostAppear");
                Vector3 CameraPosition = GameObject.Find("Main Camera").transform.position;
                if(Mathf.Abs(transform.position.x - CameraPosition.x) < 7.5F && Mathf.Abs(transform.position.y - CameraPosition.y) < 5){
                    audioSource.PlayOneShot(appearSFX);
                    exclamationMarkAnimator.Play("warning");
                    exclamationMarkAudioSource.Play();
                }
                teleportInterval = 0;
                hasDisappeared = false;
            }


        }
        else if(hasReappeared){
            
            if(teleportInterval >= teleportRate){
                hasDisappeared = true;
                hasReappeared = false;
                animator.Play("ghostDisappear");
                if(spriteRenderer.isVisible){
                    audioSource.PlayOneShot(disappearSFX);
                }
                teleportInterval = 0;
                rb.velocity = new Vector2(0, 0);
                return;
            }

            base.FixedUpdate();

        }else{

            if(animator.GetCurrentAnimatorStateInfo(0).IsName("ghostIdle")){
                hasReappeared = true;
            }

        }
    }
}