using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetworkModeCommunicator
{
    private static bool _isGameLocal;
    private static bool _isKeyboard;

    public static bool isGameLocal
    {
        set { _isGameLocal = value; }
        get
        {
            Debug.Log("Changed network game state to: " + _isGameLocal);
            return _isGameLocal;
        }
    }
    public static bool isKeyboard
    {
        set { _isKeyboard = value; }
        get
        {
            Debug.Log("Changed Input game type to: " + _isKeyboard);
            return _isKeyboard;
        }
    }
}
