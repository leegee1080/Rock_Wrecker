using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CrystalPrizes_Script : MonoBehaviour
{
    [SerializeField]private Animator _ani;
    [SerializeField]private bool _prizeSelected;
    public static CrystalPrizes_Script singleton;

    public CrystalPrize_ScriptableObject[] _prizes;
    [SerializeField]private CrystalCard_Script[] _cards;


    private void Start()
    {
        singleton = this;
    }

    public void OpenPrizes()
    {
        for (int i = 0; i < _cards.Length; i++)
        {
            _cards[i].ApplyCard(_prizes[i]);
        }

        _ani.Play("Base Layer.OpenPrizes",layer: 0,normalizedTime: 0);
        
        _prizeSelected = false;
    }
    public void ClosePrizes()
    {
        _ani.Play("Base Layer.ClosePrizes",layer: 0,normalizedTime: 0);
        MapUI_Script.singleton.OpenShopBackButton();
    }

    public void SelectReward(int buttonInt)
    {
        if(_prizeSelected){return;}

        if(buttonInt >=0 && buttonInt <3 && _prizes[buttonInt] != null)
        {
            _prizes[buttonInt].RunStatChange();
        }
        

        _prizeSelected = true;
        ClosePrizes();
    }

    public void FinishOpenPrize(int i)
    {
        Instantiate(_prizes[i].particle, parent: _cards[i].gameObject.transform);
    }
}
