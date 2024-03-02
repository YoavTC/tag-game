using System;
using Newtonsoft.Json;
using Unity.Netcode;
using UnityEngine;

public class GameSettingsManager : NetworkSingleton<GameSettingsManager>
{
    //[SerializeField] private GameData _gameData;
    public GameData gameData => _gameData.Value;

    [SerializeField] private NetworkVariable<GameData> _gameData = new NetworkVariable<GameData>();

    //private string SerializeGameData(GameData sourceGameData) => JsonConvert.SerializeObject(sourceGameData);
    //private GameData DeserializeGameData(string rawGameData) => JsonConvert.DeserializeObject<GameData>(rawGameData);
}
