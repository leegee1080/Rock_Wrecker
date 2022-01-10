using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalLoading_Script : MonoBehaviour
{
    private void Start()
    {
        //check to make this script doesn't get stuck on the loading scene
        Loading_Controller_Script.loading_controller_singleton.Load_Next_Scene();

    }
}
