using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levelselect_Controller_Script : MonoBehaviour
{
    public static Levelselect_Controller_Script levelselect_controller_singletion;

    private List<MapPOI_ScriptableObject> map_poi_list;
    [SerializeField] private MapPOI_Script map_poi_go;

    [Header("Selection Vars")]
    [SerializeField]private MapPOI_Script selected_poi;

    void Awake() => levelselect_controller_singletion = this;

    private void Start(){
        Playerinput_Controller_Script.playerinput_controller_singleton.camera_controls_allowed =true;
        
        map_poi_list = Overallgame_Controller_Script.overallgame_controller_singleton.main_map;

        if(map_poi_list == null){return;}
        foreach(MapPOI_ScriptableObject map_poi_so in map_poi_list){
            MapPOI_Script new_poi = Instantiate(map_poi_go,this.transform);
            new_poi.Init(map_poi_so);
        }
    }

    public void Communicate_Selected_POI(MapPOI_Script tapped_poi){
        selected_poi?.Lowlight_POI();
        selected_poi = tapped_poi;
    }

    public void Center_Screen(){
        Camera.main.transform.position = new Vector3(0,0,Global_Vars.min_planet_coord);
    }

    public void Back_to_Menu(){
        Overallgame_Controller_Script.overallgame_controller_singleton.Create_Map();
        Loading_Controller_Script.loading_controller_singleton.Load_Next_Scene(Scene_Enums.mainmenu);
    }
}
