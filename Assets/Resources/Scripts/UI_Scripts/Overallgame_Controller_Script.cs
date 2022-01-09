using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overallgame_Controller_Script : MonoBehaviour
{
    public static Overallgame_Controller_Script overallgame_controller_singleton;
    private void Awake()
    {
        //Checks to make sure there is only one SM
        if (overallgame_controller_singleton == null)
        {
            overallgame_controller_singleton = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this);
    }
}
