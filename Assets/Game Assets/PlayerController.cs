using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private bool testingMode;
    private bool isLocalGame;
    
    [Header("Camera")] 
    [SerializeField] private GameObject CameraPrefab;

    [Header("Movement Settings")] 
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb;

    [Header("Tagging Settings")] 
    [SerializeField] [ReadOnly] private bool isTagger;
    [SerializeField] private float taggingRadius;

    [Header("Keybinds")] 
    [SerializeField] [ReadOnly] private string moveInput = "M_PC_Horizontal";
    [SerializeField] [ReadOnly] private string jumpInput = "L_PC1_Jump";
    [SerializeField] [ReadOnly] private string tagInput = "L_PC1_Tag";
    
    private LayerMask playerLayer;
    [SerializeField] private float jumpForce;
    private bool isOnGround;
    private int extraJumps = 1;
    
    
    private void Start()
    {
        Debug.Log("Player spawned!", transform);
        isLocalGame = GameManager.Instance.isLocalGame;
        
        SetUpBindings();
        
        //Delete other client's controller
        if (!isLocalGame && !IsOwner && !testingMode) Destroy(this);
        
        //Set up variables
        rb = GetComponent<Rigidbody2D>();
        playerLayer = LayerMask.GetMask("Player");

        //Spawn camera
        if (isLocalGame)
        {
            //TODO Set up local camera
        } else if (IsOwner)
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
        float horizontalDistance = Input.GetAxis(moveInput);
        Vector2 movement = new Vector2(horizontalDistance, 0);

        //Apply movement velocity to the player's rigidbody
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
        
        //jump detection
        if (Input.GetButtonDown(jumpInput))
        {
            if (isOnGround) Jump(false);
            else if (extraJumps > 0) Jump(true);
        }

        if (Input.GetButtonDown(tagInput) && isTagger)
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
            isTagger = false;
            
            TitleSystem.Instance.DisplayText("You Tagged " + taggedPlayer+ "!", true, "#d32c2f");
            
            if (isLocalGame) GameManager.Instance.ClientTagClient(taggedPlayer, transform);
            else GameManager.Instance.ClientTagClientServerRpc(taggedPlayer.GetComponent<NetworkBehaviour>().OwnerClientId, OwnerClientId);
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

    public void GetTaggedClient(ulong taggedID)
    {
        if (OwnerClientId == taggedID)
        {
            isTagger = true;
            Debug.Log("You got tagged!");
        }
    }

    public void GetTagged()
    {
        isTagger = true;
        Debug.Log("You got tagged!");
    }
    #endregion

    #region Input Bindings
    public void SetUpBindings()
    {
        if (isLocalGame)
        {
            InputMap inputMap = GameManager.Instance.GetComponent<InputManager>().GetInputBindingMap();
            moveInput = inputMap.moveInputName;
            jumpInput = inputMap.jumpInputName;
            tagInput = inputMap.tagInputName;
        }
        else
        {
            moveInput = "M_PC_Horizontal";
            jumpInput = "M_PC_Jump";
            tagInput = "M_PC_Tag";
        }
        
    }
    #endregion
}
