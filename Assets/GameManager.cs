using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : NetworkSingleton<GameManager>
{
    [SerializeField] private GameData gameData;
    
    [Header("Objects")]
    [SerializeField] public CameraMovement clientCamera;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private UIAnimator startGameAnimator;
    [SerializeField] private TimerHandler timerHandler;
    
    [Header("Network Variables")]
    public bool isLocalGame;
    [SerializeField] public NetworkVariable<GameState> currentNetworkGameState = new NetworkVariable<GameState>(GameState.PRE);
    [SerializeField] private NetworkVariable<ulong> taggedPlayerClientID = new NetworkVariable<ulong>(57);

    private List<ulong> deadClientIDs = new List<ulong>();
    
    [SerializeField] private GameState currentLocalGameState;
    [SerializeField] private Transform taggedPlayerTransform;

    public Dictionary<ulong, Transform> localClientTransforms = new Dictionary<ulong, Transform>();
    
    void Start()
    {
        //Check if game is local or not
        isLocalGame = !gameData.isOnline;
        
        //If game is local, set up local play game state. Else, set up network game state
        if (isLocalGame) ChangeLocalGameState(GameState.PRE);
        else currentNetworkGameState.Value = GameState.PRE;

        //StartGame();
    }
    
    public void StartGame()
    {
        Debug.Log("??????????????");
        if (isLocalGame)
        {
            ChangeLocalGameState(GameState.STARTING);
        }
        else
        {
            if (IsHost) {
                ChangeLocalGameStateClientRpc(GameState.STARTING);
            
                //Tag random player
                List<ulong> connectedClients = NetworkManager.Singleton.ConnectedClientsIds.ToList();
                foreach (var deadClient in deadClientIDs)
                {
                    connectedClients.Remove(deadClient);
                }
                int randomClientIDIndex = Random.Range(0, connectedClients.Count);
                ulong randomClientID = connectedClients[randomClientIDIndex];
                taggedPlayerClientID.Value = randomClientID;
            
                TagRandomPlayerClientRpc(randomClientID);
            } Debug.Log("Host?: " + IsHost);
        }
    }
    
    [ClientRpc]
    public void ChangeLocalGameStateClientRpc(GameState newGameState)
    {
        if (isLocalGame) return;
        Debug.Log("ChangedLocalGameState to " + newGameState + " - " + OwnerClientId);
        switch (newGameState)
        {
            case GameState.PRE:
                break;
            case GameState.STARTING:
                startGameAnimator.AnimateConnectUIOut();
                clientCamera.SetFollowPlayerState(true);
                TitleSystem.Instance.DisplayText("Game Started!", true, "#5AD32C");
                //Start Game
                PlayerController[] players = FindObjectsOfType<PlayerController>();
                for (int i = 0; i < players.Length; i++)
                {
                    localClientTransforms.Add(players[i].OwnerClientId ,players[i].transform);
                    SpawnManager.Instance.SetSpawnPoint(players[i].transform, false);
                }
                
                //Start timer
                timerHandler.StartTimer();
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
                //spawn players
                Instantiate(playerPrefab, transform.position, Quaternion.identity);
                GameObject secondPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity);
                secondPlayer.GetComponent<SpriteSwapper>().SetColor(1);

                CameraMovement tempCam = GameObject.FindWithTag("TempCamera").AddComponent<CameraMovement>();
                tempCam.MoveToPosition(new Vector3(-1.5f,16,-10));
                tempCam.GetComponent<Camera>().orthographicSize = 22.5f;
                startGameAnimator.AnimateConnectUIOut();
                TitleSystem.Instance.DisplayText("Game Started!", true, "#5AD32C");
                
                //Tag random player
                TagRandomPlayer();
                
                //Start timer
                timerHandler.StartTimer();
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
        PlayerController[] filteredPlayers = allPlayers.Where(player => !player.GetComponent<PlayerController>().isDead).ToArray();
        PlayerController randomPlayer = filteredPlayers[Random.Range(0, allPlayers.Length)];
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
        
        //Update client about tagging change
        TaggerDisplay.Instance.SetNewTagger(localClientTransforms[taggedID]);
    }
    public void ReceiveTagChange(Transform taggedTransform, Transform taggerTransform)
    {
        Debug.Log(taggedTransform + " TAGGED " + taggerTransform, taggerTransform);
        taggedTransform.GetComponent<PlayerController>().GetTagged();
        
        //Update about tagging change
        TaggerDisplay.Instance.SetNewTagger(taggedTransform);
    }

    //Runs if the gamemode type is eliminations, once the timer reaches 0 
    [ClientRpc]
    public void KillTaggedPlayerClientRpc(ulong clientID)
    {
        deadClientIDs.Add(clientID);
        Debug.Log("Timer reached 0, killing tagged player!");
        taggedPlayerTransform.GetComponent<PlayerController>().KillPlayer();
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
