using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private bool isOnGround;
    private LayerMask playerLayer;

    private void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != playerLayer)
        {
            isOnGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer != playerLayer)
        {
            isOnGround = false;
        }
    }

    public bool isGrounded()
    {
        return isOnGround;
    }
}
