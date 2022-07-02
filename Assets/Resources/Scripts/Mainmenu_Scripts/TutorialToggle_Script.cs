using UnityEngine;
using UnityEngine.UI;

public class TutorialToggle_Script : MonoBehaviour
{
    [SerializeField]private Image _btn;
    [SerializeField]private Color _diabledColor;
    [SerializeField]private Color _normalColor;


    public void UpdateButton()
    {
        if(Overallgame_Controller_Script.overallgame_controller_singleton.tutOn)
        {
            _btn.color = _normalColor;
            return;
        }
        _btn.color = _diabledColor;
    }

    public void ToggleTut()
    {
        if(Overallgame_Controller_Script.overallgame_controller_singleton.tutOn)
        {
            Overallgame_Controller_Script.overallgame_controller_singleton.tutOn =false;
            PlayerPrefs.SetInt("tut",0);
            _btn.color = _diabledColor;
            AchevementManager.singlton.TutOff();
            return;
        }
        Overallgame_Controller_Script.overallgame_controller_singleton.tutOn =true;
        PlayerPrefs.SetInt("tut",1);
        _btn.color = _normalColor;
    }

}
