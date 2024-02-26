using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Tutorials.Core.Editor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderVisualizer : MonoBehaviour
{
    [SerializeField] private TMP_Text sliderVisualizer;
    private Slider slider;
    private string baseText;

    private void Start()
    {
        slider = GetComponent<Slider>();
        OnSliderValueChange();
    }

    public void OnSliderValueChange()
    {
        float value = (float) (Math.Truncate(slider.value * 100) / 100);
        if (baseText.IsNullOrEmpty())
        {
            baseText = string.Format(sliderVisualizer.text);
            OnSliderValueChange();
        }
        else
        {
            sliderVisualizer.text = baseText + value;
        }
        
    }
}
