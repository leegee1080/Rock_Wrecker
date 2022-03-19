using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField]Animator _animator;
    [SerializeField]GameObject _buttonGroup;


    public void ToggleMenu()
    {
        if(!_buttonGroup.activeSelf)
        {
            _buttonGroup.SetActive(true);
            _animator.SetBool("MenuOpen",true);
            Levelplay_Controller_Script.levelplay_controller_singleton.ChangeLevelState(LevelStatesEnum.Paused);
            return;
        };
        _animator.SetBool("MenuOpen",false);
        Levelplay_Controller_Script.levelplay_controller_singleton.ChangeLevelState(Levelplay_Controller_Script.levelplay_controller_singleton.LastLevelState);

    }

    public void HideButtons()
    {
        _buttonGroup.SetActive(false);
    }
}
