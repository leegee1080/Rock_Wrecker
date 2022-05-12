using System;
using System.IO;
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
    public float[] player_mapPos;
    public int player_score;
    public float score_multi = 1;
    public int player_dia = 0;
    public int player_top = 0;
    public int player_rub = 0;
    public int DroneShields = 0;
    public float LightRadius = 33;
    public int PlayerFuelReach = 10;
    public int PlayerFuel = 0;
    public int PlayerDrones = 0;
    public int PlayerDroneCost = 10;
    public int PlayerFuelCost = 10;
    public List<MapPOI_ScriptableObject> main_map = new List<MapPOI_ScriptableObject>();
}
[Serializable]public class MapDataStorageClass
{
    public string name;
    public List<MapData> saveableMap = new List<MapData>();
}
[Serializable]public class MapData
{
    public string name = "";
    public bool played = false;
    public bool finished = false;
    public float[] map_pos = new float[2];//conversion from a vector
    public int poi_size = 1;
    public int poi_difficulty = 1;
    public int lode_dia = 0;
    public int lode_rub = 0;
    public int lode_top = 0;
    public int level_seed = 123;
    public int rotate_speed = 1;
    public int deco_count = 0;
    public int mesh_index = 0;
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
    public const int max_planet_dia_lode_multi = 1;
    public const int max_planet_top_lode_multi = 4;
    public const int max_planet_rub_lode_multi = 3;

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
        float amountF = amount/100;
        pd.score_multi += amountF;
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
        if(pd.PlayerDrones >= max_drones){return;}
        pd.PlayerDrones += amount;
    }
    public static void FuelPlus(int amount,PlayerData pd)
    {
        if(pd.PlayerFuel >= max_fuel){return;}
        pd.PlayerFuel += amount;
        try
            {
                MapUI_Script mapUI = MapUI_Script.singleton;
                if(mapUI._fuelBtnScript._menuOpen){mapUI._fuelBtnScript.FinishOpenMenu();}
            }
            catch(Exception e)
            {
                Debug.Log("Error when adding free fuel " + e);
            }
    }
    public static void LightRadiusPlus(int amount,PlayerData pd)
    {
        pd.LightRadius += amount;
    }
}

public class Overallgame_Controller_Script : MonoBehaviour
{
    public static Overallgame_Controller_Script overallgame_controller_singleton;

    [Header("Global Player Stats")]
    public PlayerData CurrentPlayer;

    // public int player_score;
    // public int player_dia;
    // public int player_top;
    // public int player_rub;
    // public int PlayerFuel;
    // public int PlayerDrones;
    // public GameObject player_model;
    // public List<MapPOI_ScriptableObject> main_map = new List<MapPOI_ScriptableObject>();


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


        PlayerData loadedData = Load_Game();
        loadedData.main_map = Load_Map();
        if(loadedData == null)
        {
            NewGame();
            return;
        }
        CurrentPlayer = loadedData;
    }

    public void Create_Map(int map_size = default, int max_coord = default, int min_coord = default)
    {
        if(map_size == default){map_size = Global_Vars.galaxy_size;}
        if(max_coord == default){max_coord = Global_Vars.max_planet_coord;}
        if(min_coord == default){min_coord = Global_Vars.min_planet_coord;}
        CurrentPlayer.main_map = new List<MapPOI_ScriptableObject>();

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
            Vector2 rand_gen_pos = new Vector2(Global_Vars.rand_num_gen.Next(min_coord, max_coord+1), Global_Vars.rand_num_gen.Next(min_coord, max_coord+1));
            new_mappoi_so.map_pos = rand_gen_pos;

            //lode gen
            new_mappoi_so.lode_dia = (int)Global_Vars.rand_num_gen.Next(1,Global_Vars.max_planet_dia_lode_multi * new_mappoi_so.poi_size);
            new_mappoi_so.lode_rub = (int)Global_Vars.rand_num_gen.Next(1,Global_Vars.max_planet_rub_lode_multi * new_mappoi_so.poi_size);
            new_mappoi_so.lode_top = (int)Global_Vars.rand_num_gen.Next(1,Global_Vars.max_planet_top_lode_multi * new_mappoi_so.poi_size);
            //end lode gen

            foreach (Vector2 pos in dist_check_list)
            {
                if(Vector2.Distance(pos, rand_gen_pos) < 20){
                    new_mappoi_so.map_pos = default;
                    break;
                }
            }
            if(new_mappoi_so.map_pos == default){continue;}
            new_mappoi_so.name = "A " + new_mappoi_so.poi_difficulty + " difficulty, " + new_mappoi_so.poi_size + " size, planet. At " + new_mappoi_so.map_pos;
            CurrentPlayer.main_map.Add(new_mappoi_so);
            dist_check_list.Add(rand_gen_pos);
        }
    }

    public void NewGame()
    {
        CurrentPlayer = new PlayerData();
        Create_Map();
        // Save_Game(CurrentPlayer);
        // Save_Map(CurrentPlayer.main_map);
    }

    public void Debug_SaveGame()
    {
        Save_Game(CurrentPlayer);
        Save_Map(CurrentPlayer.main_map);
    }

    public void Debug_LoadGame()
    {
        PlayerData loadedData = Load_Game();
        loadedData.main_map = Load_Map();
    }

    public void Debug_ClearSaveData()
    {
        CurrentPlayer = new PlayerData();
        Create_Map();
    }

    public void SaveCurrentPlayer()
    {
        Save_Game(CurrentPlayer);
    }
    public void SaveCurrentMap()
    {
        Save_Map(CurrentPlayer.main_map);
    }


    public void Save_Game(PlayerData dataToSave)
    {
        PlayerData playerData = dataToSave;

        string fullPathP = Path.Combine(Application.persistentDataPath, "playerdata.ye");

        // playerData.main_map = null;

        try
        {
            string PdataToStore = JsonUtility.ToJson(playerData, false);

            using(FileStream stream = new FileStream(fullPathP, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(PdataToStore);
                }
            }
            Debug.Log("Player Saved");
        }
        catch(Exception e)
        {
            Debug.Log("Error when saving data " + e);
        }
    }
    public void Save_Map(List<MapPOI_ScriptableObject> mapToSave)
    {
        MapDataStorageClass mainMapData = new MapDataStorageClass();

        mainMapData.name = CurrentPlayer.name;
        for (int i = 0; i < mapToSave.Count; i++)
        {
            mainMapData.saveableMap.Add(ObjectToMapData(mapToSave[i]));
        }

        string fullPathM = Path.Combine(Application.persistentDataPath, "mapdata.ye");

        try
        {
            string MdataToStore = JsonUtility.ToJson(mainMapData, false);

            using(FileStream stream = new FileStream(fullPathM, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(MdataToStore);
                }
            }
            Debug.Log("Map Saved");
        }
        catch(Exception e)
        {
            Debug.Log("Error when saving data " + e);
        }
    }    

    public PlayerData Load_Game()
    {
        PlayerData loadedPlayer = null;

        string fullPathP = Path.Combine(Application.persistentDataPath, "playerdata.ye");
        if(File.Exists(fullPathP))
        {
            try
            {
                string PdataToLoad = "";

                using(FileStream stream = new FileStream(fullPathP, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        PdataToLoad = reader.ReadToEnd();
                    }
                }
                loadedPlayer = JsonUtility.FromJson<PlayerData>(PdataToLoad);
                Debug.Log("Player Loaded");
                return loadedPlayer;
            }
            catch(Exception e)
            {
                Debug.Log("Error when loading data " + e);
            }
            
        }
        return null;
    }
    public List<MapPOI_ScriptableObject> Load_Map()
    {
        MapDataStorageClass loadedMap = null;
        List<MapPOI_ScriptableObject> mapToReturn = new List<MapPOI_ScriptableObject>();

        string fullPathM = Path.Combine(Application.persistentDataPath, "mapdata.ye");
        if(File.Exists(fullPathM))
        {
            try
            {
                string MdataToLoad = "";

                using(FileStream stream = new FileStream(fullPathM, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        MdataToLoad = reader.ReadToEnd();
                    }
                }
                loadedMap = JsonUtility.FromJson<MapDataStorageClass>(MdataToLoad);

                for (int i = 0; i < loadedMap.saveableMap.Count; i++)
                {
                    mapToReturn.Add(MapDataToObject(loadedMap.saveableMap[i]));
                }
                Debug.Log("Map Loaded");
                return mapToReturn;
            }
            catch(Exception e)
            {
                Debug.Log("Error when loading data " + e);
            }
            
        }
        return null;
    }
    private MapPOI_ScriptableObject MapDataToObject(MapData md)
    {
        MapPOI_ScriptableObject tempPOI = (MapPOI_ScriptableObject)ScriptableObject.CreateInstance("MapPOI_ScriptableObject");
        MapData tempMD = md;

        tempPOI.name = tempMD.name;
        tempPOI.played = tempMD.played;
        tempPOI.finished = tempMD.finished;
        tempPOI.map_pos = new Vector2(tempMD.map_pos[0],tempMD.map_pos[1]);
        tempPOI.poi_size = tempMD.poi_size;
        tempPOI.poi_difficulty = tempMD.poi_difficulty;
        tempPOI.lode_dia = tempMD.lode_dia;
        tempPOI.lode_rub = tempMD.lode_rub;
        tempPOI.lode_top = tempMD.lode_top;
        tempPOI.level_seed = tempMD.level_seed;
        tempPOI.rotate_speed = tempMD.rotate_speed;
        tempPOI.deco_count = tempMD.deco_count;
        tempPOI.mesh_index = tempMD.mesh_index;

        return tempPOI;
    }
    private MapData ObjectToMapData(MapPOI_ScriptableObject so)
    {
        MapPOI_ScriptableObject tempPOI = so;
        MapData tempMD = new MapData();

        tempMD.name = tempPOI.name;
        tempMD.played = tempPOI.played;
        tempMD.finished = tempPOI.finished;
        tempMD.map_pos[0] = tempPOI.map_pos[0];
        tempMD.map_pos[1] = tempPOI.map_pos[1];
        tempMD.poi_size = tempPOI.poi_size;
        tempMD.poi_difficulty = tempPOI.poi_difficulty;
        tempMD.lode_dia = tempPOI.lode_dia;
        tempMD.lode_rub = tempPOI.lode_rub;
        tempMD.lode_top = tempPOI.lode_top;
        tempMD.level_seed = tempPOI.level_seed;
        tempMD.rotate_speed = tempPOI.rotate_speed;
        tempMD.deco_count = tempPOI.deco_count;
        tempMD.mesh_index = tempPOI.mesh_index;

        return tempMD;
    }
}
