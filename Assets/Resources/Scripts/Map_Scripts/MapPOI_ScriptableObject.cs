using UnityEngine;


[CreateAssetMenu(fileName = "New Map POI", menuName = "Scriptable Objects/New Map POI")]
public class MapPOI_ScriptableObject : ScriptableObject
{
    [Header("Name")]
    public new string name;

    
    [Header("Map Data")]
    public bool played = false;
    public bool finished = false;
    public Vector2 map_pos;
    public int poi_size;
    public int poi_difficulty;
    public int lode_dia;
    public int lode_rub;
    public int lode_top;
    public int level_seed;

    [Header("POI Deco Data")]
    public int rotate_speed;
    public int deco_count;
    public int mesh_index;
}
