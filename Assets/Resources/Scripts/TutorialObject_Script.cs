using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class TutorialClass
{
    public string name;
    public string tutText;

    public TutorialClass(string _name, string _newText)
    {
        name = _name;
        tutText = _newText;
    }
}

public class TutorialObject_Script : MonoBehaviour
{
    [SerializeField]TMP_Text _text;
    [SerializeField]Animator _ani;
    [SerializeField]GameObject _bot;
    [SerializeField]float _botSpeed;
    [SerializeField]float _typeSpeed;
    [SerializeField]TutorialClass _tutClass;
    public static TutorialObject_Script singleton;

    

    public Vector3 TalkPos;
    public Vector3 HidePos;
    // Start is called before the first frame update
    void Start()
    {
        singleton = this;
        _bot.transform.position = HidePos;
    }
    public void FirstTut()
    {
        Overallgame_Controller_Script.overallgame_controller_singleton.tutOn = true;
        StartTutorial(new TutorialClass("firsttut", "Welcome to Rock Wrecker! \nI will be your guide. \n If this is your first time playing, click the 'Play' button. \n If not, click the 'Extras' button and you can turn off tutorials.\n You can go back by clicking the 'Contiune' button."));
    }

    public void StartTutorial(TutorialClass newTC)
    {
        _tutClass = newTC;
        _text.text = "";
        iTween.MoveTo(_bot, 
            iTween.Hash
            (
                "name", "tutIn",
                "position", TalkPos,
                "speed", _botSpeed,
                "easetype", iTween.EaseType.easeOutBack,
                "oncomplete", "ShowTutorialScreen",
                "oncompletetarget", this.gameObject
            )
        );
    }
    public void StopTutorial()
    {
        iTween.StopByName("tutIn");
        HideTutorialScreen();
        iTween.MoveTo(_bot, 
            iTween.Hash
            (
                "name", "tutOut",
                "position", HidePos,
                "speed", _botSpeed,
                "easetype", iTween.EaseType.easeInSine
            )
        );
    }

    public void ShowTutorialScreen()
    {
        _ani.Play("Open");
    }
    public void HideTutorialScreen()
    {
        _ani.Play("Close");
    }

    public void FillOutText()
    {
        StartCoroutine(TypeText(_tutClass.tutText));
    }
    private IEnumerator TypeText(string _stringToType)
    {
        for (int i = 0; i < _stringToType.Length; i++)
        {
            yield return new WaitForSeconds(_typeSpeed);
            _text.text += _stringToType[i];
        }
        yield return null;
    }
}
