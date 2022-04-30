using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeShipWindows_Script : MonoBehaviour
{
    [SerializeField]private GameObject _shipWindow;
    [SerializeField]private float _fadeTime;
    public void FadeShipWindowsOut()
    {
        iTween.FadeTo(_shipWindow,0f,_fadeTime);
    }
    public void FadeShipWindowsIn()
    {
        iTween.FadeTo(_shipWindow,0.5f,_fadeTime);
    }

    public void FinishMapTrans()
    {
        Levelselect_Controller_Script.levelselect_controller_singletion.FinishToMapTrans();
        Levelselect_Controller_Script.levelselect_controller_singletion.HideShipGameObject();
    }
    public void FinishShopTrans()
    {
         Levelselect_Controller_Script.levelselect_controller_singletion.FinishSwitchToShop();
    }
    public void FinishShoptoShipTrans()
    {
         Levelselect_Controller_Script.levelselect_controller_singletion.FinishSwitchToShipFromShop();
    }

    public void FinishShipTrans()
    {
        Levelselect_Controller_Script.levelselect_controller_singletion.FinishToShipTrans();
    }
    public void FinishMenuTrans()
    {
        Levelselect_Controller_Script.levelselect_controller_singletion.Back_to_Menu();
    }
}
