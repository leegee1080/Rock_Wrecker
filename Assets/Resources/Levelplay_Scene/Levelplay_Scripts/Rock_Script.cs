using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock_Script : GridResident_Script
{
    [field: SerializeField]public Rock_ScriptableObject primary_rock_type{get; private set;}

    [field: SerializeField]public Secondary_Rock_ScriptableObject secondary_rock_type{get; private set;}


    [SerializeField]private MeshRenderer primary_renderer;
    [SerializeField]private MeshRenderer secondary_renderer;
    [SerializeField]private bool initialized;

    public void Check_Grid_Neighbor(Vector2 starting_dir, Vector2 checking_direction){
        Dictionary<Vector2, Grid_Data> map_dict = Levelplay_Controller_Script.levelplay_controller_singleton.map_coord_dict;
        Vector2 direction_modifier = starting_dir;
        Vector2 grid_pos_to_check = grid_pos + direction_modifier;
        List<Rock_Script> match_list = new List<Rock_Script>();
        //find the starting point on the starting dir
        Rock_Script farthest_left_rock = null;
        while(Primary_Rock_Type_Comparer(primary_rock_type.rock_type, grid_pos_to_check)){
            farthest_left_rock = (Rock_Script)map_dict[grid_pos_to_check].resident;
            grid_pos_to_check += direction_modifier;
        }
        if(farthest_left_rock == null){return;}
        //now check back all the way in the checking direction
        grid_pos_to_check = farthest_left_rock.grid_pos;
        direction_modifier = checking_direction;
        while(Primary_Rock_Type_Comparer(primary_rock_type.rock_type, grid_pos_to_check)){
            match_list.Add((Rock_Script)map_dict[grid_pos_to_check].resident);
            grid_pos_to_check += direction_modifier;
        }
        if(match_list.Count < Levelplay_Controller_Script.levelplay_controller_singleton.required_match_number){return;}
        //dump info into the queuefor delete
        foreach (Rock_Script item in match_list)
        {
            if(!Levelplay_Controller_Script.levelplay_controller_singleton.rocks_queue_for_destruction.Contains(item)) 
            {
                Levelplay_Controller_Script.levelplay_controller_singleton.rocks_queue_for_destruction.Add(item);

            }
        }
    }

    private bool Primary_Rock_Type_Comparer(Rock_Types_Enum main_rock_type, Vector2 grid_pos_of_rock_to_compare){
        if(   
            !Levelplay_Controller_Script.levelplay_controller_singleton.map_coord_dict.ContainsKey(grid_pos_of_rock_to_compare) ||
            Levelplay_Controller_Script.levelplay_controller_singleton.map_coord_dict[grid_pos_of_rock_to_compare].resident == null ||
            !Levelplay_Controller_Script.levelplay_controller_singleton.map_coord_dict[grid_pos_of_rock_to_compare].resident.matchable)
            {return false;}

        Rock_Script rock_to_test = (Rock_Script)Levelplay_Controller_Script.levelplay_controller_singleton.map_coord_dict[grid_pos_of_rock_to_compare].resident;
        if(rock_to_test.primary_rock_type.rock_type != this.primary_rock_type.rock_type){return false;}

        return true;
    }


    public override void Local_Board_Changed()
    {
        base.Local_Board_Changed();
        name = grid_pos.ToString();

        Check_Grid_Neighbor(Vector2.left, Vector2.right);
        Check_Grid_Neighbor(Vector2.down, Vector2.up);
        Check_Grid_Neighbor(Vector2.right, Vector2.left);
        Check_Grid_Neighbor(Vector2.up, Vector2.down);
    }

    public void Change_Rock_Types(Rock_ScriptableObject new_primary_rock_type, Secondary_Rock_ScriptableObject new_secondary_rock_type = null){
        if(initialized){Level_Events.Invoke_Board_Changed_Event();}
        primary_rock_type = new_primary_rock_type;
        secondary_rock_type = new_secondary_rock_type == null ? Levelplay_Controller_Script.levelplay_controller_singleton.default_secondary_rock_type: new_secondary_rock_type;
        Update_Primary_Rock_Type();
        Update_Secondary_Rock_Type();
        initialized = true;
    }

    public void Update_Primary_Rock_Type(){
        primary_renderer.material.color = primary_rock_type.main_color;
    }

    public void Update_Secondary_Rock_Type(){

    }

    public void Pop_Rock(){
        Levelplay_Controller_Script.levelplay_controller_singleton.map_coord_dict[grid_pos].resident = null;
        gameObject.SetActive(false);
    }
}
