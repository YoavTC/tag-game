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
        GameObject mapPrefab = mapList.mapInstances.FirstOrDefault(instance => instance.mapID == _gameData.Value.map).mapPrefab;
        if (mapPrefab != null)
        {
            Instantiate(mapPrefab);
        }
        else Instantiate(mapList.mapInstances[0].mapPrefab);
    }
}
