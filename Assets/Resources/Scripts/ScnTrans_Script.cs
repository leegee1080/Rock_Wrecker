using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScnTrans_Script : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float _transSpeed;
    [SerializeField] private float _transYDist;
    [SerializeField] private RectTransform _imageTrasform;
    public static ScnTrans_Script singleton;

    void Start()
    {
        singleton = this;
        _imageTrasform.gameObject.SetActive(true);
        ScnTransIn();
    }

    public void ScnTransIn()
    {
        iTween.ValueTo(gameObject, iTween.Hash(
        "from", 0f,
        "to", -_transYDist,
        "easetype", iTween.EaseType.easeInOutSine,
        "time", _transSpeed,
        "onupdatetarget", this.gameObject, 
        "onupdate", "SlideBG"));
    }
    public void ScnTransOut(Scene_Enums scnToLoad = default)
    {
        iTween.ValueTo(gameObject, iTween.Hash(
        "from", _transYDist,
        "to", 0f,
        "easetype", iTween.EaseType.easeInOutSine,
        "time", _transSpeed,
        "onupdatetarget", this.gameObject, 
        "onupdate", "SlideBG",
        "oncompletetarget", this.gameObject,
        "oncomplete", "LoadNextScene",
        "oncompleteparams", scnToLoad));
    }

    public void SlideBG(float pos)
    {
        _imageTrasform.localPosition = new Vector3(pos,0,0);
    }

    public void LoadNextScene(Scene_Enums nextScene = default)
    {
        if(nextScene == default){Loading_Controller_Script.loading_controller_singleton.Load_Next_Scene(); return;}
        Loading_Controller_Script.loading_controller_singleton.Load_Next_Scene(nextScene);
    }
}
