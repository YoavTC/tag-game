using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class VersionController : MonoBehaviour
{
    private string URL = "https://raw.githubusercontent.com/YoavTC/information/main/tag-game-ver.json";
    private string response;

    [SerializeField] private int _currentVersionInt;
    public int currentVersionInt => _currentVersionInt;
    
    [SerializeField] private GameObject versionMismatchNotifier;
    
    void Start()
    {
        StartCoroutine(FetchVersion());
    }
    
    private int fetchedVersionInt;

    private IEnumerator FetchVersion()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Data fetched successfully from " + URL + ":");
                response = request.downloadHandler.text;

                JObject responseJObject = JObject.Parse(response);
                fetchedVersionInt = responseJObject["version"].Value<int>();
                
                Debug.Log("Version Int: " + fetchedVersionInt);
            }
            else
            {
                Debug.LogError("Error fetching data: " + request.error);
            }
        }

        if (_currentVersionInt != fetchedVersionInt)
        {
            Debug.Log("Version mismatch!");
            versionMismatchNotifier.SetActive(true);
        }
    }
}
