using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global_Vars{
    public static System.Random rand_num_gen = new System.Random();

    public static void Print_Map_Dict<T1,T2>(Dictionary<T1, T2> new_dict){
        foreach (KeyValuePair<T1,T2> item in new_dict)
        {
            Debug.Log("Key: " + item.Key + " value: (" + item.Value + ")");               
        }     
    }

    [Header("Map Data")]
    public const int galaxy_size = 250;
    public const int max_planet_size = 4;
    public const int min_planet_size = 2;
    public const int max_planet_coord = 500;
    public const int min_planet_coord = -500;
    public const int max_planet_difficulty = 10;

    [Header("POI Data")]
    public const int max_poi_deco = 10;
    public const int max_poi_rot_speed = 10;

    [Header("Level Data")]
    public const int level_starting_x_size = 20;
    public const int level_starting_y_size = 20;
}

public class Overallgame_Controller_Script : MonoBehaviour
{
    public static Overallgame_Controller_Script overallgame_controller_singleton;

    [Header("Global Player Stats")]
    public int player_score;
    public GameObject player_model;
    public List<MapPOI_ScriptableObject> main_map = new List<MapPOI_ScriptableObject>();


    [Header("Scene Loading")]
    public Scene_Enums chosen_scene_enum;


    [Header("Level Play Vars")]
    public MapPOI_ScriptableObject selected_level;

    private void Awake()
    {
        if (overallgame_controller_singleton == null)
        {
            overallgame_controller_singleton = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this);
    }

    public void Create_Map(int map_size = default){
        if(map_size == default){map_size = Global_Vars.galaxy_size;}
        main_map = new List<MapPOI_ScriptableObject>();

        List<Vector2> dist_check_list = new List<Vector2>();

        for (int i = 0; i < map_size; i++)
        {        
            MapPOI_ScriptableObject new_mappoi_so = ScriptableObject.CreateInstance<MapPOI_ScriptableObject>();
            new_mappoi_so.played =false;
            new_mappoi_so.finished =false;
            new_mappoi_so.poi_difficulty = (int)Global_Vars.rand_num_gen.Next(0,Global_Vars.max_planet_difficulty+1);
            new_mappoi_so.poi_size = (int)Global_Vars.rand_num_gen.Next(Global_Vars.min_planet_size,Global_Vars.max_planet_size+1);
            new_mappoi_so.rotate_speed = (int)Global_Vars.rand_num_gen.Next(0,Global_Vars.max_poi_rot_speed+1);
            new_mappoi_so.deco_count = (int)Global_Vars.rand_num_gen.Next(1,Global_Vars.max_poi_deco+1);
            new_mappoi_so.level_seed = (int)Global_Vars.rand_num_gen.Next(1,9999999);
            Vector2 rand_gen_pos = new Vector2(Global_Vars.rand_num_gen.Next(Global_Vars.min_planet_coord,Global_Vars.max_planet_coord+1), Global_Vars.rand_num_gen.Next(Global_Vars.min_planet_coord,Global_Vars.max_planet_coord+1));
            new_mappoi_so.map_pos = rand_gen_pos;
            foreach (Vector2 pos in dist_check_list)
            {
                if(Vector2.Distance(pos, rand_gen_pos) < 20){
                    new_mappoi_so.map_pos = default;
                    break;
                }
            }
            if(new_mappoi_so.map_pos ==default){continue;}
            new_mappoi_so.name = "A " + new_mappoi_so.poi_difficulty + " difficulty, " + new_mappoi_so.poi_size + " size, planet. At " + new_mappoi_so.map_pos;
            main_map.Add(new_mappoi_so);
            dist_check_list.Add(rand_gen_pos);
        }
    }

    public void Load_Map(){

    }
}
