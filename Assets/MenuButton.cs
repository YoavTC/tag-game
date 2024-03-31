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
    [SerializeField] float shadowTransitionDuration = .2f;
    
    private Shadow buttonShadow;
    private Tabs tabsManager;

    private void Start()
    {
        buttonShadow = GetComponent<Shadow>();
        tabsManager = transform.parent.GetComponent<Tabs>();
    }
    
    // public void OnClick()
    // {
    //     buttonText.color = selectedColor;
    // }

    public void OnSelect(BaseEventData eventData)
    {
        tabsManager.ClickedTab(transform);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        
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
    
    public void OnPointerEnter(PointerEventData eventData) => AnimateShadow();
    public void OnPointerExit(PointerEventData eventData) => AnimateShadow(true);
}
