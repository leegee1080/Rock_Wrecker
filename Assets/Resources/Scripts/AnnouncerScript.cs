using System.Collections;
using TMPro;
using System;
using UnityEngine;

public enum AnnounceTypeEnum
{
    None,
    BigText,
    SmallText,
    TwoBtn,
    OneBtn
}

[System.Serializable]
public class AnnouncementPackage
{
    public string name;
    public AnnounceTypeEnum type;
    public string bigTxt;
    public string smallTxt;
    public Func<bool, bool>  btnFunc;
    public AnnouncementPackage(string _name, AnnounceTypeEnum _type, string _bigTxt = "", string _smallTxt = "", Func<bool, bool> _btnFunc = default)
    {
        name = _name;
        type = _type;
        bigTxt = _bigTxt;
        smallTxt = _smallTxt;
        btnFunc = _btnFunc;
    }
}

public class AnnouncerScript : MonoBehaviour
{
    [Header("Announce Vars")]
    [SerializeField]private float _typeSpeed;
    public AnnouncementPackage AnnouncementClass;
    private bool _openState;
    private AnnounceTypeEnum _announceType;
    public static AnnouncerScript singleton;


    [Header("GameObjects")]
    [SerializeField]private GameObject _announceCanv;
    [SerializeField]private Animator _announceAni;
    [SerializeField]private GameObject _twoBtnCanv;
    [SerializeField]private GameObject _oneBtnCanv;
    [SerializeField]private GameObject _bigTxtCanv;
    [SerializeField]private TMP_Text _bigTxt;
    [SerializeField]private GameObject _smallTxtCanv;
    [SerializeField]private TMP_Text _smallTxt;

    void OnGUI()
    {
        if (GUI.Button(new Rect(100, 100, 50, 50), "Open"))
        {
            ChangeOpenState(true);
        }
        if (GUI.Button(new Rect(100, 200, 50, 50), "Close"))
        {
            ChangeOpenState(false);
        }
    }

    private void Start()
    {
        singleton = this;
    }


    public void ChangeOpenState(bool state)
    {
        if(_openState == state){return;}
        if(AnnouncementClass != null){_announceType = AnnouncementClass.type;}
        _announceCanv.SetActive(true);
        _announceAni.enabled = true;
        _openState = state;
        _announceAni.SetBool("Open", state);
        if(state == false)
        {
            StopAllCoroutines();
            _bigTxtCanv.SetActive(false);
            _twoBtnCanv.SetActive(false);
            _oneBtnCanv.SetActive(false);
            _smallTxtCanv.SetActive(false);
        }

    }

    private void HideContent()
    {
        _announceCanv.SetActive(false);
        _announceAni.enabled = false;
    }

    private void ShowContent()
    {
        switch (_announceType)
        {
            case AnnounceTypeEnum.None:
                Debug.LogWarning("No announce passed!");
                return;
            case AnnounceTypeEnum.BigText:
                _bigTxt.text = "";
                _bigTxtCanv.SetActive(true);
                StartCoroutine(TypeBigText(AnnouncementClass.bigTxt));
                _bigTxt.rectTransform.localPosition = new Vector3(0,0,0);
                break;
            case AnnounceTypeEnum.SmallText:
                _bigTxt.text = "";
                _bigTxtCanv.SetActive(true);
                _bigTxt.rectTransform.localPosition = new Vector3(0,138,0);
                StartCoroutine(TypeBigText(AnnouncementClass.bigTxt));
                _smallTxt.text = "";
                _smallTxtCanv.SetActive(true);
                StartCoroutine(TypeSmallText(AnnouncementClass.smallTxt));
                break;
            case AnnounceTypeEnum.TwoBtn:
                _bigTxt.text = "";
                _bigTxtCanv.SetActive(true);
                _bigTxt.rectTransform.localPosition = new Vector3(0,138,0);
                StartCoroutine(TypeBigText(AnnouncementClass.bigTxt));
                _twoBtnCanv.SetActive(true);
                _smallTxt.text = "";
                _smallTxtCanv.SetActive(true);
                StartCoroutine(TypeSmallText(AnnouncementClass.smallTxt));
                break;
            case AnnounceTypeEnum.OneBtn:
                _bigTxt.text = "";
                _bigTxtCanv.SetActive(true);
                _bigTxt.rectTransform.localPosition = new Vector3(0,138,0);
                StartCoroutine(TypeBigText(AnnouncementClass.bigTxt));
                _oneBtnCanv.SetActive(true);
                _smallTxt.text = "";
                _smallTxtCanv.SetActive(true);
                StartCoroutine(TypeSmallText(AnnouncementClass.smallTxt));
                break;
            default:
                Debug.LogWarning("No announce passed!");
                return;
        }


    }

    private IEnumerator TypeBigText(string _stringToType)
    {
        for (int i = 0; i < _stringToType.Length; i++)
        {
            yield return new WaitForSeconds(_typeSpeed);
            _bigTxt.text += _stringToType[i];
        }
        yield return null;
    }
    private IEnumerator TypeSmallText(string _stringToType)
    {
        for (int i = 0; i < _stringToType.Length; i++)
        {
            yield return new WaitForSeconds(_typeSpeed/2);
            _smallTxt.text += _stringToType[i];
        }
        yield return null;
    }

    public void YesButton()
    {
        ChangeOpenState(false);
        AnnouncementClass.btnFunc(true);
    }
    public void NoButton()
    {
        ChangeOpenState(false);
        AnnouncementClass.btnFunc(false);
    }
    public void ContinueButton()
    {
        ChangeOpenState(false);
        AnnouncementClass.btnFunc(true);
    }
}
