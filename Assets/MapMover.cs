using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMover : MonoBehaviour
{
    private Rigidbody2D rb;
    public static float offset;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        offset = 1.5F;
        rb.velocity = new Vector2(-1 * offset, 0);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
