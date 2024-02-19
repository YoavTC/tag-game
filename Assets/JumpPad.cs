using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            LaunchRigidbody(playerController.GetComponent<Rigidbody2D>());
        }
    }

    private void LaunchRigidbody(Rigidbody2D rb)
    {
        float yVelocity = rb.velocity.y;
        
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        
        if (yVelocity < -1)
        {
            Debug.Log("yVel: " + yVelocity);
            rb.AddForce(Vector2.up * jumpForce * Math.Abs(yVelocity), ForceMode2D.Impulse);
        } else rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        
        
    }
}
