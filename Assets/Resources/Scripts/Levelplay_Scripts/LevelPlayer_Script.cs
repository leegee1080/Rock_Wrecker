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

public class LevelPlayer_Script : LevelActor_Script
{
    
    private PlayerInputActions.PlayerControlsActions player_actions;
    private float move_timer = 0;

    [SerializeField] ParticleSystem _pickUpRock_ps;


    public override void Start()
    {
        base.Start();

        player_actions = Playerinput_Controller_Script.playerinput_controller_singleton.player_input_actions.PlayerControls;
        // Input_Control_Events.move_up_event += Moveup_Player;
        // Input_Control_Events.move_down_event += Movedown_Player;
        // Input_Control_Events.move_right_event += Moveright_Player;
        // Input_Control_Events.move_left_event += Moveleft_Player;
    }

    private void Update()
    {
        if(current_state == Level_Actor_States_Enum.Pause){return;}
        if(current_state == Level_Actor_States_Enum.Moving && move_timer <=0)
        {
            Change_Level_Actor_State(Level_Actor_States_Enum.Normal);
        }
        if(current_state == Level_Actor_States_Enum.Normal)
        {
            if(player_actions.MoveUp.IsPressed())
            {
                Move((int)Player_Direction_Enum.up);
            }
            else if(player_actions.MoveRight.IsPressed())
            {
                Move((int)Player_Direction_Enum.right);
            }
            else if(player_actions.MoveDown.IsPressed())
            {
                Move((int)Player_Direction_Enum.down);
            }
            else if(player_actions.MoveLeft.IsPressed())
            {
                Move((int)Player_Direction_Enum.left);
            }
            
        }
        if(move_timer >0){ move_timer -= Time.deltaTime;}
    }

    public void Move(int Direction)
    {

        Change_Level_Actor_State(Level_Actor_States_Enum.Moving);
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
            move_timer = 0.25f;
            Check_For_Exit_Tile(desired_coord);
            _pickUpRock_ps.Play();
            SwapResidentsTweened(this, Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[desired_coord.x][desired_coord.y].resident, 1f);
            return;
            // Swap_With_Rock(desired_coord);
        }

        if(desired_coord.x > 0 && desired_coord.y > 0 && desired_coord.x < Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array.Length && desired_coord.y < Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[desired_coord.x].Length && Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[desired_coord.x][desired_coord.y].resident == null)
        {
            move_timer = 0.10f; 
            Check_For_Exit_Tile(desired_coord);
            Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[grid_pos.x][grid_pos.y].resident = null;
            PlaceResidentTween(desired_coord,move_timer,false);       
            return;
            // Swap_With_Rock(desired_coord);
        }
        
    }

    public override void Local_Board_Changed()
    {
        return; // no need to check player for matches
    }

    private void Check_For_Exit_Tile(Vector2Int desired_coord)
    {
        if(grid_pos == Levelplay_Controller_Script.levelplay_controller_singleton.player_start_gridpos){Levelplay_Controller_Script.levelplay_controller_singleton.Player_Left_Exit();}
        if(desired_coord == Levelplay_Controller_Script.levelplay_controller_singleton.player_start_gridpos){Levelplay_Controller_Script.levelplay_controller_singleton.Player_Enter_Exit();}
    }
}
