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
    private Dictionary<CrystalTypes, GameObject> _crystalGODict = new Dictionary<CrystalTypes, GameObject>();

    [SerializeField]private CrystalTypes _prevSelectedCrystal;
    [SerializeField]private CrystalTypes _selectedCrystal;
    [SerializeField]private bool _tappable = false;

    [Header("reward vars")]
    [SerializeField]private CrystalPrize_ScriptableObject[] _tier1RewardList;
    [SerializeField]private CrystalPrize_ScriptableObject[] _tier2RewardList;
    [SerializeField]private CrystalPrize_ScriptableObject[] _tier3RewardList;
    [SerializeField]private ParticleSystem[] _crackExplo;

    private int noneCrystalCount = -1;

    void Start()
    {
        singleton = this;
        _crystalGODict[CrystalTypes.None] = null;
        _crystalGODict[CrystalTypes.Diamond] = _diamondGO;
        _crystalGODict[CrystalTypes.Topaz] = _topazGO;
        _crystalGODict[CrystalTypes.Ruby] = _rubyGO;
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
        Sound_Events.Play_Sound("Game_Click");
        switch (_newCrystalStr)
        {
            case "Diamond":
                SelectCrystal(CrystalTypes.Diamond, false);
                return;   
            case "Topaz":
                SelectCrystal(CrystalTypes.Topaz, false);
                return;   
            case "Ruby":
                SelectCrystal(CrystalTypes.Ruby, false);
                return;   
            default:
            Debug.Log("Cryatal Translator: crystal type spelled wrong.");
                return;
        }
    }

    public void SelectCrystal(CrystalTypes _typeSelected, bool fastAcsend)
    {
        if(_typeSelected == _selectedCrystal){return;}
        _tappable = false;

        if(GrabOverallCrystalCount(_typeSelected) <= 0)
        {
            _typeSelected = CrystalTypes.None;
        }
        //new crystal null
        if(_typeSelected == CrystalTypes.None)
        {
            _prevSelectedCrystal = _selectedCrystal;
            if(_prevSelectedCrystal != CrystalTypes.None)
            {
                if(fastAcsend){FastAscendCrystal(_crystalGODict[_prevSelectedCrystal]);}else{AscendCrystal(_crystalGODict[_prevSelectedCrystal]);}
            }
            _selectedCrystal = CrystalTypes.None;
            return;
        }
        //last crystal null, new crystal 
        if(_selectedCrystal == CrystalTypes.None)
        {
            _prevSelectedCrystal = CrystalTypes.None;
            _selectedCrystal = _typeSelected;
            DescendCrystal();
            return;
        }
        //normal trans
        _prevSelectedCrystal = _selectedCrystal;
        AscendCrystal(_crystalGODict[_selectedCrystal]);
        _selectedCrystal = _typeSelected;

    }

    public void CrystalTapped()
    {
        if(_tappable == false){return;}
        MapUI_Script.singleton.CloseShopBackButton();
        Debug.Log("Crystal Tapped! "+ _selectedCrystal);
        _tappable = false;

        BreakCrystal(_crystalGODict[_selectedCrystal]);
        // CompleteBreak();
    }

    public void DescendCrystal()
    {
        Sound_Events.Delay_Play_Sound("Game_Correct", 0.1f);
        Sound_Events.Delay_Play_Sound("Game_CrystalGlow", 0.2f);
        if(_selectedCrystal == CrystalTypes.None){return;}
        _crystalGODict[_selectedCrystal].SetActive(true);
        GameObject newCrystalGO = _crystalGODict[_selectedCrystal];
        iTween.MoveTo(newCrystalGO, iTween.Hash(
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
            "time", 0.7f));
    }
    public void AscendCrystal(GameObject _crystal)
    {
        iTween.MoveTo(_crystal, iTween.Hash(
            "oncompletetarget", this.gameObject,
            "oncomplete", "CompleteAscend",
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
    public void FastAscendCrystal(GameObject _crystal)
    {
        _crystal.transform.localPosition = new Vector3(-11.84f,_crystalUpperPos,5f);
        _crystal.transform.localScale = new Vector3(1,1,1);
        _crystal.SetActive(false);
    }
    public void BreakCrystal(GameObject _crystal)
    {
        PlayExplo();
        iTween.ShakePosition(_crystal, iTween.Hash(
            "oncompletetarget", this.gameObject,
            "oncomplete", "CompleteBreak",
            "amount", new Vector3(0.1f,0.1f,0.1f),
            "time", 1.8f));
        iTween.ScaleTo(_crystal, iTween.Hash(
            "scale", new Vector3(0.7f,0.7f,0.7f),
            "islocal", true,
            "easetype", iTween.EaseType.easeOutSine,
            "time", 1.4f));

    }

    public void CompleteTrans()
    {
        if(_prevSelectedCrystal != CrystalTypes.None)
        {
            _crystalGODict[_prevSelectedCrystal].SetActive(false);
        }
        _tappable = true;
    }

    public void CompleteAscend()
    {
        if(_prevSelectedCrystal != CrystalTypes.None)
        {
            _crystalGODict[_prevSelectedCrystal].SetActive(false);
        }
        DescendCrystal();
    }

    public void PlayExplo()
    {
        Sound_Events.Stop_Sound("Game_CrystalGlow");
        Sound_Events.Play_Sound("Game_PrizeOpen");
        _crackExplo[(int)_selectedCrystal].Play();
    }

    public void CompleteBreak()
    {
        // _crackExplo.Play();
        GrabOverallCrystalCount(_selectedCrystal) -= 1;
        MapUI_Script.singleton.UpdateShopUINumbers();
        GivePrizeSO();
        SelectCrystal(CrystalTypes.None, true);
        CrystalPrizes_Script.singleton.OpenPrizes();
    }

    private void GivePrizeSO()
    {
        List<CrystalPrize_ScriptableObject> combinedArray;
        int randomNumber;

        switch (_selectedCrystal)
        {
            case CrystalTypes.None:
                Debug.Log("No crystal selected"); return; 
            case CrystalTypes.Diamond:
                combinedArray = new List<CrystalPrize_ScriptableObject>();
                combinedArray.AddRange(_tier2RewardList);
                combinedArray.AddRange(_tier3RewardList);
                for (int i = 0; i < 3; i++)
                {
                    randomNumber = Random.Range(0, combinedArray.Count);
                    CrystalPrizes_Script.singleton._prizes[i] = combinedArray[randomNumber];
                }
                return;
            case CrystalTypes.Topaz:
                for (int i = 0; i < 3; i++)
                {
                    randomNumber = Random.Range(0, _tier1RewardList.Length);
                    CrystalPrizes_Script.singleton._prizes[i] = _tier1RewardList[randomNumber];
                }
                return;  
            case CrystalTypes.Ruby:
                combinedArray = new List<CrystalPrize_ScriptableObject>();
                combinedArray.AddRange(_tier1RewardList);
                combinedArray.AddRange(_tier2RewardList);
                for (int i = 0; i < 3; i++)
                {
                    randomNumber = Random.Range(0, combinedArray.Count);
                    CrystalPrizes_Script.singleton._prizes[i] = combinedArray[randomNumber];
                }
                return;
            default:
                Debug.Log("returning none due do incorrect crystal type (GivePrizeSO)");
                Debug.Log("No crystal selected"); return; 
        }
    }


    public ref int GrabOverallCrystalCount(CrystalTypes _typeSelected)
    {
        switch (_typeSelected)
        {
            case CrystalTypes.None:
                return ref noneCrystalCount;
            case CrystalTypes.Diamond:
                return ref Overallgame_Controller_Script.overallgame_controller_singleton.CurrentPlayer.player_dia;
            case CrystalTypes.Topaz:
                return ref Overallgame_Controller_Script.overallgame_controller_singleton.CurrentPlayer.player_top;  
            case CrystalTypes.Ruby:
                return ref Overallgame_Controller_Script.overallgame_controller_singleton.CurrentPlayer.player_rub;
            default:
                Debug.Log("returning none due do incorrect crystal type (GrabOverallCrystalCount)");
               return ref noneCrystalCount;
        }
        
    }
}   
