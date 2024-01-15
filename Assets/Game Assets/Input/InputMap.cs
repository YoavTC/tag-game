using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Input Map")]
public class InputMap : ScriptableObject
{
    public string jumpInputName;
    public string moveInputName;
    public string tagInputName;
}
