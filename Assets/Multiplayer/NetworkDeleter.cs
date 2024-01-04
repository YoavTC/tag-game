using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class NetworkDeleter : NetworkBehaviour
{
    private Transform ownCamera;
    void Start()
    {
        // if (!IsOwner)
        // {
        //     Destroy(GetComponent<AudioListener>());
        // }
        //
        // ownCamera = GetComponent<PlayerMovement>().ownCamera;
        //
        // Camera[] cameras = FindObjectsOfType<Camera>();
        //
        // foreach (var tempCam in cameras)
        // {
        //     if (tempCam.transform != ownCamera)
        //     {
        //         Destroy(tempCam.gameObject);
        //     }
        // }

        // List<MonoBehaviour> components = GetComponents<MonoBehaviour>().ToList();
        // components.Remove(this);
        //
        // if (!IsOwner && !IsHost)
        // {
        //     foreach (MonoBehaviour component in components)
        //     {
        //         if (!(component is NetworkBehaviour))
        //         {
        //             Destroy(component);
        //         }
        //     } 
        // }
    }
}
