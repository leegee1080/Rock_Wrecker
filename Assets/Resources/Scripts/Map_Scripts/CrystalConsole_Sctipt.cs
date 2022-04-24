using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalConsole_Sctipt : MonoBehaviour
{
    public static CrystalConsole_Sctipt singleton;
    [SerializeField]private GameObject _lightRayGO;

    void Start()
    {
        singleton = this;
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

}
