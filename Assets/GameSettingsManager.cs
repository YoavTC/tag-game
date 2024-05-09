using System;
using System.Collections;
using System.Linq;
using Newtonsoft.Json;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class GameSettingsManager : NetworkSingleton<GameSettingsManager>
{
    //[SerializeField] private GameData _gameData;
    public GameData gameData => _gameData.Value;

    [SerializeField] private NetworkVariable<GameData> _gameData = new NetworkVariable<GameData>();

    //private string SerializeGameData(GameData sourceGameData) => JsonConvert.SerializeObject(sourceGameData);
    //private GameData DeserializeGameData(string rawGameData) => JsonConvert.DeserializeObject<GameData>(rawGameData);

    [SerializeField] private MapList mapList;
    private bool alreadySpawned = false;
    
    public void SpawnMap()
    {
        if (alreadySpawned) return;
        // if (GameManager.Instance.isLocalGame) return;
        Debug.Log("Received map " + _gameData.Value.map + ", trying to summon!");
        GameObject mapPrefab = mapList.mapInstances.FirstOrDefault(instance => instance.mapID == _gameData.Value.map).mapPrefab;
        if (mapPrefab != null)
        {
            Debug.Log("Map found, summoning " + mapPrefab.name + " now!");
            Instantiate(mapPrefab);
        }
        else
        {
            Debug.Log("Map NOT found, summoning default instead!");
            Instantiate(mapList.mapInstances[0].mapPrefab);
        }

        alreadySpawned = true;

        if (GameManager.Instance.isLocalGame)
        {
            StartCoroutine(MoveLocalPlayersToSpawnLocations());
        }
    }

    private IEnumerator MoveLocalPlayersToSpawnLocations()
    {
        yield return HelperFunctions.GetWait(0.25f);
        Debug.Log("MoveLocalPlayersToSpawnLocations()");
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        for (int i = 0; i < players.Length; i++)
        {
            SpawnManager.Instance.SetSpawnPoint(players[i].transform, false);
        }
    }
}
