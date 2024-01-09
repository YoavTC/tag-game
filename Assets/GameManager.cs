using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkSingleton<GameManager>
{
    [SerializeField] private UIAnimator startGameAnimator;
    
    [Header("Objects")]
    [SerializeField] public CameraMovement clientCamera;
    
    [Header("Network Variables")]
    [SerializeField] private NetworkVariable<GameState> currentNetworkGameState = new NetworkVariable<GameState>(GameState.PRE);
    [SerializeField] private NetworkVariable<ulong> taggedPlayerClientID = new NetworkVariable<ulong>(57);
    
    
    void Start()
    {
        currentNetworkGameState.Value = GameState.PRE;
    }
    
    
    public void StartGame()
    {
        if (!IsHost)
        {
            Debug.Log("Only the host can start the game!"); //display text on screen
        } else
        {
            ChangeLocalGameStateClientRpc(GameState.STARTING);
            currentNetworkGameState.Value = GameState.STARTING;
            
            //Tag random player
            ulong[] connectedClients = NetworkManager.Singleton.ConnectedClientsIds.ToArray();
            int randomClientIDIndex = Random.Range(0, connectedClients.Length);
            ulong randomClientID = connectedClients[randomClientIDIndex];
            taggedPlayerClientID.Value = randomClientID;
            
            TagRandomPlayerClientRpc(randomClientID);
        }
    }
    
    [ClientRpc]
    private void TagRandomPlayerClientRpc(ulong randomClientID)
    {
        Debug.Log("I am " + OwnerClientId + " and the random client id is: " + randomClientID);
        if (OwnerClientId == randomClientID)
        {
            clientCamera.playerTransform.GetComponent<PlayerController>().GetTagged();
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
                }
                break;
            case GameState.ACTIVE:
                break;
            case GameState.PAUSED:
                break;
            case GameState.POST:
                break;
        }
        
        if (IsHost) currentNetworkGameState.Value = newGameState;
    }
    
    
    //Runs on server when client tags client
    [ServerRpc(RequireOwnership = false)]
    public void ClientTagClientServerRpc(ulong taggedID, ulong taggerID)
    {
        taggedPlayerClientID.Value = taggedID;
        ReceiveTagChangeClientRpc(taggedID, taggerID);
    }

    //Runs on every client upon tag change
    [ClientRpc]
    public void ReceiveTagChangeClientRpc(ulong taggedID, ulong taggerID)
    {
        Debug.Log(taggerID + " TAGGED " + taggedID);
        if (taggedID == OwnerClientId)
        {
            clientCamera.playerTransform.GetComponent<PlayerController>().GetTagged();
        }
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
