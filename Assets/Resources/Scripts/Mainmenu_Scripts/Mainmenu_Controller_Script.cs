using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Mainmenu_States_Enum
{
    home,
    options,
    credits,
    quit,
    play
}

public class Mainmenu_Controller_Script : MonoBehaviour
{
    public static  Mainmenu_Controller_Script mainmenu_controller_singleton;

    [Header("Container Gameobjects")]
    [SerializeField]
    private GameObject home_container_go;
    [SerializeField]
    private GameObject options_container_go;
    [SerializeField]
    private GameObject credits_container_go;
    [SerializeField]
    private GameObject play_container_go;
    [SerializeField]
    private GameObject confirm_container_go;


    [Header("Overall Menu Vars")]
    public Mainmenu_States_Enum mainmenu_state;



    void Awake() => mainmenu_controller_singleton = this;

    private void Start() {
        Playerinput_Controller_Script.playerinput_controller_singleton.camera_controls_allowed =false;
    }

    public void New_Game(){
        // Overallgame_Controller_Script.overallgame_controller_singleton.Create_Map();
        // Loading_Controller_Script.loading_controller_singleton.Load_Next_Scene(Scene_Enums.levelselect);

        ScnTrans_Script.singleton.ScnTransOut(Scene_Enums.levelselect);
    }
}
