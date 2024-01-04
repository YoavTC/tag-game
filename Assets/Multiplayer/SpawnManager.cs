using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private Transform f_spawnPoint, s_spawnPoint;
    private bool spawnFirst = true;

    public void SetSpawnPoint(Transform playerTransform)
    {
        if (spawnFirst)
        {
            playerTransform.position = f_spawnPoint.position;
            spawnFirst = false;
        } else playerTransform.position = s_spawnPoint.position;
    }
}
