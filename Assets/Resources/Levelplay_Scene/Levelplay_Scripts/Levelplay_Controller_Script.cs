using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levelplay_Controller_Script : MonoBehaviour
{
    public static Levelplay_Controller_Script levelplay_controller_singleton;

    [Header("Canvas Elements")]
    [SerializeField]private GameObject ingame_menu_container;

    [Header("Level Gen Vars")]
    private Dictionary<Vector2, GameObject> map_coord_dict = new Dictionary<Vector2, GameObject>();
    [SerializeField]private int map_x_size;
    [SerializeField]private int map_y_size;
    [SerializeField]private int map_unit_spacing;


    void Awake() => levelplay_controller_singleton = this;

    private void Start() {
        Playerinput_Controller_Script.playerinput_controller_singleton.camera_controls_allowed = true;

        map_x_size = Global_Vars.level_starting_x_size * Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.poi_size;
        map_y_size = Global_Vars.level_starting_y_size * Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.poi_size;

        Gen_Map_Coords();
    }

    private void Gen_Map_Coords(){
        for (int x = -map_x_size/2; x <= map_x_size/2; x++)
        {   
            for (int y = -map_y_size/2; y <= map_y_size/2; y++)
            {
                Vector2 new_coord = new Vector2(x,y);
                map_coord_dict[new_coord] = null;
            }
        }
        
        Print_Map_Dict<Vector2, GameObject>(map_coord_dict);
    }

    private void Print_Map_Dict<T1,T2>(Dictionary<T1, T2> new_dict){
        foreach (KeyValuePair<T1,T2> item in new_dict)
        {
            Debug.Log(item.Key + "=>" + item.Value);               
        }     
    }

    public void Show_Ingame_Menu(){
        
    }

}
