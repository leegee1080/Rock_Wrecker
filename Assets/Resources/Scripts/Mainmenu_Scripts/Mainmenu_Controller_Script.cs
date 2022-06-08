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
    [SerializeField]private GameObject _creditsContainer;
    [SerializeField]private GameObject _logoContainer;
    [SerializeField]private float _logoChangeSpeed;
    [SerializeField]private Vector3 _logoHome;
    [SerializeField]private Vector3 _logoOffScreen;


    void Awake() => mainmenu_controller_singleton = this;

    private void Start()
    {
        Playerinput_Controller_Script.playerinput_controller_singleton.camera_controls_allowed =false;
        Playerinput_Controller_Script.playerinput_controller_singleton.on_screen_controls_allowed =false;

        Sound_Events.Play_Sound("Music_MenuLoop");

        _homeContainer.OpenMenu();
        TutorialObject_Script.singleton.FindandPlayTutorialObject("first_mainmenu");
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
            PlayerPrefs.SetInt(Sound_Type_Tags.fx.ToString(), 50);
            Sound_Events.Change_Volume(0.5f, Sound_Type_Tags.fx);
            PlayerPrefs.SetInt(Sound_Type_Tags.music.ToString(), 50);
            Sound_Events.Change_Volume(0.5f, Sound_Type_Tags.music);
            ShowMainmenu();
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
        SwapLogos(_logoContainer, _creditsContainer);
    }
    public void ShowOptions()
    {
        Sound_Events.Play_Sound("Game_Click");
        _homeContainer.CloseMenu();
        _optionsContainer.OpenMenu();
        SwapLogos(_creditsContainer, _logoContainer);
    }
    // public void ShowCredits()
    // {
    //     Sound_Events.Play_Sound("Game_Click");
    //     _homeContainer.CloseMenu();
    //     SwapLogos(_creditsContainer, _logoContainer);
    // }

    public void TurnOffTut()
    {
        Overallgame_Controller_Script.overallgame_controller_singleton.tutOn = false;
        PlayerPrefs.SetInt("tut", 0);
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

    public void SwapLogos(GameObject logoIn, GameObject LogoOut)
    {
        iTween.Stop();
        iTween.MoveTo(LogoOut.gameObject, iTween.Hash(
            "position", _logoOffScreen,
            "islocal", true,
            "easetype", iTween.EaseType.easeOutBack,
            "time", _logoChangeSpeed));

        iTween.MoveTo(logoIn.gameObject, iTween.Hash(
            "position", _logoHome,
            "delay", _logoChangeSpeed/2,
            "islocal", true,
            "easetype", iTween.EaseType.easeOutBack,
            "time", _logoChangeSpeed));
    }
}
