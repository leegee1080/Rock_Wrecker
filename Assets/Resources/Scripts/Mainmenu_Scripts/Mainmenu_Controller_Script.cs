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


    void Awake() => mainmenu_controller_singleton = this;

    private void Start()
    {
        Playerinput_Controller_Script.playerinput_controller_singleton.camera_controls_allowed =false;
        Playerinput_Controller_Script.playerinput_controller_singleton.on_screen_controls_allowed =false;

        Sound_Events.Play_Sound("Music_MenuLoop");

        _homeContainer.OpenMenu();
    }

    public void NewGame()
    {
        Sound_Events.Play_Sound("Game_Click");
        
        AnnouncerScript.singleton.AnnouncementClass = new AnnouncementPackage("startover", AnnounceTypeEnum.TwoBtn,"START OVER?","Are you sure?", ClearSavedGame);
        AnnouncerScript.singleton.ChangeOpenState(true);
    }

    public bool ClearSavedGame(bool choice)
    {
        if(choice)
        {
            Sound_Events.Play_Sound("Game_PlayerDeath");
            Overallgame_Controller_Script.overallgame_controller_singleton.NewGame();
            AnnouncerScript.singleton.ChangeOpenState(false);
            return true;
        }

        AnnouncerScript.singleton.ChangeOpenState(false);
        return false;
    }

    public void ContinueGame()
    {
        Sound_Events.Play_Sound("Game_DropshipLaunch");
        _homeContainer.CloseMenu();
        Sound_Events.Stop_Sound("Music_MenuLoop");
        ScnTrans_Script.singleton.ScnTransOut(Scene_Enums.levelselect);
    }

    public void ShowMainmenu()
    {
        Sound_Events.Play_Sound("Game_ClickOff");
        _optionsContainer.CloseMenu();
        _homeContainer.OpenMenu();
    }
    public void ShowOptions()
    {
        Sound_Events.Play_Sound("Game_Click");
        _homeContainer.CloseMenu();
        _optionsContainer.OpenMenu();
    }
    public void ShowCredits()
    {
        Sound_Events.Play_Sound("Game_Click");
        _homeContainer.CloseMenu();
    }
    public void QuitGame()
    {
        AnnouncerScript.singleton.AnnouncementClass = new AnnouncementPackage("quitgame", AnnounceTypeEnum.TwoBtn,"Quit Game?","Are you sure?", CloseApp);
        AnnouncerScript.singleton.ChangeOpenState(true);
    }

    public bool CloseApp(bool choice)
    {
        if(choice==false)
        {
            AnnouncerScript.singleton.ChangeOpenState(false);
            return false;
        }
        Application.Quit();
        return true;
    }
}
