using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Mainmenu_Controller_Script : MonoBehaviour
{
    public static  Mainmenu_Controller_Script mainmenu_controller_singleton;

    [Header("Container Gameobjects")]
    [SerializeField]private MenuContainer_Script _homeContainer;
    [SerializeField]private MenuContainer_Script _optionsContainer;
    [SerializeField]private MenuContainer_Script _creditsContainer;
    [SerializeField]private MenuContainer_Script _playContainer;

    [Header("Ref Gameobjects")]
    [SerializeField]private TMP_Text _playText;


    void Awake() => mainmenu_controller_singleton = this;

    private void Start()
    {
        Playerinput_Controller_Script.playerinput_controller_singleton.camera_controls_allowed =false;
        Playerinput_Controller_Script.playerinput_controller_singleton.on_screen_controls_allowed =false;

        Sound_Events.Play_Sound("Music_Menu");
        Sound_Events.Delay_Play_Sound("Music_MenuLoop",45f);

        _homeContainer.OpenMenu();
    }

    public void New_Game()
    {
        _homeContainer.CloseMenu();
        Sound_Events.Stop_Sound("Music_Menu");
        Sound_Events.Stop_Sound("Music_MenuLoop");
        Audio_Controller_Script.singleton.StopAllCoroutines();
        ScnTrans_Script.singleton.ScnTransOut(Scene_Enums.levelselect);
    }

    public void NewGame()
    {
        AnnouncerScript.singleton.AnnouncementClass = new AnnouncementPackage("startover", AnnounceTypeEnum.TwoBtn,"START OVER?","Are you sure?", ClearSavedGame);
        AnnouncerScript.singleton.ChangeOpenState(true);
    }

    public bool ClearSavedGame(bool choice)
    {
        if(choice)
        {
            Overallgame_Controller_Script.overallgame_controller_singleton.Debug_ClearSaveData();
            AnnouncerScript.singleton.ChangeOpenState(false);
            return true;
        }

        AnnouncerScript.singleton.ChangeOpenState(false);
        return false;
    }

    public void ContinueGame()
    {

    }

    public void ShowMainmenu()
    {
        _optionsContainer.CloseMenu();
        _homeContainer.OpenMenu();
    }
    public void ShowOptions()
    {
        _homeContainer.CloseMenu();
        _optionsContainer.OpenMenu();
    }
    public void QuitGame()
    {

    }
}
