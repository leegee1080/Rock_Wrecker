using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Rock_Script : GridResident_Script
{
    [field: SerializeField]public Rock_ScriptableObject primary_rock_type{get; private set;}

    [field: SerializeField]public Secondary_Rock_ScriptableObject secondary_rock_type{get; private set;}

    [SerializeField]private GameObject _artContainer_go;
    [SerializeField]private GameObject default_rock;
    [SerializeField]private MeshRenderer primary_renderer;
    [SerializeField]private MeshFilter primary_filter;
    [SerializeField]private MeshRenderer secondary_renderer;
    [SerializeField]private MeshFilter secondary_filter;
    [SerializeField]private bool initialized;
    [SerializeField]private ParticleSystem _secGlow_ps;
    [SerializeField]private ParticleSystem _glowParticle_ps;
    

    public void Check_Grid_Neighbor(Vector2Int starting_dir, Vector2Int checking_direction)
    {
        Func<Vector2Int, Grid_Data> find_griddata_func = Levelplay_Controller_Script.levelplay_controller_singleton.Find_Grid_Data;
        // Grid_Data[][]map_dict = Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array;
        Vector2Int direction_modifier = starting_dir;
        Vector2Int grid_pos_to_check = grid_pos + direction_modifier;
        List<Rock_Script> match_list = new List<Rock_Script>();
        //find the starting point on the starting dir
        Rock_Script farthest_left_rock = null;
        while(Primary_Rock_Type_Comparer(primary_rock_type.rock_type, grid_pos_to_check))
        {
            if(find_griddata_func(grid_pos_to_check).resident == null){return;}
            farthest_left_rock = (Rock_Script)find_griddata_func(grid_pos_to_check).resident;
            grid_pos_to_check += direction_modifier;
        }
        //now check back all the way in the checking direction
        if(farthest_left_rock == null){return;}
        grid_pos_to_check = farthest_left_rock.grid_pos;
        direction_modifier = checking_direction;
        while(Primary_Rock_Type_Comparer(primary_rock_type.rock_type, grid_pos_to_check))
        {
            if(find_griddata_func(grid_pos_to_check).resident == null){return;}
            match_list.Add((Rock_Script)find_griddata_func(grid_pos_to_check).resident);
            grid_pos_to_check += direction_modifier;
        }
        if(match_list.Count < Levelplay_Controller_Script.levelplay_controller_singleton.required_match_number){return;}
        //dump info into the queue for delete
        foreach (Rock_Script item in match_list)
        {
            if(!Levelplay_Controller_Script.levelplay_controller_singleton.rocks_queue_for_destruction.Contains(item)) 
            {
                Levelplay_Controller_Script.levelplay_controller_singleton.rocks_queue_for_destruction.Add(item);

            }
        }
    }

    private bool Primary_Rock_Type_Comparer(Rock_Types_Enum main_rock_type, Vector2Int grid_pos_of_rock_to_compare){
        if(Levelplay_Controller_Script.levelplay_controller_singleton.Find_Grid_Data(grid_pos_of_rock_to_compare) == null){return false;}

        Grid_Data griddata_to_compare = Levelplay_Controller_Script.levelplay_controller_singleton.Find_Grid_Data(grid_pos_of_rock_to_compare);

        if(griddata_to_compare.resident == null || !griddata_to_compare.resident.matchable){return false;}

        Rock_Script rock_to_test = (Rock_Script)griddata_to_compare.resident;
        if(rock_to_test.primary_rock_type.rock_type != this.primary_rock_type.rock_type){return false;}

        return true;
    }


    public override void Local_Board_Changed()
    {
        base.Local_Board_Changed();
        name = grid_pos.ToString();

        Check_Grid_Neighbor(Vector2Int.left, Vector2Int.right);
        Check_Grid_Neighbor(Vector2Int.down, Vector2Int.up);
        Check_Grid_Neighbor(Vector2Int.right, Vector2Int.left);
        Check_Grid_Neighbor(Vector2Int.up, Vector2Int.down);
    }

    public void Change_Rock_Types(Rock_ScriptableObject new_primary_rock_type, Secondary_Rock_ScriptableObject new_secondary_rock_type = null){
        if(initialized){Level_Events.Invoke_Board_Changed_Event();}
        primary_rock_type = new_primary_rock_type;
        secondary_rock_type = new_secondary_rock_type == null ? Levelplay_Controller_Script.levelplay_controller_singleton.default_secondary_rock_type: new_secondary_rock_type;
        Update_Primary_Rock_Type();
        Update_Secondary_Rock_Type();
        initialized = true;
    }

    public void Update_Primary_Rock_Type()
    {
        primary_renderer.material.color = primary_rock_type.main_color;
        primary_filter.mesh = primary_rock_type.main_mesh;
        default_rock.transform.Rotate(new Vector3(0,Global_Vars.rand_num_gen.Next(0,180),0));

        ParticleSystem.MainModule ps_main = _secGlow_ps.main;
        ps_main.startColor = new ParticleSystem.MinMaxGradient(secondary_rock_type.main_color_glow);
        ps_main = _glowParticle_ps.main;
        ps_main.startColor = new ParticleSystem.MinMaxGradient(secondary_rock_type.main_color_glow);
    }

    public void Update_Secondary_Rock_Type()
    {
        if(secondary_rock_type.secondary_type == Secondary_Rock_Types_Enum.none){secondary_renderer.gameObject.SetActive(false); return;}
        secondary_renderer.gameObject.SetActive(true);
        secondary_renderer.material.color = secondary_rock_type.main_color;
    }

    public override void LandAfterTween()
    {
        base.LandAfterTween();
        GameObject _dustPoof = Levelplay_Controller_Script.levelplay_controller_singleton.DustPoofPool.CallNext();
        _dustPoof.SetActive(true);
        _dustPoof.transform.position = gameObject.transform.position;
    }

    public void DeleteRock()
    {
        Levelplay_Controller_Script.levelplay_controller_singleton.Find_Grid_Data(grid_pos).resident = null;
        gameObject.SetActive(false);
    }

    public void Pop_Rock()
    {
        Levelplay_Controller_Script.levelplay_controller_singleton.Find_Grid_Data(grid_pos).resident = null;
        _artContainer_go.SetActive(false);
    }
}
