using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TapableObject_Script : MonoBehaviour
{

    public UnityEvent tap_event;

    public void Call_Tap_Event(){
        tap_event?.Invoke();
    }
}
