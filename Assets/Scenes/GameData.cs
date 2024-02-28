using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Data")]
public class GameData : ScriptableObject
{
    public bool isOnline;
    public int gameModeType; 
    //Timeless 0
    //Single 1
    //Elimination 2

    public float speedMultiplier, jumpMultiplier, taggerSpeedMultiplier, tagStunDuration, doubleJumps, eliminationTime;
}
