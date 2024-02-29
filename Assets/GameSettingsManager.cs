using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsManager : Singleton<GameSettingsManager>
{
    [SerializeField] private GameData gameData;
    
    private void Start()
    {
        SetUpSettings();
    }

    private void SetUpSettings()
    {
        
    }

    public GameData GetGameSettings()
    {
        GameData tempGameData = gameData;
        return tempGameData;
    }
}
