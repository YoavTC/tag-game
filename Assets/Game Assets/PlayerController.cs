using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private bool testingMode;
    
    [Header("Camera")] 
    [SerializeField] private GameObject CameraPrefab;

    [Header("Movement Settings")] 
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb;

    [Header("Tagging Settings")] 
    [SerializeField] private float taggingRadius;
    
    private LayerMask playerLayer;
    [SerializeField] private float jumpForce;
    private bool isOnGround;
    private int extraJumps = 1;
    
    
    private void Start()
    {
        //Delete other client's controller
        if (!IsOwner && !testingMode) Destroy(this);
        
        //Set up variables
        rb = GetComponent<Rigidbody2D>();
        playerLayer = LayerMask.GetMask("Player");

        //Spawn camera
        if (IsOwner)
        {
            CameraMovement clientCamera = Instantiate(CameraPrefab).GetComponent<CameraMovement>();
            clientCamera.InitiateCameraSettings(transform);
        }
        
        //Set player's position
        SpawnManager.Instance.SetSpawnPoint(transform);
    }
    
    void Update()
    {
        //Get input movement
        float horizontalDistance = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(horizontalDistance, 0);

        //Apply movement velocity to the player's rigidbody
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
        
        //jump detection
        if (Input.GetButtonDown("Jump"))
        {
            if (isOnGround) Jump(false);
            else if (extraJumps > 0) Jump(true);
        }

        if (Input.GetButtonDown("Tag"))
        {
            Tag();
        }
    }

    #region Jumping & Ground
    //Jump logic
    private void Jump(bool isDouble)
    {
        if (isDouble) extraJumps--;
        else extraJumps = 1;
        
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    
    //Ground check detection
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

    #region Tagging

    private void Tag()
    {
        Transform taggedPlayer = GetTaggedPlayer();

        if (taggedPlayer != null)
        {
            TitleSystem.Instance.DisplayText("You Tagged " + taggedPlayer+ "!", true, "#d32c2f");
            Debug.Log("You tagged " + taggedPlayer, taggedPlayer);
            try
            { 
                GameManager.Instance.ClientTagClientServerRpc(taggedPlayer.GetComponent<NetworkBehaviour>().OwnerClientId, OwnerClientId);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }
    }
    
    private Transform GetTaggedPlayer()
    {
        Debug.Log("Searching for tags...");
        Vector3 position = transform.position;
        List<Collider2D> taggedPlayers = Physics2D.OverlapCircleAll(position, taggingRadius, playerLayer).ToList();
        taggedPlayers.Remove(GetComponent<Collider2D>());

        if (taggedPlayers.Count > 1)
        {
            Transform closestTaggedPlayer = taggedPlayers[0].transform;
            float closestDistance = Vector2.Distance(closestTaggedPlayer.position, position);

            for (int i = 0; i < taggedPlayers.Count; i++)
            {
                float distance = Vector2.Distance(taggedPlayers[i].transform.position, position);

                if (distance < closestDistance)
                {
                    closestTaggedPlayer = taggedPlayers[i].transform;
                    closestDistance = distance;
                }
            }
            return closestTaggedPlayer;
        } 
        if (taggedPlayers.Count == 1)
        {
            return taggedPlayers[0].transform;
        }

        return null;
    }
    #endregion
}
