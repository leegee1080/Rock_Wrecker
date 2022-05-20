using UnityEngine;


[CreateAssetMenu(fileName = "New Overall MinMax", menuName = "Scriptable Objects/New Overall MinMax")]
public class OverallMinMax_ScriptableObject : ScriptableObject
{
    public int max_planet_size;
    public int min_planet_size;
    public int min_planet_difficulty;
    public int max_planet_difficulty;
    public int max_planet_dia_lode_multi;
    public int max_planet_top_lode_multi;
    public int max_planet_rub_lode_multi;
}
