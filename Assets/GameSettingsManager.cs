using System;
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
    private void Start()
    {
        //GameObject mapPrefab = mapList.mapInstances[]
            //[_gameData.Value.map];
        GameObject mapPrefab = mapList.mapInstances.FirstOrDefault(item => item.mapID == _gameData.Value.map)
            //Get map prefab from map enum from the maps list
            Instantiate(mapPrefab);
    }
}
