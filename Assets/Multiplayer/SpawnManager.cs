using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private Transform f_spawnPoint, s_spawnPoint;
    [SerializeField] private Transform preGameSpawnPoint;
    private bool spawnFirst = true;

    public void SetSpawnPoint(Transform playerTransform, bool lobby)
    {
        if (lobby)
        {
            playerTransform.position = preGameSpawnPoint.position;
            return;
        } 
        
        if (spawnFirst)
        {
            Debug.Log(playerTransform + " AT: " + f_spawnPoint, playerTransform);
            playerTransform.position = f_spawnPoint.position;
            spawnFirst = false;
        } else playerTransform.position = s_spawnPoint.position; Debug.Log(playerTransform + " AT: " + s_spawnPoint, playerTransform);
    }
}
