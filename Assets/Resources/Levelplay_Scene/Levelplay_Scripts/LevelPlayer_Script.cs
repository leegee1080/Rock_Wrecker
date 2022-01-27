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

    // public bool Swap_With_Rock(Vector2 rock_grid_pos_to_swap){
    //     if(current_player_state == Level_Actor_States_Enum.Normal){
    //         Vector3 previous_pos = transform.position;
    //         gameObject.transform.position = Levelplay_Controller_Script.levelplay_controller_singleton.map_coord_dict[rock_grid_pos_to_swap].actual_pos;
    //         Levelplay_Controller_Script.levelplay_controller_singleton.map_coord_dict[rock_grid_pos_to_swap].resident.transform.position = previous_pos;
    //         Levelplay_Controller_Script.levelplay_controller_singleton.map_coord_dict[grid_pos].resident = Levelplay_Controller_Script.levelplay_controller_singleton.map_coord_dict[rock_grid_pos_to_swap].resident;
    //         Levelplay_Controller_Script.levelplay_controller_singleton.map_coord_dict[rock_grid_pos_to_swap].resident = this;
    //         grid_pos = rock_grid_pos_to_swap;
    //     }
    //     return false;
    // }
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
        Vector2 desired_coord = Vector2.zero;
        switch (Direction)
        {
            case (int)Player_Direction_Enum.up:
                desired_coord = grid_pos + new Vector2(0,1);
                break;
            case (int)Player_Direction_Enum.right:
                desired_coord = grid_pos + new Vector2(1,0);
                break;
            case (int)Player_Direction_Enum.down:
                desired_coord = grid_pos + new Vector2(0,-1);
                break;
            case (int)Player_Direction_Enum.left:
                desired_coord = grid_pos + new Vector2(-1,0);
                break;
            default:
                Debug.LogError("No direction int passed to player!");
                return;
        }
        if(Levelplay_Controller_Script.levelplay_controller_singleton.map_coord_dict[desired_coord].resident != null && Levelplay_Controller_Script.levelplay_controller_singleton.map_coord_dict[desired_coord].resident.moveable)
        {
            Swap_Residents(this, Levelplay_Controller_Script.levelplay_controller_singleton.map_coord_dict[desired_coord].resident);
            // Swap_With_Rock(desired_coord);
        }
    }

    public override bool Place_Resident(Vector2 new_pos){
        return base.Place_Resident(new_pos);
    }

    private void OnDestroy() {
        
    }
}
