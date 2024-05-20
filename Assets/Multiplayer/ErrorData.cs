using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Error Data", menuName = "Error Data")]
public class ErrorData : ScriptableObject
{
    public bool hasError;
    public int errorCode;
    public string errorMessage;
}
