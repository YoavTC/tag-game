using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ListPlayers : MonoBehaviour
{
    private TMP_Text display;
    private List<NetworkClient> cachedNetworkClients;

    private void Start()
    {
        display = GetComponent<TMP_Text>();
    }

    void Update()
    {
        IReadOnlyList<NetworkClient> networkClients = NetworkManager.Singleton.ConnectedClientsList.ToList();
        if (cachedNetworkClients == null || networkClients.Count != cachedNetworkClients.Count)
        {
            cachedNetworkClients = networkClients.ToList();
        }

        if (cachedNetworkClients != null)
        {
            display.text = "";
            foreach (var VARIABLE in cachedNetworkClients)
            {
                display.text += "\n" + VARIABLE.ClientId;
            } 
        }
    }
}
