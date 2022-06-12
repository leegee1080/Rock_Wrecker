using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapUI_Script : MonoBehaviour
{
    public static MapUI_Script singleton;
    [SerializeField]private Animator _animator;
    [SerializeField]private GameObject _ShipButtonsGroup;
    [SerializeField]private GameObject _MapButtonsGroup;
    [SerializeField]private GameObject[] _inventroyIcons;
    [SerializeField]private TMP_Text[] _inventroyTexts;

    [SerializeField]private GameObject _fuelUIContainer;
    [SerializeField]private GameObject _droneUIContainer;

    private Overallgame_Controller_Script _ocScript;
    [SerializeField]public FuelBtn_Script _fuelBtnScript;
    [SerializeField]public GameObject _droneShield;

    
    private IEnumerator _runningBlinkRountine;

    private void Awake()
    {
        singleton = this;
        _ocScript = Overallgame_Controller_Script.overallgame_controller_singleton;
        if(Overallgame_Controller_Script.overallgame_controller_singleton.CurrentPlayer.DroneShields > 0){_droneShield.SetActive(true);}else{_droneShield.SetActive(false);}
    }

    public void OpenSelectionMiniMenu()
    {
        _MapButtonsGroup.SetActive(true);
        _ShipButtonsGroup.SetActive(false);
        _animator.Play("Base Layer.ShowMapbuttonsMini",layer: 0,normalizedTime: 0);
    }
    public void CloseSelectionMiniMenu()
    {
        _animator.Play("Base Layer.HideMapbuttonsMini",layer: 0,normalizedTime: 0);
    }
    public void OpenShipMiniMenu()
    {
        _MapButtonsGroup.SetActive(false);
        _ShipButtonsGroup.SetActive(true);
        _animator.Play("Base Layer.OpenShipMenu",layer: 0,normalizedTime: 0);
    }
    public void CloseShipMiniMenu()
    {
        _animator.Play("Base Layer.CloseShipMenu",layer: 0,normalizedTime: 0);
    }

    public void OpenShopBackButton()
    {
        UpdateShopUINumbers();
        _animator.Play("Base Layer.ShopMiniMenuOpen",layer: 0,normalizedTime: 0);
    }
    public void CloseShopBackButton()
    {
        _animator.Play("Base Layer.ShopMiniMenuClose",layer: 0,normalizedTime: 0);
    }


    public void PurchaseDrone(bool freeDrone = false)
    {
        if(_ocScript.CurrentPlayer.player_score >= Overallgame_Controller_Script.overallgame_controller_singleton.CurrentPlayer.PlayerDroneCost)
        {
            if(_ocScript.CurrentPlayer.PlayerDrones >= Global_Vars.max_drones)
            {
                BlinkGameObject(_droneUIContainer);
                Sound_Events.Play_Sound("Game_Incorrect");
                return;
            }
            //purchase
            if(!freeDrone){_ocScript.CurrentPlayer.player_score -= Overallgame_Controller_Script.overallgame_controller_singleton.CurrentPlayer.PlayerDroneCost;}
            _ocScript.CurrentPlayer.PlayerDrones += 1;
            Sound_Events.Play_Sound("Game_Correct");
            Levelselect_Controller_Script.levelselect_controller_singletion.DroneCountText.text = _ocScript.CurrentPlayer.PlayerDrones + "";
            UpdateShopUINumbers();
            return;
        }
        //deny purchase
        BlinkGameObject(_inventroyIcons[0]);
    }

    public void PurchaseFuel()
    {
        if(_ocScript.CurrentPlayer.player_score >= Overallgame_Controller_Script.overallgame_controller_singleton.CurrentPlayer.PlayerFuelCost)
        {
            if(_ocScript.CurrentPlayer.PlayerFuel >= Global_Vars.max_fuel)
            {
                BlinkGameObject(_fuelUIContainer);
                Sound_Events.Play_Sound("Game_Incorrect");
                return;
            }
            //purchase
            _ocScript.CurrentPlayer.player_score -= Overallgame_Controller_Script.overallgame_controller_singleton.CurrentPlayer.PlayerFuelCost;
            _ocScript.CurrentPlayer.PlayerFuel += 1;
            Sound_Events.Play_Sound("Game_Correct");
            if(_fuelBtnScript._menuOpen){_fuelBtnScript.FinishOpenMenu();}
            UpdateShopUINumbers();
            return;
        }
        //deny purchase
        BlinkGameObject(_inventroyIcons[0]);
    }

    public void UpdateShopUINumbers()
    {
        _inventroyTexts[0].text = _ocScript.CurrentPlayer.player_score + "";
        _inventroyTexts[1].text = _ocScript.CurrentPlayer.player_dia + "";
        _inventroyTexts[2].text = _ocScript.CurrentPlayer.player_top + "";
        _inventroyTexts[3].text = _ocScript.CurrentPlayer.player_rub + "";
        if(Overallgame_Controller_Script.overallgame_controller_singleton.CurrentPlayer.DroneShields > 0){_droneShield.SetActive(true);}else{_droneShield.SetActive(false);}
    }

    public void BlinkGameObject(GameObject _obj)
    {
        if(_runningBlinkRountine != null){StopCoroutine(_runningBlinkRountine);}
        _runningBlinkRountine = BlinkUIGO(_obj);
        StartCoroutine(_runningBlinkRountine);
    }

    private IEnumerator BlinkUIGO(GameObject _obj)
    {
        for (int i = 0; i < 5; i++)
        {
            _obj.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            _obj.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
