using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] private bool visualChanges = false;

    private void Start()
    {
        HideTabsContent(transform);
    }

    public void ClickedTab(Transform tabTransform)
    {
        HideTabsContent(tabTransform);

        if (visualChanges)
        {
            tabTransform.GetChild(0).GetComponent<TMP_Text>().color = selectedColor;
            tabTransform.GetComponent<Image>().sprite = enabledSprite; 
        }
        
        GameObject tabContent = tabsDictionary[tabTransform].gameObject;
        tabContent.SetActive(true);
    }
    
    private void HideTabsContent(Transform tabTransform)
    {
        foreach (var tab in tabsDictionary)
        {
            if (tab.Value != tabTransform)
            {
                if (visualChanges)
                {
                    tab.Key.GetComponent<Image>().sprite = disabledSprite;
                    tab.Key.GetChild(0).GetComponent<TMP_Text>().color = defaultColor;
                }
                tab.Value.gameObject.SetActive(false);
            }
        }
    }
}
