using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Playerinput_Controller_Script : MonoBehaviour
{
    public static Playerinput_Controller_Script playerinput_controller_singleton;

    public Event tap_down;
    public Event tap_up;


    private void Awake()
    {
        if (playerinput_controller_singleton == null)
        {
            playerinput_controller_singleton = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this);
    }
}
