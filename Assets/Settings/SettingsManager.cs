using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Settings Variables")]
    //Saved preferences
    [SerializeField] private string username;
    [SerializeField] private string color;

    //Saved settings
    
    //Window modes:
    //1. Windowed
    //2. Fullscreen
    //3. Fullscreen Windowed
    [SerializeField] private int windowMode;
    [SerializeField] private float volume;
    [SerializeField] private float mouseSens;

    //Saved binds
    [SerializeField] private string wBind, aBind, sBind, dBind;
    [SerializeField] private string tagBind;

    [Header("UI Objects")] 
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private Dropdown colorDropdown, windowModeDropdown;
    [SerializeField] private Slider volumeSlider, mouseSensSlider;
    
    //TODO 
    //Add input fields and think of way to get keybinds
    

    private void LoadSettings()
    {
        username = PlayerPrefs.GetString("username");
        color = PlayerPrefs.GetString("color");
        windowMode = PlayerPrefs.GetInt("windowMode");
        volume = PlayerPrefs.GetFloat("volume");
        mouseSens = PlayerPrefs.GetFloat("mouseSense");
            
        wBind = PlayerPrefs.GetString("wBind");
        aBind = PlayerPrefs.GetString("aBind");
        sBind = PlayerPrefs.GetString("sBind");
        dBind = PlayerPrefs.GetString("dBind");
            
        tagBind = PlayerPrefs.GetString("tagBind");
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetString("username",username);
        PlayerPrefs.SetString("color", color);
        PlayerPrefs.SetInt("windowMode", windowMode);
        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("mouseSense", mouseSens);
        
        PlayerPrefs.SetString("wBind", wBind);
        PlayerPrefs.SetString("aBind", aBind);
        PlayerPrefs.SetString("sBind", sBind);
        PlayerPrefs.SetString("dBind", dBind);
        
        PlayerPrefs.SetString("tagBind", tagBind);
    }


    public void OnUsernameChange() {}
    
    public void OnColorChange() {}
    
    public void OnWindowModeChange() {}
    
    public void OnVolumeChange() {}
    
    public void OnMouseSensitivityChange() {}
}
