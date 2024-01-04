using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class NetworkLister : NetworkBehaviour
{
    void Start()
    {
        if (!NetworkManager.IsHost) Destroy(this);
        display = GetComponent<TMP_Text>();
    }

    private TMP_Text display;
    
    void Update()
    {
        List<NetworkClient> networkClients = NetworkManager.Singleton.ConnectedClientsList.ToList();
        display.text = "";
        foreach (var VARIABLE in networkClients)
        {
            display.text += "\n" + VARIABLE.ClientId;
        }
    }
}
