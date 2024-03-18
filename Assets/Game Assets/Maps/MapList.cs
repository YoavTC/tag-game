using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map List", menuName = "Maps/Map List")]
public class MapList : ScriptableObject
{
    public MapInstance[] mapInstances;
}