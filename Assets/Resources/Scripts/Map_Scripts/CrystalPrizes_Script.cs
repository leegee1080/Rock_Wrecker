using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalPrizes_Script : MonoBehaviour
{
    [SerializeField]private Animator _ani;
    [SerializeField]private bool _prizeSelected;
    public static CrystalPrizes_Script singleton;



    private void Start()
    {
        singleton = this;
    }

    public void OpenPrizes()
    {
        _ani.Play("Base Layer.OpenPrizes",layer: 0,normalizedTime: 0);
        MapUI_Script.singleton.CloseShopBackButton();
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
        _prizeSelected = true;
        ClosePrizes();
    }
}
