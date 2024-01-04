using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Camera")] 
    [SerializeField] private GameObject CameraPrefab;

    [Header("Movement Settings")] 
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb;
    
    private LayerMask playerLayer;
    [SerializeField] private float jumpForce;
    private bool isOnGround;
    private int extraJumps = 1;

    [Header("Testing")] 
    [SerializeField] private bool testingMode;
    
    private void Start()
    {
        //Setup
        if (!IsOwner && !testingMode) Destroy(this);
        rb = GetComponent<Rigidbody2D>();
        playerLayer = LayerMask.GetMask("Player");

        if (IsOwner)
        {
            CameraMovement clientCamera = Instantiate(CameraPrefab).GetComponent<CameraMovement>();
            clientCamera.InitiateCameraSettings(transform);
        }
        
        //Set Position
        SpawnManager.Instance.SetSpawnPoint(transform);
    }
    void Update()
    {
        float horizontalDistance = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(horizontalDistance, 0);

        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
        
        if (Input.GetButtonDown("Jump"))
        {
            if (isOnGround) Jump(false);
            else if (extraJumps > 0) Jump(true);
        }
    }

    #region Jumping & Ground

    private void Jump(bool isDouble)
    {
        if (isDouble) extraJumps--;
        else extraJumps = 1;
        
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != playerLayer)
        {
            isOnGround = true;
            extraJumps = 1;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer != playerLayer)
        {
            isOnGround = false;
        }
    }
    #endregion
}
