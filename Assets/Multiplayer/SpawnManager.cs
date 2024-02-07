using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    private int spawnPointIndex = 1;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints.Add(transform.GetChild(i));
        }
    }

    public void SetSpawnPoint(Transform playerTransform, bool lobby)
    {
        if (lobby)
        {
            playerTransform.position = spawnPoints[0].position;
        } else
        {
            playerTransform.position = spawnPoints[spawnPointIndex].position;
            spawnPointIndex++;
        }

        // playerTransform.DOMoveZ(-5, 5f).OnComplete(() =>
        // {
        //     playerTransform.GetComponent<ClientNetworkTransform>().SyncPositionZ = false;
        // });
    }
}
