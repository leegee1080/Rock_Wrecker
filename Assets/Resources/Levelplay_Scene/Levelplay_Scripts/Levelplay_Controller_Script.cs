using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Rock_Types_Enum
{
    Copper,//green
    Iron,//red
    Gold,//yellow
    Silver,//white
    Cobalt,//blue
    Quartz//black
}

public enum Secondary_Rock_Types_Enum
{
    none,//black
    Diamond,//teal
    Ruby,//magenta
    Topaz//orange
}

public enum Match_Direction_Enum
{
    up,
    right,
    down,
    left
}

public class Level_Events
{
    #region board change event
    public static event System.Action board_changed_event;
    public static void Invoke_Board_Changed_Event()
    {
        board_changed_event?.Invoke();
    }
    #endregion
}

public class Grid_Data{

    public Vector2Int grid_pos{get; private set;}
    public GridResident_Script resident{get; set;}
    public Vector3 actual_pos{get; private set;}
    public float noise_data;
    public bool breadth_checked = false;

    public Grid_Data(Vector2Int new_grid_pos, GridResident_Script new_resident, Vector3 new_actual_pos, float new_noise)
    {
        grid_pos = new_grid_pos;
        resident = new_resident;
        actual_pos = new_actual_pos;
        noise_data = new_noise;
    }
    public override string ToString()
    {
        return (" actual_pos: " + actual_pos + " resident: " + resident + " noise_data: " + noise_data);
    }

    public Grid_Data[] Return_Neighbors(){

        Func<Vector2Int, Grid_Data> find_griddata_func = Levelplay_Controller_Script.levelplay_controller_singleton.Find_Grid_Data;
        Grid_Data[] neighbor_array = new Grid_Data[4];

        neighbor_array[0] = find_griddata_func(grid_pos + Vector2Int.up);
        neighbor_array[1] = find_griddata_func(grid_pos + Vector2Int.right);
        neighbor_array[2] = find_griddata_func(grid_pos + Vector2Int.down);
        neighbor_array[3] = find_griddata_func(grid_pos + Vector2Int.left);
        
        foreach (var item in neighbor_array)
        {
            if(item == null){continue;}
        }

        return neighbor_array;
    }
}

public class Levelplay_Controller_Script : MonoBehaviour
{
    public static Levelplay_Controller_Script levelplay_controller_singleton;

    [Header("Canvas Elements")]
    [SerializeField]private GameObject ingame_menu_container;
    [SerializeField]private GameObject levelplay_camera;
    [SerializeField]private GameObject score_menu_container;
    [SerializeField]private ScoreItem_Script[] ui_scoreitems = new ScoreItem_Script[4];


    [Header("Level Gen Vars")]
    [SerializeField]private int map_x_size;
    [SerializeField]private int map_y_size;
    [SerializeField]private int map_unit_spacing;
    [field: SerializeField]public Grid_Data[][] x_lead_map_coord_array{get; private set;}
    public GameObject TestRock;
    public GameObject TestWall;
    public GameObject TestObjectContainer;
    [field: SerializeField]public Rock_ScriptableObject default_primary_rock_type{get; private set;}
    [field: SerializeField]public Secondary_Rock_ScriptableObject default_secondary_rock_type{get; private set;}
    public int void_percentage;
    public int wall_percentage;
    public int rock_percentage;
    public int player_spawn_percentage;
    private float max_map_noise;
    private float avg_map_noise;
    private float min_map_noise;


    [Header("Gameplay Vars")]
    public int[] resources_collected_array = new int[4];
    [SerializeField]private LevelPlayer_Script current_player_serialized;
    public LevelPlayer_Script current_player {get; private set;}
    [SerializeField] private bool game_started;
    [SerializeField] private Vector2Int player_start_gridpos;


    [Header("Matching Vars")]
    public List<Rock_Script> rocks_queue_for_destruction;
    [field:SerializeField] public int required_match_number{get; private set;}


    void Awake() => levelplay_controller_singleton = this;

    private void Start()
    {
        current_player = null;
        current_player = Instantiate(current_player_serialized, parent: TestObjectContainer.transform);

        Playerinput_Controller_Script.playerinput_controller_singleton.camera_controls_allowed = true;

        map_x_size = Global_Vars.level_starting_x_size * Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.poi_size;
        map_y_size = Global_Vars.level_starting_y_size * Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.poi_size;

        Gen_Map_Coords(Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.level_seed);
        Find_Max_Play_Area(Find_Map_Breadth_Search_Seed());
        Find_Player_Spawn();
        Gen_Map_Residents();
        Deliver_Rock_Types();
        Pregame_Board_Check();
        Clean_Rock_Queue();
        game_started = true;

        Playerinput_Controller_Script.playerinput_controller_singleton.on_screen_controls_allowed = true;
    }

    // private void LateUpdate()
    // {
    //     if(current_player == null){return;}
    //     controls_container_go.transform.position = Camera.main.WorldToScreenPoint(current_player.transform.position + Vector3.right * controls_visual_offset);
    //     Vector3 new_container_go_scale = new Vector3 (-Camera.main.gameObject.transform.position.z,-Camera.main.gameObject.transform.position.z,-Camera.main.gameObject.transform.position.z);
    //     new_container_go_scale = new Vector3(
    //         Mathf.Clamp(controls_original_scale.x - (new_container_go_scale.x/(controls_visual_camera_zoom_ratio*100)),0.1f,controls_original_scale.x*10),
    //         Mathf.Clamp(controls_original_scale.y - (new_container_go_scale.y/(controls_visual_camera_zoom_ratio*100)),0.1f,controls_original_scale.y*10),
    //         Mathf.Clamp(controls_original_scale.z - (new_container_go_scale.z/(controls_visual_camera_zoom_ratio*100)),0.1f,controls_original_scale.z*10)
    //     );
    //     controls_container_go.transform.localScale = new_container_go_scale;
    // }

    private void Update()
    {
        if(rocks_queue_for_destruction.Count > 0){
            Clean_Rock_Queue();
        }
    }

    public Grid_Data Find_Grid_Data(Vector2Int grid_pos)
    {
        if(grid_pos.x < 0 || grid_pos.x >= x_lead_map_coord_array.Length || grid_pos.y < 0 || grid_pos.y >= x_lead_map_coord_array[grid_pos.x].Length)
        {
            return null;
        }
        return x_lead_map_coord_array[grid_pos.x][grid_pos.y];
    }

#region Scoring
    private void Clean_Rock_Queue()
    {
        List<Rock_Script> instanced_rock_queue = new List<Rock_Script>();
        foreach (Rock_Script item in rocks_queue_for_destruction)
        {
            instanced_rock_queue.Add(item);
        }
        rocks_queue_for_destruction.Clear();

        if(game_started)
        {
            StartCoroutine(Timed_Rock_Pop(instanced_rock_queue));
        }
        else
        {
            Pregame_Rock_Pop(instanced_rock_queue);
        }
    }

    private void Pregame_Rock_Pop(List<Rock_Script> new_rock_queue)
    {
        foreach (Rock_Script item in new_rock_queue)
        {
            item.Pop_Rock();
        }
    }

    IEnumerator Timed_Rock_Pop(List<Rock_Script> new_rock_queue)
    {
        foreach (Rock_Script item in new_rock_queue)
        {
            resources_collected_array[0] += 100;
            if(item.secondary_rock_type.secondary_type != Secondary_Rock_Types_Enum.none){resources_collected_array[(int)item.secondary_rock_type.secondary_type] += 1;}
            item.Pop_Rock();

            yield return new WaitForSeconds(.05f);
        }

        if(score_menu_container.activeSelf)
        {
            foreach (ScoreItem_Script item in ui_scoreitems)
            {
                item.Update_My_Score(resources_collected_array);
            }
        }
    }
#endregion

#region  MenuItems
    public void Show_Ingame_Menu()
    {
        Exit_Level_To_Map();
    }

    public void Show_Ingame_Inventory()
    {
        if(!score_menu_container.activeSelf)
        {
            score_menu_container.SetActive(true);
            foreach (ScoreItem_Script item in ui_scoreitems)
            {
                item.Update_My_Score(resources_collected_array);
            }
        }
        else{score_menu_container.SetActive(false);}
        
    }

    public void Exit_Level_To_Map()
    {
        Overallgame_Controller_Script.overallgame_controller_singleton.player_score += Level_Exit_Score_Calc();
        Loading_Controller_Script.loading_controller_singleton.Load_Next_Scene(Scene_Enums.levelselect);
    }
    public void Exit_Level_To_MainMenu()
    {
        Loading_Controller_Script.loading_controller_singleton.Load_Next_Scene(Scene_Enums.mainmenu);
    }

    private int Level_Exit_Score_Calc()
    {
        int temp_score = resources_collected_array[0];
        for (int i = 1; i < resources_collected_array.Length; i++)//skip the first index because that is the none secondary_rock type
        {
            int bonus = resources_collected_array[i] * Rock_Types_Storage_Script.rock_types_controller_singleton.secondary_rock_type_dict[(Secondary_Rock_Types_Enum)i].score_bonus;
            temp_score += bonus;
        }
        return temp_score;
    }
#endregion

#region MapSetup
    private void Gen_Map_Coords(int seed)
    {
        x_lead_map_coord_array = new Grid_Data[map_x_size][];

        System.Random rand = new System.Random(seed);
        float x_offset = rand.Next(5, 15);
        float y_offset = rand.Next(5, 15);

        List<float> noise_list = new List<float>();

        //create x lead array
        for (int x = 0; x < map_x_size; x++)
        {
            Grid_Data[] new_y_grid_data = new Grid_Data[map_y_size];
            for (int y = 0; y < map_y_size; y++)
            {
                new_y_grid_data[y] = new Grid_Data(new Vector2Int(x,y), null, new Vector3(x * map_unit_spacing, y * map_unit_spacing, 0), Mathf.PerlinNoise((x-map_x_size)/x_offset,(y-map_y_size)/y_offset) * 100f); 
                noise_list.Add(new_y_grid_data[y].noise_data);
            }
            x_lead_map_coord_array[x] = new_y_grid_data;
        }

        avg_map_noise = noise_list.Average();
        max_map_noise = noise_list.Max();
        min_map_noise = noise_list.Min();
        // Debug.Log(max_map_noise);
        // Debug.Log(avg_map_noise);
        // Debug.Log(min_map_noise);
        // Debug.Log(Mathf.Lerp(max_map_noise,min_map_noise,rock_percentage/100f));
        // Debug.Log(Mathf.Lerp(max_map_noise,min_map_noise,void_percentage/100f));
        // Debug.Log(Mathf.Lerp(max_map_noise,min_map_noise,wall_percentage/100f));
        // Global_Vars.Print_Map_Array(x_lead_map_coord_array);
    }
    private Vector2Int Find_Map_Breadth_Search_Seed()
    {
        IEnumerable<Vector2Int> possible_seed_start_locations = 
            from row in x_lead_map_coord_array
            from cell in row
            where cell.noise_data > min_map_noise && cell.noise_data < Mathf.Lerp(min_map_noise,max_map_noise,player_spawn_percentage/100f)
            select cell.grid_pos;
        return possible_seed_start_locations.ElementAt(0);
    }
    private void Find_Player_Spawn()
    {
        IEnumerable<Vector2Int> possible_player_start_locations = 
            from row in x_lead_map_coord_array
            from cell in row
            where cell.noise_data > min_map_noise && cell.noise_data < Mathf.Lerp(min_map_noise,max_map_noise,player_spawn_percentage/100f) && cell.breadth_checked && cell.grid_pos.x > 0 && cell.grid_pos.x < map_x_size-1 && cell.grid_pos.y > 0 && cell.grid_pos.y < map_y_size-1
            select cell.grid_pos;
        player_start_gridpos = possible_player_start_locations.ElementAt((int)Global_Vars.rand_num_gen.Next(0,possible_player_start_locations.Count()));
    }
    private List<Grid_Data> Find_Max_Play_Area(Vector2Int starting_grid_pos)
    {
        List<Grid_Data> playable_blob = new List<Grid_Data>();
        Grid_Data current_grid_data;

        //seeding the list
        current_grid_data = Find_Grid_Data(starting_grid_pos);
        current_grid_data.breadth_checked = true;
        playable_blob.Add(current_grid_data);
        foreach (Grid_Data neighbor in current_grid_data.Return_Neighbors())
        {
            if(neighbor != null && !playable_blob.Contains(neighbor)){ playable_blob.Add(neighbor);}
        }

        int breakout_timer = 0;//to make sure it never gets stuck

        while(Still_Grid_Data_To_Check(playable_blob) && breakout_timer < (map_x_size*map_y_size) ) //make sure there is still more to check
        {     
            List<Grid_Data> clone_playable_blob =new List<Grid_Data>(playable_blob);
            foreach (Grid_Data item in clone_playable_blob)                                         //loop through the list to check
            {
                if(item.breadth_checked == true){continue;} 
                foreach (Grid_Data neighbor in item.Return_Neighbors())
                {   
                    if(neighbor != null && neighbor.noise_data < Mathf.Lerp(min_map_noise,max_map_noise,wall_percentage/100f) && !playable_blob.Contains(neighbor))
                    { 
                        playable_blob.Add(neighbor);
                    }
                    
                }
                item.breadth_checked = true;
            }
            breakout_timer+=1;
        }
        return playable_blob;
    }
    private bool Still_Grid_Data_To_Check(List<Grid_Data> list_to_check)
    {
        IEnumerable<Grid_Data> unchecked_grid_data = 
            from item in list_to_check
            where item.breadth_checked == false
            select item;
        
        if(unchecked_grid_data.Count() > 0){return true;}
        return false;
    }
    private void Gen_Map_Residents()
    {
        GameObject new_go;
        IEnumerable<Grid_Data> matchable_residents = 
            from row in x_lead_map_coord_array
            from cell in row
            where cell.breadth_checked==true
            select cell;
        
        foreach (Grid_Data item in matchable_residents)
        {
            if(item.grid_pos == player_start_gridpos)
            {
                current_player.Place_Resident(player_start_gridpos);
                levelplay_camera.transform.position = new Vector3(current_player.transform.position.x,current_player.transform.position.y-10,levelplay_camera.transform.position.z);
                current_player.current_player_state = Level_Actor_States_Enum.Normal;
                continue;
            }
            if(item.grid_pos.x <= 0 || item.grid_pos.x >= map_x_size-1|| item.grid_pos.y <= 0 || item.grid_pos.y >= map_y_size-1) //make sure there is no edge matchables
            {
                new_go = Instantiate(TestWall, parent: TestObjectContainer.transform);
                new_go.GetComponent<GridResident_Script>().Place_Resident(item.grid_pos);
                continue;
            }
            new_go = Instantiate(TestRock, parent: TestObjectContainer.transform);
            new_go.GetComponent<GridResident_Script>().Place_Resident(item.grid_pos);
        }

        IEnumerable<Grid_Data> residents = 
            from row in x_lead_map_coord_array
            from cell in row
            where cell.resident != null
            select cell;

        foreach (Grid_Data item in residents)
        {
            foreach (Grid_Data neighbor in item.Return_Neighbors())
            {
                if(neighbor == null || !item.resident.matchable){continue;}
                if(neighbor.resident == null)
                {
                    new_go = Instantiate(TestWall, parent: TestObjectContainer.transform);
                    new_go.GetComponent<GridResident_Script>().Place_Resident(neighbor.grid_pos);
                }
            }
        }
    }
    private void Deliver_Rock_Types()
    {
        foreach (Grid_Data[] y in x_lead_map_coord_array)
        {
            foreach (Grid_Data item in y)
            {
                if(item.resident != null && item.resident.matchable)
                {
                    Rock_Script rock = (Rock_Script)item.resident;
                    rock.Change_Rock_Types(
                        Rock_Types_Storage_Script.rock_types_controller_singleton.rock_so_list[Global_Vars.rand_num_gen.Next(0,Rock_Types_Storage_Script.rock_types_controller_singleton.rock_so_list.Count)],
                        Determine_Secondary_Rock_Type()
                        );
                }
            }
        }
    }
    private Secondary_Rock_ScriptableObject Determine_Secondary_Rock_Type()
    {
        Secondary_Rock_ScriptableObject temp_sec_rock_type = null;
        int rock_chance = 100 - Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.poi_difficulty;

        if((int)Global_Vars.rand_num_gen.Next(0,rock_chance) <= 1)
        {
            temp_sec_rock_type = 
            Rock_Types_Storage_Script.rock_types_controller_singleton.secondary_so_list[Global_Vars.rand_num_gen.Next(0,Rock_Types_Storage_Script.rock_types_controller_singleton.secondary_so_list.Count)];
        }
        return temp_sec_rock_type;
    }
    public void Pregame_Board_Check()
    {
        Rock_Script matchable_rock_to_test = null;
        Rock_Script matchable = null;
        List<Rock_Script> list_of_matched = new List<Rock_Script>();

        for (int x = 0; x < map_x_size; x++)
        {
            for (int y = 0; y < map_y_size; y++)
            {
                Grid_Data item = Find_Grid_Data(new Vector2Int(x,y));
                if(item.resident != null && item.resident.matchable)
                {
                    matchable_rock_to_test = (Rock_Script)item.resident;
                    if(matchable != null && matchable.primary_rock_type == matchable_rock_to_test.primary_rock_type)
                    {
                        list_of_matched.Add(matchable_rock_to_test);
                        matchable = matchable_rock_to_test;
                        if(list_of_matched.Count() >= required_match_number)
                        {
                            foreach (Rock_Script rock in list_of_matched)
                            {
                                rocks_queue_for_destruction.Add(rock);
                            }
                            list_of_matched.Clear();
                            matchable = null;
                        }
                    }
                    else
                    {
                        list_of_matched.Clear();
                        matchable = matchable_rock_to_test;
                        list_of_matched.Add(matchable);
                    }
                }
            }
        }
        for (int y = 0; y < map_y_size; y++)
        {
            for (int x = 0; x < map_x_size; x++)
            {
                Grid_Data item = Find_Grid_Data(new Vector2Int(x,y));
                if(item.resident != null && item.resident.matchable)
                {
                    matchable_rock_to_test = (Rock_Script)item.resident;
                    if(matchable != null && matchable.primary_rock_type == matchable_rock_to_test.primary_rock_type)
                    {
                        list_of_matched.Add(matchable_rock_to_test);
                        matchable = matchable_rock_to_test;
                        if(list_of_matched.Count() >= required_match_number)
                        {
                            foreach (Rock_Script rock in list_of_matched)
                            {
                                rocks_queue_for_destruction.Add(rock);
                            }
                            list_of_matched.Clear();
                            matchable = null;
                        }
                    }
                    else
                    {
                        list_of_matched.Clear();
                        matchable = matchable_rock_to_test;
                        list_of_matched.Add(matchable);
                    }
                }
            }
        }

    }
    
#endregion

}
