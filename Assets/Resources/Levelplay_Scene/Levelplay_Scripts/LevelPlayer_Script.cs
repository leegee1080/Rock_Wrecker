using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Player_Direction_Enum
{
    up,
    right,
    down,
    left
}

public class LevelPlayer_Script : GridResident_Script
{
    public Level_Actor_States_Enum current_player_state;

    public override void Start() {
        base.Start();
        Input_Control_Events.move_up_event += Moveup_Player;
        Input_Control_Events.move_down_event += Movedown_Player;
        Input_Control_Events.move_right_event += Moveright_Player;
        Input_Control_Events.move_left_event += Moveleft_Player;
    }

    private void Moveup_Player(){
        Move((int)Player_Direction_Enum.up);
    }

    private void Movedown_Player(){
        Move((int)Player_Direction_Enum.down);
    }

    private void Moveright_Player(){
        Move((int)Player_Direction_Enum.right);
    }

    private void Moveleft_Player(){
        Move((int)Player_Direction_Enum.left);
    }

    public void Move(int Direction){
        Vector2Int desired_coord = Vector2Int.zero;
        switch (Direction)
        {
            case (int)Player_Direction_Enum.up:
                desired_coord = grid_pos + new Vector2Int(0,1);
                break;
            case (int)Player_Direction_Enum.right:
                desired_coord = grid_pos + new Vector2Int(1,0);
                break;
            case (int)Player_Direction_Enum.down:
                desired_coord = grid_pos + new Vector2Int(0,-1);
                break;
            case (int)Player_Direction_Enum.left:
                desired_coord = grid_pos + new Vector2Int(-1,0);
                break;
            default:
                Debug.LogError("No direction int passed to player!");
                return;
        }
        if(Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[desired_coord.x][desired_coord.y].resident != null && Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[desired_coord.x][desired_coord.y].resident.moveable)
        {
            Swap_Residents(this, Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[desired_coord.x][desired_coord.y].resident);
            // Swap_With_Rock(desired_coord);
        }

        if(desired_coord.x > 0 && desired_coord.y > 0 && desired_coord.x < Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array.Length && desired_coord.y < Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[desired_coord.x].Length && Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[desired_coord.x][desired_coord.y].resident == null)
        {
            Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[grid_pos.x][grid_pos.y].resident = null;
            Place_Resident(desired_coord);
            // Swap_With_Rock(desired_coord);
        }
    }

    public override bool Place_Resident(Vector2Int new_pos){
        return base.Place_Resident(new_pos);
    }

    private void OnDestroy() {
        Input_Control_Events.move_up_event -= Moveup_Player;
        Input_Control_Events.move_down_event -= Movedown_Player;
        Input_Control_Events.move_right_event -= Moveright_Player;
        Input_Control_Events.move_left_event -= Moveleft_Player;
    }
}
