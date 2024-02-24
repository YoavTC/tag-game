using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
{
    [Header("Visual Settings")] 
    [SerializeField] private Color selectedColor = Color.white, defaultColor = Color.black;
    private TMP_Text buttonText;
    
    private void Start() => buttonText = transform.GetChild(0).GetComponent<TMP_Text>();
    
    // public void OnClick()
    // {
    //     buttonText.color = selectedColor;
    // }

    public void OnSelect(BaseEventData eventData)
    {
        buttonText.color = selectedColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        buttonText.color = defaultColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //play sound
    }
}
