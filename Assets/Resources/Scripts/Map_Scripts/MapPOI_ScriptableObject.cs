using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Map POI", menuName = "Scriptable Objects/New Map POI")]
public class MapPOI_ScriptableObject : ScriptableObject
{
    [Header("Name")]
    public new string name;

    
    [Header("Map Data")]
    public Vector2 map_pos;
    public int poi_size;
    public int poi_difficulty;

}
