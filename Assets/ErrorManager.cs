using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ErrorManager : MonoBehaviour
{
    [SerializeField] private ErrorData errorData;
    [SerializeField] private TMP_Text errorBody;

    public void DisplayError()
    {
        if (errorData.hasError)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            if (!IsCustomErrorMessages()) errorBody.text = "Error: " + errorData.errorMessage;
            errorData.hasError = false;
        }
        else
        {
            CleanErrorData();
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    
    void Start()
    {
        DisplayError();
    }

    private bool IsCustomErrorMessages()
    {
        switch (errorData.errorCode)
        {
            case -2147467261: //Null value
                errorBody.text = "Invalid room code!";
                return true;
            case -2146233088: //Can't find room
                errorBody.text = "Can't find the room!";
                return true;
            case 0: //No internet connection
                errorBody.text = "Can't connect to the internet!\n Some things may not work!";
                return true;
        }

        return false;
    }

    private void CleanErrorData()
    {
        errorData.hasError = false;
        errorData.errorMessage = "";
        errorData.errorCode = 0;
    }
}
