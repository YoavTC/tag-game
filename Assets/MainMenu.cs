using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string mainScene, settingsScene, creditsScene, mainMenuScene;

    public void HoverOverButton()
    {
        //play sound
    }

    public void OnPressBack()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
    
    public void OnPressPlay()
    {
        NetworkModeCommunicator.isGameLocal = false;
        StartCoroutine(StartScene(mainScene));
        //SceneManager.LoadScene(mainScene);
    }

    public void OnPressPlayLocal()
    {
        NetworkModeCommunicator.isGameLocal = true;
        StartCoroutine(StartScene(mainScene));
        //SceneManager.LoadScene(mainScene);
    }
    
    public void OnPressSettings()
    {
        SceneManager.LoadScene(settingsScene);
    }
    
    public void OnPressCredits()
    {
        SceneManager.LoadScene(creditsScene);
    }

    private IEnumerator StartScene(string sceneName)
    {
        yield return HelperFunctions.GetWait(1f);
        SceneManager.LoadScene(sceneName);
    }
}
