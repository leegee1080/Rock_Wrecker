using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Levelselect_Controller_Script : MonoBehaviour
{
    public static Levelselect_Controller_Script levelselect_controller_singletion;

    void Awake() => levelselect_controller_singletion = this;
}
