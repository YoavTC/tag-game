using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Data")]
public class GameData : ScriptableObject, INetworkSerializable
{
    public bool isOnline;
    public int gameModeType; 
    //Timeless 0
    //Single 1
    //Elimination 2

    public float speedMultiplier, jumpMultiplier, taggerSpeedMultiplier, tagStunDuration, doubleJumps, eliminationTime;

    public bool isHost;
    public string joinCode;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref isOnline);
        serializer.SerializeValue(ref gameModeType);
        serializer.SerializeValue(ref isHost);
        serializer.SerializeValue(ref joinCode);
        
        serializer.SerializeValue(ref speedMultiplier);
        serializer.SerializeValue(ref jumpMultiplier);
        serializer.SerializeValue(ref taggerSpeedMultiplier);
        serializer.SerializeValue(ref tagStunDuration);
        serializer.SerializeValue(ref doubleJumps);
        serializer.SerializeValue(ref eliminationTime);
    }
}
