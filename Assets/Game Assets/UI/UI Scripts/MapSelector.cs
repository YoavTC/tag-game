using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

public class MapSelector : MonoBehaviour
{
    //[SerializeField] public SerializedDictionary<Maps, Sprite> maps = new SerializedDictionary<Maps, Sprite>();
    public MapList maps;
    private List<Sprite> mapSprites = new List<Sprite>();
    
    [SerializeField] private Image previewDisplay;

    private void Start()
    {
        //mapSprites = maps.Values.ToList();
        mapSprites = maps.mapInstances.Select(item => item.previewSprite).ToList();
        ChangeMap(true);
    }

    public int previewIndex;
    
    public void ChangeMap(bool isRight)
    {
        if (isRight)
        {
            previewIndex++;
            if (previewIndex >= mapSprites.Count) previewIndex = 0;
        }
        else
        {
            previewIndex--;
            if (previewIndex <= -1) previewIndex = mapSprites.Count - 1;
        }
        //previewIndex += isRight ? 1 : -1;
        previewDisplay.sprite = mapSprites[previewIndex];
    }
}

public enum Maps
{
    TG_CLASSIC,
    TG_WORKSITE,
    TG_GYMBOREE,
    TG_SPACESHIP
}
