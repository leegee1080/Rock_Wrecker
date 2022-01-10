using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalLoading_Script : MonoBehaviour
{
    private void Start()
    {
        //check to make this script doesn't get stuck on the loading scene
        StartCoroutine(IFinished_Loading_Animation());
    }



        private IEnumerator IFinished_Loading_Animation()
    {
        yield return new WaitForSeconds(1f);
        Loading_Controller_Script.loading_controller_singleton.Load_Next_Scene();
    }
}
