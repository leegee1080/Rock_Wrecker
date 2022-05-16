using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelBtn_Script : MonoBehaviour
{
    public static FuelBtn_Script singleton;
    [SerializeField]private RectTransform _fuelContainer;
    [SerializeField]private GameObject _fuelTicksContainer;
    [SerializeField]public bool _menuOpen;
    [SerializeField]public bool MenuDisabled;
    [SerializeField]private float _tickAnimationSpeed;
    [SerializeField]private float _menuOpenSpeed;


    private void Awake()
    {
        singleton = this;
    }


    public void ToggleHud()
    {
        if(MenuDisabled){return;}
        if(_menuOpen)
        {
            Sound_Events.Play_Sound("Game_ClickOff");
            _menuOpen = false;
            HideTicks();
            return;
        }
        Sound_Events.Play_Sound("Game_Click");
        _menuOpen = true;
        SlideIn(0, 1);
    }

    private void HideTicks()
    {
        foreach (Transform tick in _fuelTicksContainer.transform)
        {
            tick.gameObject.SetActive(false);
        }
        SlideIn(1, 0);
    }
    private IEnumerator AnimateTicks()
    {
        foreach (Transform tick in _fuelTicksContainer.transform)
        {
            if(_menuOpen ==false){break;}
            Sound_Events.Play_Sound("Game_Correct");
            tick.gameObject.SetActive(true);
            yield return new WaitForSeconds(_tickAnimationSpeed);
        }   
        foreach (Transform tick in _fuelTicksContainer.transform)
        {
            if(_menuOpen ==false){break;}
            tick.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(_tickAnimationSpeed);
        for (int i = 0; i < Overallgame_Controller_Script.overallgame_controller_singleton.CurrentPlayer.PlayerFuel; i++)
        {
            if(_menuOpen ==false){break;}
            _fuelTicksContainer.transform.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForSeconds(_tickAnimationSpeed);
        }
    }
    public void SlideIn(float _start, float _finish)
    {
        iTween.ValueTo(_fuelContainer.gameObject, iTween.Hash(
            "from", _start,
            "to", _finish,
            "easetype", iTween.EaseType.easeOutSine,
            "time", _menuOpenSpeed,
            "onupdatetarget", this.gameObject, 
            "onupdate", "MoveGuiElement",
            "oncomplete", "FinishOpenMenu",
            "oncompletetarget", this.gameObject));
    }
    public void MoveGuiElement(float yScale)
    {
        _fuelContainer.localScale = new Vector3(1,yScale,1);
    }

    public void FinishOpenMenu()
    {
        StartCoroutine(AnimateTicks());
    }


}
