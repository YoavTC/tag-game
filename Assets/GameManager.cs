using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkSingleton<GameManager>
{
    [SerializeField] private UIAnimator startGameAnimator;
    
    [Header("Objects")]
    [SerializeField] private CameraMovement mainCamera;
    [SerializeField] private Transform lobbyPosition;
    
    [Header("Settings")]
    [SerializeField] private GameState currentGameState;
    [SerializeField] private NetworkVariable<GameState> currentNetworkGameState = new NetworkVariable<GameState>(GameState.PRE);
    
    void Start()
    {
        currentNetworkGameState.Value = GameState.PRE;
        currentGameState = GameState.PRE;
    }
    
    public void StartGame()
    {
        if (!IsHost)
        {
            Debug.Log("Only the host can start the game!"); //display text on screen
        } else { 
            ChangeGameState(GameState.STARTING);
            currentNetworkGameState.Value = GameState.STARTING;
        }
    }

    public void ChangeGameState(GameState newGameState)
    {
        switch (newGameState)
        {
            case GameState.PRE:
                mainCamera.MoveToPosition(lobbyPosition.position);
                break;
            case GameState.STARTING:
                if (currentGameState == GameState.PRE)
                {
                    mainCamera.FollowPlayer(true);
                    startGameAnimator.AnimateConnectUIOut();
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
        currentGameState = newGameState;
        currentNetworkGameState.Value = newGameState;
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
