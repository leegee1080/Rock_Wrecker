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
    [SerializeField]private float camera_drag_speed; 
    private Vector2 drag_start;
    private bool drag_started;
    private Vector2 drag_dist;

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
        // player_input_actions.PlayerControls.TapDrag.performed += context => Tap_Drag_Started(context);
        // player_input_actions.PlayerControls.TapDrag.canceled += context => Tap_Drag_Ended(context);
    }

    private void Tap_Down(InputAction.CallbackContext context){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(player_input_actions.PlayerControls.TapPOS.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.GetComponent<TapableObject_Script>() != null)
        {
            tapped_object = hit.transform.gameObject.GetComponent<TapableObject_Script>();
        }
        drag_start = player_input_actions.PlayerControls.TapPOS.ReadValue<Vector2>();
        drag_started = true;
    }

    private void Tap_Up(InputAction.CallbackContext context){
        if (drag_started) {drag_started = false;}
        if(tapped_object == null){return;}
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(player_input_actions.PlayerControls.TapPOS.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.GetComponent<TapableObject_Script>() == tapped_object)
        {
            print("hit "+ hit.transform.name);
        }
        tapped_object = null;
    }

    private void Update(){
        if(drag_started){
            drag_dist = drag_start -  player_input_actions.PlayerControls.TapPOS.ReadValue<Vector2>();
            Vector3 cam_move = new Vector3(drag_dist.x, drag_dist.y, 0) * (camera_drag_speed /1000);
            Camera.main.transform.position += new Vector3(cam_move.x, cam_move.y, 0);
            drag_start = player_input_actions.PlayerControls.TapPOS.ReadValue<Vector2>();
        }
    }

    // void Update () {
    //     if (drag_started) {
    //         drag_start = new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera_z_offset);
    //         drag_start = Camera.main.ScreenToWorldPoint (drag_start);
    //         Camera.main.transform.position = drag_start;

    //     } 
    //     else if (drag_started) {
    //         var MouseMove = new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera_z_offset);
    //         MouseMove = Camera.main.ScreenToWorldPoint (MouseMove);
    //         MouseMove.z = transform.position.z;
    //         transform.position = transform.position - (MouseMove - drag_start);
    //     }
// }


    private void OnEnable(){
        player_input_actions.Enable();
    }

    private void OnDisable(){
        player_input_actions.Disable();
    }
}
