using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CrystalTypes
{
    None,
    Diamond,
    Topaz,
    Ruby
}

public enum PlayerUpgradeTypes
{
    DroneShield,
    Money,
    FuelEff,
    MoneyMultiplyer,
    CheaperDrone,
    CheaperFuel,
    FreeDrone,
    FreeFuel,
    LightRadius
}

[Serializable]public class PlayerData
{
    public string name;
    public int player_score;
    public float score_multi = 1;
    public int player_dia = 0;
    public int player_top = 0;
    public int player_rub = 0;
    public int DroneShields = 0;
    public float LightRadius;
    public int PlayerFuelReach = 10;
    public int PlayerFuel = 0;
    public int PlayerDrones = 0;
    public int PlayerDroneCost = 10;
    public int PlayerFuelCost = 10;
    public GameObject player_model;
    public List<MapPOI_ScriptableObject> main_map = new List<MapPOI_ScriptableObject>();
}

public static class Global_Vars
{
    public static System.Random rand_num_gen = new System.Random();

    public static void Print_Map_Dict<T1,T2>(Dictionary<T1, T2> new_dict){
        foreach (KeyValuePair<T1,T2> item in new_dict)
        {
            Debug.Log("Key: " + item.Key + " value: (" + item.Value + ")");               
        }     
    }

    public static void Print_Map_Array(Grid_Data[][] new_array){
        foreach (Grid_Data[] i in new_array)
        {
            foreach(Grid_Data j in i)
            {
                Debug.Log("gridpos: (" + j.grid_pos + ")" + " res: (" + j.resident + ")"+ " actualpos: (" + j.actual_pos + ")"+ " noise: (" + j.noise_data + ")");    
            }           
        }  
    }

    [Header("Map Data")]
    public const int galaxy_size = 250;
    public const int max_planet_size = 4;
    public const int min_planet_size = 1;
    public const int max_planet_coord = 500;
    public const int min_planet_coord = -500;
    public const int max_planet_difficulty = 3;
    public const int max_planet_mesh_index = 7;
    public const int max_planet_dia_lode_multi = 5;
    public const int max_planet_top_lode_multi = 5;
    public const int max_planet_rub_lode_multi = 5;

    [Header("Ship Data")]
    public const int fuel_reach = 10;
    

    [Header("Shop Data")]
    public const int drone_cost = 10;
    public const int fuel_cost = 10;
    public const int max_fuel = 8;
    public const int max_drones = 9;

    [Header("POI Data")]
    public const int max_poi_deco = 2;
    public const int max_poi_rot_speed = 10;

    [Header("Level Data")]
    public const int level_starting_x_size = 20;
    public const int level_starting_y_size = 20;

    [Header("CrystalPrizeFuncs")]
    public static readonly Dictionary<PlayerUpgradeTypes, Action<int,PlayerData>> PlayerUpgradeFuncDict = new Dictionary<PlayerUpgradeTypes, Action<int,PlayerData>>
    {
        {PlayerUpgradeTypes.DroneShield, DroneShieldPlus},
        {PlayerUpgradeTypes.Money, MoneyPlus},
        {PlayerUpgradeTypes.FuelEff, FuelEffPlus},
        {PlayerUpgradeTypes.MoneyMultiplyer, MoneyMultiPlus},
        {PlayerUpgradeTypes.CheaperDrone, CheaperDronePlus},
        {PlayerUpgradeTypes.CheaperFuel, CheaperFuelPlus},
        {PlayerUpgradeTypes.FreeDrone, DronePlus}, 
        {PlayerUpgradeTypes.FreeFuel, FuelPlus},
        {PlayerUpgradeTypes.LightRadius, LightRadiusPlus}
    };
    public static void DroneShieldPlus(int amount,PlayerData pd)
    {
        pd.DroneShields += amount;
    }
    public static void MoneyPlus(int amount,PlayerData pd)
    {
        pd.player_score += amount;
    }
    public static void FuelEffPlus(int amount,PlayerData pd)
    {
        pd.PlayerFuelReach += amount;
    }
    public static void MoneyMultiPlus(int amount,PlayerData pd)
    {
        float amountF = amount/10;
        pd.PlayerFuel += (int)amountF;
    }
    public static void CheaperDronePlus(int amount,PlayerData pd)
    {
        pd.PlayerDroneCost -= amount;
    }
    public static void CheaperFuelPlus(int amount,PlayerData pd)
    {
        pd.PlayerFuelCost -= amount;
    }
    public static void DronePlus(int amount,PlayerData pd)
    {
        pd.PlayerDrones -= amount;
    }
    public static void FuelPlus(int amount,PlayerData pd)
    {
        pd.PlayerFuel -= amount;
    }
    public static void LightRadiusPlus(int amount,PlayerData pd)
    {
        pd.LightRadius -= amount;
    }
}

public class Overallgame_Controller_Script : MonoBehaviour
{
    public static Overallgame_Controller_Script overallgame_controller_singleton;

    [Header("Global Player Stats")]
    public PlayerData CurrentPlayer;
    public int player_score;
    public int player_dia;
    public int player_top;
    public int player_rub;
    public int PlayerFuel;
    public int PlayerDrones;
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

        //DEBUG
        CurrentPlayer = new PlayerData();
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
            new_mappoi_so.deco_count = (int)Global_Vars.rand_num_gen.Next(1,Global_Vars.max_poi_deco * (new_mappoi_so.poi_difficulty+ 1) * new_mappoi_so.poi_size);
            new_mappoi_so.level_seed = (int)Global_Vars.rand_num_gen.Next(1,9999999);
            new_mappoi_so.mesh_index = (int)Global_Vars.rand_num_gen.Next(0,Global_Vars.max_planet_mesh_index);
            Vector2 rand_gen_pos = new Vector2(Global_Vars.rand_num_gen.Next(Global_Vars.min_planet_coord,Global_Vars.max_planet_coord+1), Global_Vars.rand_num_gen.Next(Global_Vars.min_planet_coord,Global_Vars.max_planet_coord+1));
            new_mappoi_so.map_pos = rand_gen_pos;

            //lode gen
            new_mappoi_so.lode_dia = (int)Global_Vars.rand_num_gen.Next(0,Global_Vars.max_planet_dia_lode_multi * new_mappoi_so.poi_size);
            new_mappoi_so.lode_rub = (int)Global_Vars.rand_num_gen.Next(0,Global_Vars.max_planet_rub_lode_multi * new_mappoi_so.poi_size);
            new_mappoi_so.lode_top = (int)Global_Vars.rand_num_gen.Next(0,Global_Vars.max_planet_top_lode_multi * new_mappoi_so.poi_size);
            //end lode gen

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
