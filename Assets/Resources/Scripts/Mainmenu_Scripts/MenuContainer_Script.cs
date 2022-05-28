using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuContainer_Script : MonoBehaviour
{
    [SerializeField]Animator[] _animatedItems;
    [SerializeField]IEnumerator _transCoroutine;
    [SerializeField]float _transTime;

    public void OpenMenu()
    {
        if(_transCoroutine != null){StopCoroutine(_transCoroutine);}
        _transCoroutine = TransIn();
        StartCoroutine(_transCoroutine);
    }

    public void CloseMenu()
    {
        for (int i = 0; i < _animatedItems.Length; i++)
        {
            _animatedItems[i].Play("Close");
        }
    }

    private IEnumerator TransIn()
    {
        for (int i = 0; i < _animatedItems.Length; i++)
        {
            _animatedItems[i].Play("Open");
            yield return new WaitForSeconds(_transTime);
        }
    }
}
