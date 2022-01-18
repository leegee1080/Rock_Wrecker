using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levelplay_Controller_Script : MonoBehaviour
{
    public static Levelplay_Controller_Script levelplay_controller_singleton;

    [Header("Canvas Elements")]
    [SerializeField]private GameObject ingame_menu_container;

    [Header("Level Gen Vars")]
    [SerializeField]private int map_x_size;
    [SerializeField]private int map_y_size;


    void Awake() => levelplay_controller_singleton = this;

    private void Start() {
        Playerinput_Controller_Script.playerinput_controller_singleton.camera_controls_allowed = true;

        map_x_size = Global_Vars.level_starting_x_size * Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.poi_size;
        map_y_size = Global_Vars.level_starting_y_size * Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.poi_size;

    }

    public void Show_Ingame_Menu(){
        
    }

}
