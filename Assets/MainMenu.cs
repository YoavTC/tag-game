using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string mainScene, settingsScene, creditsScene;

    public void HoverOverButton()
    {
        //play sound
    }
    
    public void OnPressPlay()
    {
        SceneManager.LoadScene(mainScene);
    }
    
    public void OnPressSettings()
    {
        SceneManager.LoadScene(settingsScene);
    }
    
    public void OnPressCredits()
    {
        SceneManager.LoadScene(creditsScene);
    }
}
