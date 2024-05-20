using System;
using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private bool testingMode;
    private bool isLocalGame;
    
    [Header("Camera")] 
    [SerializeField] private GameObject CameraPrefab;
    private CameraMovement clientCamera;

    [Header("Movement Settings")] 
    [SerializeField] private float moveSpeed;
    private float taggerMoveSpeed;
    [SerializeField] private float slamSpeed;
    [SerializeField] private float jumpForce;
    private Rigidbody2D rb;

    [Header("Tagging Settings")] 
    [SerializeField] private bool isTagger;
    [SerializeField] private bool canTag = true;
    [SerializeField] private float taggingRadius;
    [SerializeField] private float tagCooldown;
    private NetworkVariable<bool> isTaggerNetwork = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
    [Header("Key Binds")] 
    private string moveInput = "M_PC_Horizontal";
    private string jumpInput = "L_PC1_Jump";
    private string tagInput = "L_PC1_Tag";
    private string slamInput = "L_PC1_Slam";
    
    private LayerMask playerLayer;
    private bool isOnGround;
    private int doubleJumps;
    private int extraJumps = 1;

    public bool isDead;
    
    [Header("Particles")]
    [SerializeField] private GameObject tagParticle;
    [SerializeField] private GameObject jumpParticle;

    [Header("Audio")] 
    [SerializeField] private AudioClip jumpClips;
    [SerializeField] private AudioClip explodeClip;

    private void Start()
    {
        isLocalGame = GameManager.Instance.isLocalGame;
        
        SetupBindings();
        SetupWaistband();
        StartCoroutine(SetupGameSettings());
        
        //Delete other client's controller
        // if (!isLocalGame && !IsOwner && !testingMode) Destroy(this);
        
        //Set up variables
        rb = GetComponent<Rigidbody2D>();
        playerLayer = LayerMask.GetMask("Player");
        

        //Spawn camera
        if (!isLocalGame && IsOwner)
        {
            clientCamera = Instantiate(CameraPrefab).GetComponent<CameraMovement>();
            clientCamera.InitiateCameraSettings(transform);
        }
        
        if (!isLocalGame)
        {
            transform.position = new Vector3(0, 80, 0);
            //SpawnManager.Instance.SetSpawnPoint(transform, true);
        }
        
        //Fade out black screen
        IntroCanvas.Instance.StartIntro();
        
        transform.GetChild(0).GetComponent<TMP_Text>().text = OwnerClientId.ToString();
    }
    
    void Update()
    {
        if (!isLocalGame && !IsOwner && !testingMode) return;
        
        //Get input movement
        float horizontalDistance = Input.GetAxis(moveInput);
        Vector2 movement = new Vector2(horizontalDistance, 0);

        //Apply movement velocity to the player's rigidbody
        rb.velocity = new Vector2(movement.x * (isTagger ? taggerMoveSpeed : moveSpeed), rb.velocity.y);
        
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

        if (Input.GetButtonDown(slamInput))
        {
            rb.velocity = new Vector2(rb.velocity.x, slamSpeed);
        }
    }

    public void KillPlayer()
    {
        isDead = true;
        isTagger = false;
        canTag = false;
        clientCamera.transform.position = new Vector3(-1.5f, 16f, -10f);
        Destroy(gameObject);
    }

    #region Jumping & Ground
    //Jump logic
    private void Jump(bool isDouble)
    {
        if (isDouble) extraJumps--;
        else extraJumps = doubleJumps;

        if (!isOnGround) Instantiate(jumpParticle, transform);
        
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        
        //Play audio
        AudioManager.Instance.PlaySFX(jumpClips, true);
    }
    
    //Ground check detection
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != playerLayer)
        {
            isOnGround = true;
            extraJumps = doubleJumps;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer != playerLayer)
        {
            isOnGround = false;
        }
    }

    public void BoostPlayer(float boostStrength)
    {
        rb.velocity += new Vector2(0, boostStrength);
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
            
            //TitleSystem.Instance.DisplayText(isLocalGame ? "You tagged someone!!":"Tagged!!", true, "#4285f4");
            TitleSystem.Instance.DisplayText("Tagged!!", true, "#4285f4");
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
        TitleSystem.Instance.DisplayText("You got TAGGED!!", true, "#c33232");
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
            TitleSystem.Instance.DisplayText("You got TAGGED!!", true, "#c33232");
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
            if (playerControllers[i].isTaggerNetwork.Value)
            {
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

    #region Setup
    private void SetupBindings()
    {
        if (isLocalGame)
        {
            InputMap inputMap = GameManager.Instance.GetComponent<InputManager>().GetInputBindingMap();
            moveInput = inputMap.moveInputName;
            jumpInput = inputMap.jumpInputName;
            tagInput = inputMap.tagInputName;
            slamInput = inputMap.slamInputName;
        }
        else
        {
            moveInput = "M_PC_Horizontal";
            jumpInput = "M_PC_Jump";
            tagInput = "M_PC_Tag";
            slamInput = "M_PC_Slam";
        }
    }

    private void SetupWaistband()
    {
        if (isLocalGame) return;
        Debug.Log(OwnerClientId);
        GetComponent<SpriteSwapper>().SetColor((int) OwnerClientId);
    }

    private IEnumerator SetupGameSettings()
    {
        yield return HelperFunctions.GetWait(0.25f);
        
        Debug.Log("Game data received!");
        isDead = false;
        GameData gameData = GameSettingsManager.Instance.gameData;
        moveSpeed = gameData.speedMultiplier;
        jumpForce = gameData.jumpMultiplier;
        doubleJumps = (int) gameData.doubleJumps;
        taggerMoveSpeed = moveSpeed * gameData.taggerSpeedMultiplier;
        
        //Spawn map
        GameSettingsManager.Instance.SpawnMap();
    }
    #endregion
}
