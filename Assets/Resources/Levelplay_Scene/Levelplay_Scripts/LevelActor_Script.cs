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

    public bool Place_Actor(Vector2Int new_pos){

        if (Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[new_pos.x][new_pos.y].resident != null)
        {
            gameObject.transform.position = Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[new_pos.x][new_pos.y].actual_pos;
            return true;
        }
        return false;
    }
}
