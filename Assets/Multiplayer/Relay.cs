using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Relay : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private TMP_Text codeDisplay;
    [SerializeField] private UIAnimator startUIAnimator;
    
    private string joinCode;

    private IEnumerator Start()
    {
        yield return HelperFunctions.GetWait(0.5f);
        InitializeGame();
    }

    async void InitializeGame()
    {
        if (GameManager.Instance.isLocalGame)
        {
            Debug.Log("Starting Local Game...");
            GameManager.Instance.ChangeLocalGameState(GameState.STARTING);
            //ConnectToRoom();
        }
        else
        {
            Debug.Log("Starting Online Game...");
            InitializationOptions hostOptions = new InitializationOptions().SetProfile("host");
            InitializationOptions clientOptions = new InitializationOptions().SetProfile("client");

            await UnityServices.InitializeAsync(hostOptions);

            AuthenticationService.Instance.SignedIn += () =>
            {
                Debug.Log("Signed in: " + AuthenticationService.Instance.PlayerId);
            };

            if (AuthenticationService.Instance.IsAuthorized)
            {
                Debug.Log("Authorized");
                AuthenticationService.Instance.SignOut();
                await UnityServices.InitializeAsync(clientOptions);
            }

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            if (gameData.isHost)
            {
                CreateRelay();
            } else JoinRelay();
        }
    }

    private void ConnectToRoom()
    {
        startUIAnimator.AnimateStartUIIn();
        GameManager.Instance.ChangeLocalGameStateClientRpc(GameState.PRE);
    }
    
    #region CreateRelay
    public async void CreateRelay()
    {
        if (GameManager.Instance.isLocalGame) return;
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(8);
            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4,
                (ushort) allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
                );

            NetworkManager.Singleton.StartHost();
            ConnectToRoom();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        
        ConnectToRoom();
        codeDisplay.text = joinCode;
    }
    #endregion

    #region JoinRelay
    public async void JoinRelay()
    {
        if (GameManager.Instance.isLocalGame) return;
        try
        {
            string inputJoinCode = gameData.joinCode;
            Debug.Log("Joining relay with: " + inputJoinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(inputJoinCode);
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                joinAllocation.RelayServer.IpV4,
                (ushort) joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes,
                joinAllocation.Key,
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData
                );
            
            NetworkManager.Singleton.StartClient();
            ConnectToRoom();
            
            codeDisplay.text = inputJoinCode;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    #endregion
}
