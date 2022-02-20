using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridResident_Script : MonoBehaviour
{
    public Vector2Int grid_pos;
    public bool moveable;
    public bool matchable;


    public virtual void Start() {
        name = grid_pos.ToString();
    }

    public virtual bool Place_Resident(Vector2Int new_pos)
    {
        if (Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[new_pos.x][new_pos.y].resident == null)
        {
            Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[new_pos.x][new_pos.y].resident = this;
            gameObject.transform.position = Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[new_pos.x][new_pos.y].actual_pos;
            grid_pos = new_pos;
            return true;
        }
        return false;
    }

    public virtual bool Swap_Residents(GridResident_Script first_res, GridResident_Script second_res)
    {
        if(Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[first_res.grid_pos.x][first_res.grid_pos.y].resident != null && Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[first_res.grid_pos.x][first_res.grid_pos.y].resident != null)
        {
            Vector2Int first_res_grid_pos = first_res.grid_pos;
            Vector2Int second_res_grid_pos = second_res.grid_pos;

            Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[first_res_grid_pos.x][first_res_grid_pos.y].resident = null;
            Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[second_res_grid_pos.x][second_res_grid_pos.y].resident = null;

            second_res.Place_Resident(first_res_grid_pos);
            first_res.Place_Resident(second_res_grid_pos);
            
            second_res.Local_Board_Changed();
            first_res.Local_Board_Changed();

            return true;
        }
        return false;
    }

    public virtual void Local_Board_Changed(){
        // Level_Events.Invoke_Board_Changed_Event();
    }

    public virtual void OnDestroy()
    {

    }
}
