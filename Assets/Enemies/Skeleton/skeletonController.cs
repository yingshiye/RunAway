using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeletonController : Enemy
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("ground"))
        {
            direction *= -1;
        }
    }

}
