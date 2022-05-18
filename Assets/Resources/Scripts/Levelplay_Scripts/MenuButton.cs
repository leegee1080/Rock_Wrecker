using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField]Animator _animator;
    [SerializeField]GameObject _buttonGroup;


    public void ToggleMenu()
    {
        if
        (
            Levelplay_Controller_Script.levelplay_controller_singleton.CurrentLevelState == LevelStatesEnum.Setup ||
            Levelplay_Controller_Script.levelplay_controller_singleton.CurrentLevelState == LevelStatesEnum.CleanupLose ||
            Levelplay_Controller_Script.levelplay_controller_singleton.CurrentLevelState == LevelStatesEnum.CleanupWin
        ){return;}
        
        if(!_buttonGroup.activeSelf)
        {
            Sound_Events.Play_Sound("Game_Click");
            _buttonGroup.SetActive(true);
            _animator.SetBool("MenuOpen",true);
            Levelplay_Controller_Script.levelplay_controller_singleton.ChangeLevelState(LevelStatesEnum.Paused);
            return;
        };
        Sound_Events.Play_Sound("Game_ClickOff");
        _animator.SetBool("MenuOpen",false);
        Levelplay_Controller_Script.levelplay_controller_singleton.ChangeLevelState(Levelplay_Controller_Script.levelplay_controller_singleton.LastLevelState);

    }

    public void HideButtons()
    {
        _buttonGroup.SetActive(false);
    }

    public void OptionsBtn()
    {   

    }

    public void QuitBtn()
    {   
        Sound_Events.Play_Sound("Game_Click");
        AnnouncerScript.singleton.AnnouncementClass = new AnnouncementPackage("quit confirm", AnnounceTypeEnum.TwoBtn, "Confirm Quit?", "", ConfirmExitToMap);
        AnnouncerScript.singleton.ChangeOpenState(true);
    }

    static bool ConfirmExitToMap(bool choice)
    {
        if(choice == false){return false;}
        Sound_Events.Play_Sound("Game_Click");
        Levelplay_Controller_Script.levelplay_controller_singleton.Exit_Level_To_Map(choice);
        return true;
    }
}
