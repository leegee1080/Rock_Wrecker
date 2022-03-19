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
        Level_Events.pause_toggle_event += PauseResident;
    }

    public virtual Vector3 Place_Resident(Vector2Int new_pos)
    {
        if (Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[new_pos.x][new_pos.y].resident == null)
        {
            Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[new_pos.x][new_pos.y].resident = this;
            gameObject.transform.position = Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[new_pos.x][new_pos.y].actual_pos;
            grid_pos = new_pos;
            return gameObject.transform.position;
        }
        return Vector3.zero;
    }
    public virtual bool PlaceResidentTween(Vector2Int new_pos, float time, bool hop)
    {
        Hashtable _moveHash = iTween.Hash
        (
            // "position", Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[new_pos.x][new_pos.y].actual_pos,
            "x", Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[new_pos.x][new_pos.y].actual_pos.x,
            "y", Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[new_pos.x][new_pos.y].actual_pos.y,
            "time", time
        );
        Hashtable _bounceToPeak = iTween.Hash
        (
            "x", Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[new_pos.x][new_pos.y].actual_pos.x,
            "y", Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[new_pos.x][new_pos.y].actual_pos.y,
            "Z", -3f,
            "time", time/4,
            "easetype", iTween.EaseType.easeOutSine
        );
        Hashtable _bounceToFloor = iTween.Hash
        (
            "x", Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[new_pos.x][new_pos.y].actual_pos.x,
            "y", Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[new_pos.x][new_pos.y].actual_pos.y,
            "Z", 0f,
            "time", time/8,
            "delay", time/4,
            "easetype", iTween.EaseType.easeInSine,
            "oncomplete", "LandAfterTween"
        );
        if (Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[new_pos.x][new_pos.y].resident == null)
        {
            if(hop)
            {
                iTween.MoveTo(gameObject, _bounceToPeak);
                iTween.MoveTo(gameObject, _bounceToFloor);
            }
            else
            {
                iTween.MoveTo(gameObject, _moveHash);
            }
            
            Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[new_pos.x][new_pos.y].resident = this;
            grid_pos = new_pos;
            return true;
        }
        return false;
    }

    public virtual bool SwapResidentsInstant(GridResident_Script first_res, GridResident_Script second_res)
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
    public virtual bool SwapResidentsTweened(GridResident_Script first_res, GridResident_Script second_res, float time)
    {
        if(Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[first_res.grid_pos.x][first_res.grid_pos.y].resident != null && Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[first_res.grid_pos.x][first_res.grid_pos.y].resident != null)
        {
            Vector2Int first_res_grid_pos = first_res.grid_pos;
            Vector2Int second_res_grid_pos = second_res.grid_pos;

            Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[first_res_grid_pos.x][first_res_grid_pos.y].resident = null;
            Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[second_res_grid_pos.x][second_res_grid_pos.y].resident = null;



            second_res.PlaceResidentTween(first_res_grid_pos,time,true);
            first_res.PlaceResidentTween(second_res_grid_pos,time, false);
            
            second_res.Local_Board_Changed();
            first_res.Local_Board_Changed();

            return true;
        }
        return false;
    }

    public virtual void PauseResident(bool paused)
    {

    }

    public virtual void Attacked(GridResident_Script attacker)
    {

    }

    public virtual void LandAfterTween()
    {
        
    }

    public virtual void Local_Board_Changed(){
        // Level_Events.Invoke_Board_Changed_Event();
    }

    public virtual void OnDestroy()
    {
        Level_Events.pause_toggle_event -= PauseResident;
    }
}
