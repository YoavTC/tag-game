using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private SerializedDictionary<InputMapID, InputMap> inputMaps = new SerializedDictionary<InputMapID, InputMap>();
    private bool isPlayer2 = false;

    private void Start()
    {
        isPlayer2 = false;
    }

    public InputMap GetInputBindingMap()
    {
        //return player 1
        if (!isPlayer2)
        {
            isPlayer2 = true;
            return inputMaps[InputMapID.l_PC1];
        } 
        //return player 2
        return inputMaps[InputMapID.l_PC2];
    }
}

public enum InputMapID
{
    l_PC1, l_PC2
}
