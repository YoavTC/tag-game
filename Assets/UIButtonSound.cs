using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    [SerializeField] private AudioClip hoverEnter, hoverExit, click;
    [SerializeField] private SoundOptions soundOptions = new (0.2f, 1f, 0f, false, false, new Vector2(0.75f, 1.25f));
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverEnter != null) AudioManager.Instance.PlaySound(hoverEnter, soundOptions);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverExit != null) AudioManager.Instance.PlaySound(hoverExit, soundOptions);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (click != null) AudioManager.Instance.PlaySound(click, soundOptions);
    }

    [Button]
    public void ResetSettings()
    {
        soundOptions = new SoundOptions(0.2f, 1f, 0f, false, false, new Vector2(0.75f, 1.25f));
    }
}
