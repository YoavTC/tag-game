using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameData gameDataSettings;

    [Header("UI Settings")]
    [SerializeField] private Toggle isOnlineToggle;
    [SerializeField] private Transform[] tabs = new Transform[3];
    [SerializeField] private Slider[] 
        speedSliders,
        jumpSliders,
        taggerSpeedSliders,
        tagStunSliders,
        doubleJumpSliders,
        eliminationTimeSliders;
    
    
    public void StartGame()
    {
        Transform activeTab = null;
        foreach (var tab in tabs)
        {
            if (tab.gameObject.activeInHierarchy) activeTab = tab;
        }
        int tabIndex = Array.IndexOf(tabs, activeTab);
        
        Debug.Log("Index of: " + tabIndex);

        gameDataSettings.gameModeType = tabIndex;
        gameDataSettings.isOnline = isOnlineToggle.isOn;
        
        gameDataSettings.speedMultiplier = speedSliders[tabIndex].value;
        gameDataSettings.jumpMultiplier = jumpSliders[tabIndex].value;
        gameDataSettings.taggerSpeedMultiplier = taggerSpeedSliders[tabIndex].value;
        //gameDataSettings.tagStunDuration = tagStunSliders[tabIndex].value;
        //gameDataSettings.doubleJumps = doubleJumpSliders[tabIndex].value;
        if (tabIndex == 2) gameDataSettings.eliminationTime = eliminationTimeSliders[0].value;
    }
}

