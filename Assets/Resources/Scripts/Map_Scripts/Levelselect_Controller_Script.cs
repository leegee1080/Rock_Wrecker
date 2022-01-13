using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Levelselect_Controller_Script : MonoBehaviour
{
    public static Levelselect_Controller_Script levelselect_controller_singletion;

    private Vector2 cursor_pos = Vector2.zero;

    void Awake() => levelselect_controller_singletion = this;

    public void Fire_POIselect_Raycast(InputAction.CallbackContext input_context){

            if(input_context.phase == InputActionPhase.Canceled){
                print("fired");
                cursor_pos = input_context.ReadValue<Vector2>();
                print(cursor_pos);
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(cursor_pos);
                if (Physics.Raycast(ray, out hit))
                {
                    // Check if hit.transform is door, 
                    print("hit "+ hit.transform.name);
                }
            }

    }
}
