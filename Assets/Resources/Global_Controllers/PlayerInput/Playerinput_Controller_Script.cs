using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Playerinput_Controller_Script : MonoBehaviour
{
    public static Playerinput_Controller_Script playerinput_controller_singleton;

    private PlayerInputActions player_input_actions;

    [Header("TapConfirm Vars")]
    private TapableObject_Script tapped_object;

    [Header("TapDrag Vars")]
    private Vector2 drag_start;
    private bool drag_started;
    private Vector2 dragged_direction;

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

    private void Start() {
        player_input_actions.PlayerControls.TapDown.started += context => Tap_Down(context);
        player_input_actions.PlayerControls.TapUp.canceled += context => Tap_Up(context);
        player_input_actions.PlayerControls.TapDrag.performed += context => Tap_Drag_Started(context);
        player_input_actions.PlayerControls.TapDrag.canceled += context => Tap_Drag_Ended(context);
    }

    private void Tap_Down(InputAction.CallbackContext context){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(player_input_actions.PlayerControls.TapPOS.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.GetComponent<TapableObject_Script>() != null)
        {
            tapped_object = hit.transform.gameObject.GetComponent<TapableObject_Script>();
            return;
        }
        tapped_object = null;
    }

    private void Tap_Up(InputAction.CallbackContext context){
        if(tapped_object == null){return;}
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(player_input_actions.PlayerControls.TapPOS.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.GetComponent<TapableObject_Script>() == tapped_object)
        {
            print("hit "+ hit.transform.name);
        }
    }
    
    private void Tap_Drag_Started(InputAction.CallbackContext context){
        drag_started = true;
        drag_start = player_input_actions.PlayerControls.TapPOS.ReadValue<Vector2>();
    }

    private void Tap_Drag_Ended(InputAction.CallbackContext context){
        if(!drag_started){return;}
        print("tapdrag performed direction: " + (drag_start - player_input_actions.PlayerControls.TapPOS.ReadValue<Vector2>()).normalized);
        drag_started = false;
    }

    private void OnEnable(){
        player_input_actions.Enable();
    }

    private void OnDisable(){
        player_input_actions.Disable();
    }
}
