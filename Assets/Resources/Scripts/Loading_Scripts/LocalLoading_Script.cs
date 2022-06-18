using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalLoading_Script : MonoBehaviour
{
    [SerializeField]private float _loadingDelay;
    private void Start()
    {
        Audio_Controller_Script.singleton.SelectSound("Game_Loading");
        Audio_Controller_Script.singleton.FadeSoundIn(Audio_Controller_Script.singleton.currentGameVolumeLevel, 0.06f);
        Sound_Events.Play_Sound("Game_Loading");
        // Playerinput_Controller_Script.playerinput_controller_singleton.camera_controls_allowed =false;

        StartCoroutine(IFinished_Loading_Animation());
    }



    private IEnumerator IFinished_Loading_Animation()
    {
        yield return new WaitForSeconds(_loadingDelay);

        Audio_Controller_Script.singleton.SelectSound("Game_Loading");
        Audio_Controller_Script.singleton.FadeSoundOut(0.1f);
        ScnTrans_Script.singleton.ScnTransOut();
        // Loading_Controller_Script.loading_controller_singleton.Load_Next_Scene();
    }

}
