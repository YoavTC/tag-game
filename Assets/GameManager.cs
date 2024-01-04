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
    
    //TODO
    //Set up correct tagged player syncing and display for tagged player
    
    void Start()
    {
        currentNetworkGameState.Value = GameState.PRE;
    }
    
    
    public void StartGame()
    {
        if (!IsHost)
        {
            Debug.Log("Only the host can start the game!"); //display text on screen
        } else {
            
            ChangeLocalGameStateClientRpc(GameState.STARTING);
            currentNetworkGameState.Value = GameState.STARTING;
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
    
    
    
    [ServerRpc]
    public void TagPlayerServerRpc(ulong taggedID, ulong taggerID)
    {
        taggedPlayerClientID.Value = taggedID;
        ReceiveTagChangeClientRpc(taggedID, taggerID);
        Debug.Log(taggerID + " tagged " + taggedID);
        
    }

    [ClientRpc]
    public void ReceiveTagChangeClientRpc(ulong taggedID, ulong taggerID)
    {
        if (OwnerClientId == taggedID)
        {
            //Tell player they got tagged
            Debug.Log("you got tagged by " + taggerID);
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
