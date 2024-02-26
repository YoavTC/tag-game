using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tabs : MonoBehaviour
{
    [SerializeField] private Sprite enabledSprite, disabledSprite;
    [SerializeField] private Color selectedColor = Color.white, defaultColor = Color.black;
    
    [SerializeField] private SerializedDictionary<Transform, Transform>
        tabsDictionary = new SerializedDictionary<Transform, Transform>();

    public void ClickedTab(Transform tabTransform)
    {
        foreach (var tab in tabsDictionary)
        {
            if (tab.Value != tabTransform)
            {
                tab.Key.GetComponent<Image>().sprite = disabledSprite;
                tab.Key.GetChild(0).GetComponent<TMP_Text>().color = defaultColor;
                tab.Value.gameObject.SetActive(false);
            }
        }

        tabTransform.GetChild(0).GetComponent<TMP_Text>().color = selectedColor;
        tabTransform.GetComponent<Image>().sprite = enabledSprite;
        
        GameObject tabContent = tabsDictionary[tabTransform].gameObject;
        tabContent.SetActive(true);
    }
}
