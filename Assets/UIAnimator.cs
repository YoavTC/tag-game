using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimator : MonoBehaviour
{
    [SerializeField] private float UIAnimationSpeed;

    [SerializeField] private SerializedDictionary<Transform, Transform[]> ConnectRoomUIElements = new SerializedDictionary<Transform, Transform[]>();
    
    public void AnimateConnectUIOut()
    {
        foreach (var UIElement in ConnectRoomUIElements)
        {
            if (UIElement.Key.GetComponent<Selectable>()) UIElement.Key.GetComponent<Selectable>().interactable = false;
            UIElement.Key.DOMoveY(UIElement.Value[1].position.y, UIAnimationSpeed);
        }
    }

    public void AnimateStartUIIn()
    {
        foreach (var UIElement in ConnectRoomUIElements)
        {
            if (UIElement.Key.GetComponent<Selectable>()) UIElement.Key.GetComponent<Selectable>().interactable = true;
            UIElement.Key.DOMoveY(UIElement.Value[0].position.y, UIAnimationSpeed);
        }
    }
}