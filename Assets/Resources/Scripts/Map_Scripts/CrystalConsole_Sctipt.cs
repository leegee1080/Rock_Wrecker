using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalConsole_Sctipt : MonoBehaviour
{
    public static CrystalConsole_Sctipt singleton;
    [SerializeField]private GameObject _lightRayGO;
    [SerializeField]private float _crystalUpperPos;
    [SerializeField]private float _crystalLowerPos;

    [SerializeField]private GameObject _diamondGO;
    [SerializeField]private GameObject _topazGO;
    [SerializeField]private GameObject _rubyGO;
    private Dictionary<CrystalTypes, GameObject> _crystalDict = new Dictionary<CrystalTypes, GameObject>();

    [SerializeField]private CrystalTypes _prevSelectedCrystal;
    [SerializeField]private CrystalTypes _selectedCrystal;
    [SerializeField]private bool _tappable = false;

    void Start()
    {
        singleton = this;
        _crystalDict[CrystalTypes.Diamond] = _diamondGO;
        _crystalDict[CrystalTypes.Topaz] = _topazGO;
        _crystalDict[CrystalTypes.Ruby] = _rubyGO;
    }


    public void ShowCrystalTable()
    {
        iTween.ScaleTo(_lightRayGO, iTween.Hash(
            "scale", new Vector3(0.12f,1,1),
            "islocal", true,
            "easetype", iTween.EaseType.easeOutSine,
            "time", 1f));
    }

    public void HideCrystalTable()
    {
        iTween.ScaleTo(_lightRayGO, iTween.Hash(
            "scale", new Vector3(1,1,1),
            "islocal", true,
            "easetype", iTween.EaseType.easeOutSine,
            "time", 1f));
    }
    public void SelectButtonTranslator(string _newCrystalStr)
    {   
        
        switch (_newCrystalStr)
        {
            case "Diamond":
                SelectCrystal(CrystalTypes.Diamond);
                return;   
            case "Topaz":
                SelectCrystal(CrystalTypes.Topaz);
                return;   
            case "Ruby":
                SelectCrystal(CrystalTypes.Ruby);
                return;   
            default:
            Debug.Log("Cryatal Translator: crystal type spelled wrong.");
                return;
        }
    }

    public void SelectCrystal(CrystalTypes _typeSelected)
    {
        if(_typeSelected == _selectedCrystal){return;}
        _tappable = false;
        //new crystal null
        if(_typeSelected == CrystalTypes.None)
        {
            if(_prevSelectedCrystal != CrystalTypes.None)
            {
                _prevSelectedCrystal = _selectedCrystal;
                AscendCrystal(_crystalDict[_prevSelectedCrystal]);
            }
            _selectedCrystal = CrystalTypes.None;
            return;
        }
        //lasst crystal null, new crystal 
        if(_selectedCrystal == CrystalTypes.None)
        {
            _prevSelectedCrystal = CrystalTypes.None;
            _selectedCrystal = _typeSelected;
            DescendCrystal();
            return;
        }
        //normal trans
        _prevSelectedCrystal = _selectedCrystal;
        AscendCrystal(_crystalDict[_selectedCrystal]);
        _selectedCrystal = _typeSelected;

    }

    public void DescendCrystal()
    {
        if(_selectedCrystal == CrystalTypes.None){return;}
        _crystalDict[_selectedCrystal].SetActive(true);
        GameObject newCrystalGO = _crystalDict[_selectedCrystal];
        iTween.MoveTo(newCrystalGO, iTween.Hash(
            "name", "Desc",
            "oncompletetarget", this.gameObject,
            "oncomplete", "CompleteTrans",
            "y", _crystalLowerPos,
            "easetype", iTween.EaseType.easeOutSine,
            "time", .8f,
            "islocal", true));
        iTween.ScaleTo(newCrystalGO, iTween.Hash(
            "scale", new Vector3(2,2,2),
            "islocal", true,
            "easetype", iTween.EaseType.easeOutSine,
            "time", 1));
    }
    public void AscendCrystal(GameObject _crystal)
    {
        iTween.MoveTo(_crystal, iTween.Hash(
            "name", "Asc",
            "oncompletetarget", this.gameObject,
            "oncomplete", "DescendCrystal",
            "y", _crystalUpperPos,
            "easetype", iTween.EaseType.easeOutSine,
            "time", .4f,
            "islocal", true));
        iTween.ScaleTo(_crystal, iTween.Hash(
            "scale", new Vector3(1,1,1),
            "islocal", true,
            "easetype", iTween.EaseType.easeOutSine,
            "time", .2f));
    }

    public void CompleteTrans()
    {
        if(_prevSelectedCrystal != CrystalTypes.None)
        {
            _crystalDict[_prevSelectedCrystal].SetActive(false);
        }
        _tappable = true;
    }

    public void CrystalTapped()
    {
        if(_tappable == false){return;}
        Debug.Log("Crystal Tapped! "+ _selectedCrystal);
    }

}
