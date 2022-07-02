using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class TutorialClass
{
    public string name;
    public string tutText;
    public bool shown;
    public GameObject[] ToHide;

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
    [SerializeField]TutorialClass _firstTutClass;

    [SerializeField]TutorialClass _tutClass;
    public static TutorialObject_Script singleton;
    public TutorialClass[] TutArray;

    

    public Vector3 TalkPos;
    public Vector3 HidePos;
    // Start is called before the first frame update
    private void Awake()
    {
        singleton = this;
    }
    void Start()
    {
        _bot.transform.localPosition = HidePos;
    }
    public void FirstTut()
    {
        Overallgame_Controller_Script.overallgame_controller_singleton.tutOn = true;
        StartTutorial(_firstTutClass);
    }

    public void StartTutorial(TutorialClass newTC)
    {
        if(!Overallgame_Controller_Script.overallgame_controller_singleton.tutOn || newTC.shown || Overallgame_Controller_Script.overallgame_controller_singleton.shownTuts.Contains(newTC.name)){return;}
        _bot.SetActive(true);
        foreach (GameObject item in newTC.ToHide)
        {
            item.SetActive(false);
        }
        newTC.shown = true;
        Overallgame_Controller_Script.overallgame_controller_singleton.shownTuts.Add(newTC.name);
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
                "oncompletetarget", this.gameObject,
                "islocal", true
            )
        );
    }
    public void StopTutorial()
    {
        iTween.StopByName(this.gameObject,"tutIn");
        HideTutorialScreen();
        iTween.MoveTo(_bot, 
            iTween.Hash
            (
                "name", "tutOut",
                "position", HidePos,
                "speed", _botSpeed,
                "easetype", iTween.EaseType.easeInSine,
                "oncomplete", "FinishCloseTut",
                "oncompletetarget", this.gameObject,
                "islocal", true
            )
        );
        foreach (GameObject item in _tutClass.ToHide)
        {
            item.SetActive(true);
        }
    }

    public void ShowTutorialScreen()
    {
        _ani.Play("Open");
    }
    public void HideTutorialScreen()
    {
        _ani.Play("Close");
    }

    public void FinishCloseTut()
    {
        _bot.SetActive(false);
    }

    public void FillOutText()
    {
        StartCoroutine(TypeText(_tutClass.tutText));
    }
    private IEnumerator TypeText(string _stringToType)
    {
        string s = _stringToType.Replace("*", "\n");
        for (int i = 0; i < s.Length; i++)
        {
            yield return new WaitForSeconds(_typeSpeed);
            _text.text += s[i];
        }
        yield return null;
    }

    public void FindandPlayTutorialObject(string name)
    {

        StartTutorial(System.Array.Find(TutArray, tut => tut.name == name));
    }

    public void ResetTutorialObjects()
    {
        foreach (TutorialClass item in TutArray)
        {
            item.shown = false;
        }
    }
}
