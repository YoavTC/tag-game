using System.Linq;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : NetworkSingleton<GameManager>
{
    [SerializeField] private UIAnimator startGameAnimator;
    
    [Header("Objects")]
    [SerializeField] public CameraMovement clientCamera;
    [SerializeField] private GameObject playerPrefab;
    
    [Header("Network Variables")]
    public bool isLocalGame;
    [SerializeField] public NetworkVariable<GameState> currentNetworkGameState = new NetworkVariable<GameState>(GameState.PRE);
    [SerializeField] private NetworkVariable<ulong> taggedPlayerClientID = new NetworkVariable<ulong>(57);
    
    [SerializeField] private GameState currentLocalGameState;
    [SerializeField] private Transform taggedPlayerTransform;
    
    
    void Start()
    {
        //Check if game is local or not
        isLocalGame = NetworkModeCommunicator.isGameLocal;
        
        //If game is local, set up local play game state. Else, set up network game state
        if (isLocalGame) currentLocalGameState = GameState.PRE;
        else currentNetworkGameState.Value = GameState.PRE;
    }
    public void StartGame()
    {
        if (isLocalGame)
        {
            ChangeLocalGameState(GameState.STARTING);
            TagRandomPlayer();
        }
        else
        {
            if (IsHost) {
                ChangeLocalGameStateClientRpc(GameState.STARTING);
            
                //Tag random player
                ulong[] connectedClients = NetworkManager.Singleton.ConnectedClientsIds.ToArray();
                int randomClientIDIndex = Random.Range(0, connectedClients.Length);
                ulong randomClientID = connectedClients[randomClientIDIndex];
                taggedPlayerClientID.Value = randomClientID;
            
                TagRandomPlayerClientRpc(randomClientID);
            } 
        }
    }
    
    [ClientRpc]
    public void ChangeLocalGameStateClientRpc(GameState newGameState)
    {
        GameState currentGameState = currentNetworkGameState.Value;
        Debug.Log("ChangedLocalGameState to " + newGameState + " - " + OwnerClientId);
        switch (newGameState)
        {
            case GameState.PRE:
                break;
            case GameState.STARTING:
                if (currentGameState == GameState.PRE)
                {
                    startGameAnimator.AnimateConnectUIOut();
                    clientCamera.SetFollowPlayerState(true);
                    TitleSystem.Instance.DisplayText("Game Started!", true, "#5AD32C");
                    //Start Game
                    PlayerController[] players = FindObjectsOfType<PlayerController>();
                    for (int i = 0; i < players.Length; i++)
                    {
                        SpawnManager.Instance.SetSpawnPoint(players[i].transform, false);
                    }
                }
                break;
            case GameState.ACTIVE:
                break;
            case GameState.PAUSED:
                break;
            case GameState.POST:
                break;
        }
        
        if (IsHost)
        {
            currentNetworkGameState.Value = newGameState;
            currentLocalGameState = newGameState;
        }
    }
    public void ChangeLocalGameState(GameState newGameState)
    {
        switch (newGameState)
        {
            case GameState.PRE:
                break;
            case GameState.STARTING:
                if (currentLocalGameState == GameState.PRE)
                {
                    //spawn players
                    Instantiate(playerPrefab, transform.position, Quaternion.identity);
                    Instantiate(playerPrefab, transform.position, Quaternion.identity);

                    var tempCam = GameObject.FindWithTag("TempCamera").AddComponent<CameraMovement>();
                    tempCam.GetComponent<CameraMovement>().MoveToPosition(new Vector3(-1.5f,16,-10));
                    tempCam.GetComponent<Camera>().orthographicSize = 22.5f;
                    startGameAnimator.AnimateConnectUIOut();
                    TitleSystem.Instance.DisplayText("Game Started!", true, "#5AD32C");
                    
                    //Start game
                    PlayerController[] players = FindObjectsOfType<PlayerController>();
                    for (int i = 0; i < players.Length; i++)
                    {
                        SpawnManager.Instance.SetSpawnPoint(players[i].transform, false);
                    }
                }
                break;
            case GameState.ACTIVE:
                break;
            case GameState.PAUSED:
                break;
            case GameState.POST:
                break;
        }

        currentLocalGameState = newGameState;
    }
    
    [ClientRpc]
    private void TagRandomPlayerClientRpc(ulong randomClientID)
    {
        clientCamera.playerTransform.GetComponent<PlayerController>().GetTaggedClient(randomClientID);
    }
    private void TagRandomPlayer()
    {
        PlayerController[] allPlayers = FindObjectsOfType<PlayerController>();
        PlayerController randomPlayer = allPlayers[Random.Range(0, allPlayers.Length)];
        randomPlayer.GetTagged();
    }
    
    //Runs on server when client tags client
    [ServerRpc(RequireOwnership = false)]
    public void ClientTagClientServerRpc(ulong taggedID, ulong taggerID)
    {
        taggedPlayerClientID.Value = taggedID;
        ReceiveTagChangeClientRpc(taggedID, taggerID);
    }
    public void ClientTagClient(Transform taggedTransform, Transform taggerTransform)
    {
        taggedPlayerTransform = taggedTransform;
        ReceiveTagChange(taggedTransform, taggerTransform);
    }

    //Runs on every client upon tag change
    [ClientRpc]
    public void ReceiveTagChangeClientRpc(ulong taggedID, ulong taggerID )
    {
        Debug.Log(taggerID + " TAGGED " + taggedID);
        clientCamera.playerTransform.GetComponent<PlayerController>().GetTaggedClient(taggedID);
    }
    public void ReceiveTagChange(Transform taggedTransform, Transform taggerTransform)
    {
        Debug.Log(taggedTransform + " TAGGED " + taggerTransform, taggerTransform);
        taggedTransform.GetComponent<PlayerController>().GetTagged();
    }
}

public enum GameState
{
    PRE, //when players are joining the lobby
    STARTING, //the animation and countdown before the game starts
    ACTIVE, //when the game is played played
    PAUSED, //when the game is paused
    POST //after the game is finished
}
