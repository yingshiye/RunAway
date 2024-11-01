using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectible : MonoBehaviour
{
    // Start is called before the first frame update
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = Resources.LoadAll<Sprite>("Pumpkins/pixel_pumpkins")[Random.Range(0, 8)];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GetComponent<Animator>().Play("collect");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
