using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelectDropship_Script : MonoBehaviour
{
    [SerializeField]private TrailRenderer _trailRend;
    // Start is called before the first frame update

    [SerializeField]private float _scaleTime;
    [SerializeField]private float _launchTime;

    void Start()
    {
        Playerinput_Controller_Script.playerinput_controller_singleton.StartCamShake(0.5f,0.1f);

        iTween.MoveTo(gameObject, iTween.Hash(
        "position", Levelselect_Controller_Script.levelselect_controller_singletion.selected_poi.transform.position,
        "orienttopath", true,
        "looktime", 1f,
        "easetype", iTween.EaseType.easeOutExpo,
        "time", _launchTime,
        "oncompletetarget", this.gameObject,
        "oncomplete", "CompleteAnimation"));

        // iTween.MoveAdd(gameObject, iTween.Hash(
        // "amount", transform.forward * 1000f,
        // "easetype", iTween.EaseType.easeOutQuad,
        // "time", 10f,
        // "oncompletetarget", this.gameObject));

        // iTween.LookTo(gameObject, iTween.Hash(
        // "looktarget", Levelselect_Controller_Script.levelselect_controller_singletion.selected_poi.transform.position,
        // "easetype", iTween.EaseType.easeOutQuad,
        // "time", 3f));

        iTween.ScaleTo(gameObject, iTween.Hash(
        "scale", new Vector3(0.01f,0.01f,0.01f),
        "easetype", iTween.EaseType.easeOutExpo,
        "time", _scaleTime));

        iTween.ValueTo(gameObject, iTween.Hash(
        "from", 1f,
        "to", 0f,
        "easetype", iTween.EaseType.easeOutExpo,
        "time", _scaleTime,
        "onupdatetarget", this.gameObject, 
        "onupdate", "ScaleTrailRend"));
    }

    public void CompleteAnimation()
    {
        Debug.Log("complete");
        Levelselect_Controller_Script.levelselect_controller_singletion.FinishedDeploy();
    }

    public void ScaleTrailRend(float size)
    {
        _trailRend.startWidth = size;
        _trailRend.endWidth = size*2;
    }
    void Update()
    {
        
    }
}
