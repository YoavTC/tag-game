using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map List", menuName = "Maps/Map List")]
public class MapList : ScriptableObject
{
    public MapInstance[] mapInstances;
}

[CreateAssetMenu(fileName = "Map Instance", menuName = "Maps/Map Instance")]
public class MapInstance : ScriptableObject
{
    public Sprite previewSprite;
    public GameObject mapPrefab;
    public Maps mapID;
}