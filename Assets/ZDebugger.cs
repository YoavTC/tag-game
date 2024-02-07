using DG.Tweening;
using TMPro;
using UnityEngine;

public class ZDebugger : MonoBehaviour
{
    private TMP_Text text;
    private Transform parent;
    
    void Start()
    {
        text = GetComponent<TMP_Text>();
        parent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Z: " + transform.position.z;
        parent.position = new Vector3(parent.position.x, parent.position.y, -5f);
    }
}
