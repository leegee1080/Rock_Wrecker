using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Level_Actor_States_Enum
{   
    Normal,
    Dead,
    Frozen,
    Pause
}

public class LevelActor_Script : MonoBehaviour
{
    public Level_Actor_States_Enum current_state
    {
        get { return current_state; }
        set {
                current_state = value; 
                Debug.Log(this.name + " state changed to: "+ value); 
            }
    }

    public bool Place_Actor(Vector2 new_pos){

        if (Levelplay_Controller_Script.levelplay_controller_singleton.map_coord_dict[new_pos].resident != null)
        {
            gameObject.transform.position = Levelplay_Controller_Script.levelplay_controller_singleton.map_coord_dict[new_pos].actual_pos;
            return true;
        }
        return false;
    }
}
