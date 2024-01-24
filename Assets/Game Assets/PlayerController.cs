using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Unity.Netcode;
using UnityEditor;
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
    private NetworkVariable<bool> isTaggerNetwork = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] private bool isTagger;
    [SerializeField] private bool canTag = true;
    [SerializeField] private float taggingRadius;
    [SerializeField] private float tagCooldown;
    [SerializeField] private GameObject tagParticle;

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
        //if (!isLocalGame && !IsOwner && !testingMode) Destroy(this);
        
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
        if (!isLocalGame && !IsOwner && !testingMode) return;
        
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

        if (Input.GetButtonDown(tagInput) && isTagger && canTag)
        {
            Tag();
            StartCoroutine(TagCooldown());
        }
    }
    
    //Draw tag radius
    //private void OnDrawGizmos() { Gizmos.color = Color.red; Gizmos.DrawWireSphere(transform.position, taggingRadius); }

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
        Transform taggedPlayer = GetClosestPlayer();
        Instantiate(tagParticle, transform.position, Quaternion.identity).transform.SetParent(transform);
        

        if (taggedPlayer != null)
        {
            isTagger = false;
            if (!isLocalGame) isTaggerNetwork.Value = false;
            
            if (isLocalGame) GameManager.Instance.ClientTagClient(taggedPlayer, transform);
            else
            {
                GameManager.Instance.ClientTagClientServerRpc(taggedPlayer.GetComponent<NetworkBehaviour>().OwnerClientId, OwnerClientId);
                //Set other
                TaggerDisplay.Instance.SetNewTagger(taggedPlayer);
            }
        }
    }

    private Transform GetClosestPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, taggingRadius, playerLayer);

        Transform closestTransform = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, collider.transform.position);

                if (distanceToPlayer < closestDistance)
                {
                    closestDistance = distanceToPlayer;
                    closestTransform = collider.transform;
                }
            }
        }

        return closestTransform;
    }

    private bool firstTime = true;
    
    public void GetTagged()
    {
        //Set self
        TaggerDisplay.Instance.SetNewTagger(transform);
        isTagger = true;
        if (!isLocalGame) isTaggerNetwork.Value = true;
        Debug.Log("You got tagged!");
    }

    public void GetTaggedClient(ulong taggedID)
    {
        if (OwnerClientId == taggedID)
        {
            //Set self
            TaggerDisplay.Instance.SetNewTagger(transform);
            isTagger = true;
            if (!isLocalGame) isTaggerNetwork.Value = true;
            Debug.Log("You got tagged!");
        }
        else if (firstTime) {
            //Set other
            firstTime = false;
            StartCoroutine(DelaySetPart());
        }
    }

    private IEnumerator DelaySetPart()
    {
        yield return HelperFunctions.GetWait(0.25f);
        PlayerController[] playerControllers = FindObjectsOfType<PlayerController>();
        for (int i = 0; i < playerControllers.Length; i++)
        {
            Debug.Log("Looping...");
            if (playerControllers[i].isTaggerNetwork.Value)
            {
                Debug.Log("Found:", playerControllers[i].transform);
                TaggerDisplay.Instance.SetNewTagger(playerControllers[i].transform);
            }
        }
    }

    private IEnumerator TagCooldown()
    {
        canTag = false;
        yield return HelperFunctions.GetWait(tagCooldown);
        canTag = true;
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
