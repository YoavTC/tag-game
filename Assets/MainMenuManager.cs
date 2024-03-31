using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameData gameDataSettings;

    private void Start()
    {
        //Reset join code
        gameDataSettings.joinCode = "NULL";
    }
    
    #region Create
    [Header("Create UI Settings")]
    [SerializeField] private Toggle isOnlineToggle;
    [SerializeField] private Transform[] tabs = new Transform[3];
    [SerializeField] private Slider[] 
        speedSliders,
        jumpSliders,
        taggerSpeedSliders,
        tagStunSliders,
        doubleJumpSliders,
        eliminationTimeSliders;
    
    [SerializeField] private MapSelector mapSelector;
    
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
        gameDataSettings.isHost = true;
        gameDataSettings.joinCode = "NULL";
        
        gameDataSettings.speedMultiplier = speedSliders[tabIndex].value;
        gameDataSettings.jumpMultiplier = jumpSliders[tabIndex].value;
        gameDataSettings.taggerSpeedMultiplier = taggerSpeedSliders[tabIndex].value;
        gameDataSettings.tagStunDuration = tagStunSliders[tabIndex].value;
        gameDataSettings.doubleJumps = doubleJumpSliders[tabIndex].value;
        // gameDataSettings.map = mapSelector.maps.Keys.ToArray()[mapSelector.previewIndex]; 
        gameDataSettings.map = mapSelector.maps.mapInstances[mapSelector.previewIndex].mapID;
        if (tabIndex == 2) gameDataSettings.eliminationTime = eliminationTimeSliders[0].value;
        if (tabIndex == 1) gameDataSettings.eliminationTime = eliminationTimeSliders[1].value;
        //Move Scene
        SceneManager.LoadScene("GameScene");
    }
    #endregion

    [SerializeField] private TMP_InputField codeInputField;
    public void JoinGame()
    {
        gameDataSettings.isHost = false;
        gameDataSettings.joinCode = codeInputField.text;
        //Move Scene
        SceneManager.LoadScene("GameScene");
    }
}