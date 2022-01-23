using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable_GridResident 
{
    public void Tapped();
}

public class GridResident_Script : MonoBehaviour
{
    public Vector2 grid_pos;
    public bool moveable;

    public virtual bool Place_Resident(Vector2 new_pos){

        if (Levelplay_Controller_Script.levelplay_controller_singleton.map_coord_dict[new_pos].resident == null)
        {
            Levelplay_Controller_Script.levelplay_controller_singleton.map_coord_dict[new_pos].resident = this;
            gameObject.transform.position = Levelplay_Controller_Script.levelplay_controller_singleton.map_coord_dict[new_pos].actual_pos;
            grid_pos = new_pos;
            return true;
        }
        return false;
    }

    public virtual bool Swap_Residents(GridResident_Script first_res, GridResident_Script second_res){
        if(Levelplay_Controller_Script.levelplay_controller_singleton.map_coord_dict[first_res.grid_pos].resident != null && Levelplay_Controller_Script.levelplay_controller_singleton.map_coord_dict[first_res.grid_pos].resident != null)
        {
            Vector2 first_res_grid_pos = first_res.grid_pos;
            Vector2 second_res_grid_pos = second_res.grid_pos;

            Levelplay_Controller_Script.levelplay_controller_singleton.map_coord_dict[first_res_grid_pos].resident = null;
            Levelplay_Controller_Script.levelplay_controller_singleton.map_coord_dict[second_res_grid_pos].resident = null;

            second_res.Place_Resident(first_res_grid_pos);
            first_res.Place_Resident(second_res_grid_pos);
            return true;
        }
        return false;
    }
}
