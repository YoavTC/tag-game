using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Visual Settings")] 
    [SerializeField] private Color selectedColor = Color.white, defaultColor = Color.black;
    [SerializeField] float shadowTransitionDuration = .2f;
    
    private TMP_Text buttonText;
    private Shadow buttonShadow;

    private void Start()
    {
        buttonText = transform.GetChild(0).GetComponent<TMP_Text>();
        buttonShadow = GetComponent<Shadow>();  
    }
    
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
        AnimateShadow();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AnimateShadow(true);
    }

    private void AnimateShadow(bool isOut = false)
    {
        buttonShadow.DOKill();
        
        Vector2 currentVector = buttonShadow.effectDistance;
        Vector2 endVector = isOut ? new Vector2(0, 0) : new Vector2(25, -25); 
        
        DOTween.To(() => currentVector, x => currentVector = x, endVector, shadowTransitionDuration)
            .OnUpdate(() =>
            {
                buttonShadow.effectDistance = currentVector;
            });
    }
}
