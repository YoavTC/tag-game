using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class SpriteSwapper : MonoBehaviour
{
    [SerializeField] private List<Color> colors = new List<Color>();
    
    public void SetColor(int colorIndex)
    {
        Material waistbandMat = new Material(GetComponent<SpriteRenderer>().material);
        waistbandMat.SetColor("_Color", colors[colorIndex]);
        GetComponent<SpriteRenderer>().material = waistbandMat;
    }
}