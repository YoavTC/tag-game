using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class SpriteSwapper : MonoBehaviour
{
    [SerializeField] private List<Color> colors = new List<Color>();
    
    [SerializeField] private Sprite chaseSprite, runSprite;
    

    public void ChangeSprite(bool isChase)
    {
        GetComponent<SpriteRenderer>().sprite = isChase ? chaseSprite : runSprite;
    }
    

    public void SetColor(int colorIndex)
    {
        Material waistbandMat = new Material(GetComponent<SpriteRenderer>().material);
        waistbandMat.SetColor("_Color", colors[colorIndex]);
        GetComponent<SpriteRenderer>().material = waistbandMat;
    }
}