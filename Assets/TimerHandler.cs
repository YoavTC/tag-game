using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TimerHandler : MonoBehaviour
{
    private int gameModeType;
    
    [SerializeField] private TMP_Text timerTextDisplay;
    [SerializeField] private int timer;
    private float timeElapsed;
    private bool timerActive;
    private float timeThreshold;
    private float timeToBeRemoved;
    private string stringFormat;

    public void StartTimer()
    {
        gameModeType = GameSettingsManager.Instance.gameData.gameModeType;

        switch (gameModeType)
        {
            case 0:
            {
                Debug.Log("Timeless");
                break;
            }
            case 1:
            {
                Debug.Log("Single");
                SetUpTimer();
                break;
            }
            case 2:
            {
                Debug.Log("Elimination");
                SetUpTimer();
                break;
            }
        }
    }

    private void SetUpTimer()
    {
        timer = (int) GameSettingsManager.Instance.gameData.eliminationTime * 10;
        
        timeToBeRemoved = 10f; // Increased by a factor of 10 to reflect the change in precision
        stringFormat = "F0"; // Change the format to display one decimal place
        timerActive = true;
        timeThreshold = 1f; // Default time threshold for integer timer
        
        timerTextDisplay.text = (timer / 10f).ToString(stringFormat);
        timerTextDisplay.transform.parent.gameObject.SetActive(true);
    }

    private void TimerEnded()
    {
        if (gameModeType == 1)
        {
            Debug.Log("Game over!!");
        }
        
        if (gameModeType == 2)
        {
            Debug.Log("Round over, starting again!");
            SetUpTimer();
        }
    }

    private void Update()
    {
        if (timerActive)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed >= timeThreshold)
            {
                timeElapsed = 0f;
                timer -= Mathf.FloorToInt(timeToBeRemoved);
                timerTextDisplay.text = (timer / 10f).ToString(stringFormat); // Divide by 10 when displaying
                
                if (timer < 50 && timer % 10 == 0)
                {
                    Debug.Log("Boop... " + timer);
                }
                
                if (timer <= 90) // Adjust threshold value accordingly
                {
                    if (timeToBeRemoved == 10f) timer = 100;
                    timeThreshold = 0.1f;
                    timeToBeRemoved = 1f;
                    stringFormat = "F1"; // Adjust the format to display two decimal places
                }

                if (timer <= 0)
                {
                    timerActive = false;
                    timer = 0;
                    timeElapsed = 0f;
                    timeToBeRemoved = 10f; // Reset time to be removed
                    stringFormat = "F0"; // Reset format to display one decimal place
                    TimerEnded();
                }
            }
        }
    }
}