using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CapitalizeInput : MonoBehaviour
{
    private TMP_InputField inputField;

    private void Start() => inputField = GetComponent<TMP_InputField>();
    
    public void OnValueChange()
    {
        inputField.text = inputField.text.ToUpper();
    }
}
