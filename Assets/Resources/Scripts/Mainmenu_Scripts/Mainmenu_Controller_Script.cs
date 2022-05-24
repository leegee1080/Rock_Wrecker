using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Mainmenu_Controller_Script : MonoBehaviour
{
    public static  Mainmenu_Controller_Script mainmenu_controller_singleton;

    [Header("Container Gameobjects")]
    [SerializeField]private GameObject home_container_go;
    [SerializeField]private GameObject options_container_go;
    [SerializeField]private GameObject credits_container_go;
    [SerializeField]private GameObject play_container_go;

    [Header("Ref Gameobjects")]
    [SerializeField]private TMP_Text _playText;


    void Awake() => mainmenu_controller_singleton = this;

    private void Start()
    {
        Playerinput_Controller_Script.playerinput_controller_singleton.camera_controls_allowed =false;
        Playerinput_Controller_Script.playerinput_controller_singleton.on_screen_controls_allowed =false;

        Sound_Events.Play_Sound("Music_Menu");
        Sound_Events.Delay_Play_Sound("Music_MenuLoop",45f);
    }

    public void New_Game()
    {
        Sound_Events.Stop_Sound("Music_Menu");
        Sound_Events.Stop_Sound("Music_MenuLoop");
        Audio_Controller_Script.singleton.StopAllCoroutines();
        ScnTrans_Script.singleton.ScnTransOut(Scene_Enums.levelselect);
    }

    public void NewGame()
    {

    }

    public void ContinueGame()
    {

    }

    public void ShowOptions()
    {

    }
    public void QuitGame()
    {

    }
}
