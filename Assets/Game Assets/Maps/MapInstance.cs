using UnityEngine;

[CreateAssetMenu(fileName = "Map Instance", menuName = "Maps/Map Instance")]
public class MapInstance : ScriptableObject
{
    public Sprite previewSprite;
    public GameObject mapPrefab;
    public Maps mapID;
}