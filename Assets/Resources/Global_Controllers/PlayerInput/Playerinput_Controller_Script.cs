using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Playerinput_Controller_Script : MonoBehaviour
{
    public static Playerinput_Controller_Script playerinput_controller_singleton;

    private PlayerInputActions player_input_actions;

    public Event tap_down_event;
    public Event tap_up_event;


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

        player_input_actions = new PlayerInputActions();
    }

    private void Tap_Down(){

    }

    private void Tap_Up(){
        
    }

    private void OnEnable(){
        player_input_actions.Enable();
    }

    private void OnDisable(){
        player_input_actions.Disable();
    }
}
