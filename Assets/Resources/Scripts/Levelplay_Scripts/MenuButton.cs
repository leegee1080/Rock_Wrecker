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
            return;
        };
        _animator.SetBool("MenuOpen",false);
    }

    public void HideButtons()
    {
        _buttonGroup.SetActive(false);
    }
}
