using System.Collections;
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

    public GridResident_Script resident{get; set;}
    public Vector3 actual_pos{get; private set;}

    public float noise_data;

    public Grid_Data(GridResident_Script new_resident, Vector3 new_actual_pos, float new_noise){
        resident = new_resident;
        actual_pos = new_actual_pos;
        noise_data = new_noise;
    }
    public override string ToString(){
        return (" actual_pos: " + actual_pos + " resident: " + resident);
    }
}

public class Levelplay_Controller_Script : MonoBehaviour
{
    public static Levelplay_Controller_Script levelplay_controller_singleton;

    [Header("Canvas Elements")]
    [SerializeField]private GameObject ingame_menu_container;

    [SerializeField]private GameObject controls_container_go;
    [SerializeField] private float controls_visual_offset;
    [SerializeField] private Vector3 controls_original_scale;
    [SerializeField] private float controls_visual_camera_zoom_ratio;


    [Header("Level Gen Vars")]
    public int level_gen_seed;
    [SerializeField]private int map_x_size;
    [SerializeField]private int map_y_size;
    [SerializeField]private int map_unit_spacing;
    public Dictionary<Vector2, Grid_Data> map_coord_dict {get; private set;} = new Dictionary<Vector2, Grid_Data>();
    public GameObject TestRock;
    public GameObject TestWall;
    public GameObject TestObjectContainer;
    [field: SerializeField]public Rock_ScriptableObject default_primary_rock_type{get; private set;}
    [field: SerializeField]public Secondary_Rock_ScriptableObject default_secondary_rock_type{get; private set;}
    public int void_percentage;
    public int wall_percentage;
    public int rock_percentage;
    private float max_map_noise;
    private float avg_map_noise;
    private float min_map_noise;


    [Header("Gameplay Vars")]
    [SerializeField]private LevelPlayer_Script current_player_serialized;
    public LevelPlayer_Script current_player {get; private set;}

    [Header("Matching Vars")]
    public List<Rock_Script> rocks_queue_for_destruction;
    [field:SerializeField] public int required_match_number{get; private set;}
    // [SerializeField] private List<Match_Direction_Enum> match_direction_list = new List<Match_Direction_Enum>();


    void Awake() => levelplay_controller_singleton = this;

    private void Start() {
        // Level_Events.board_changed_event += Check_For_Pattern;

        current_player = current_player_serialized;

        Playerinput_Controller_Script.playerinput_controller_singleton.camera_controls_allowed = true;

        map_x_size = Global_Vars.level_starting_x_size * Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.poi_size;
        map_y_size = Global_Vars.level_starting_y_size * Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.poi_size;

        Gen_Map_Coords(level_gen_seed);
        Gen_Map_Residents();
        Deliver_Rock_Types();
        Pregame_Board_Check_Y();
        Pregame_Board_Check_X();
    }

    private void LateUpdate() {
        if(current_player == null){return;}
        controls_container_go.transform.position = Camera.main.WorldToScreenPoint(current_player.transform.position + Vector3.right * controls_visual_offset);
        Vector3 new_container_go_scale = new Vector3 (-Camera.main.gameObject.transform.position.z,-Camera.main.gameObject.transform.position.z,-Camera.main.gameObject.transform.position.z);
        new_container_go_scale = new Vector3(
            Mathf.Clamp(controls_original_scale.x - (new_container_go_scale.x/(controls_visual_camera_zoom_ratio*100)),0.1f,controls_original_scale.x*10),
            Mathf.Clamp(controls_original_scale.y - (new_container_go_scale.y/(controls_visual_camera_zoom_ratio*100)),0.1f,controls_original_scale.y*10),
            Mathf.Clamp(controls_original_scale.z - (new_container_go_scale.z/(controls_visual_camera_zoom_ratio*100)),0.1f,controls_original_scale.z*10)
        );
        controls_container_go.transform.localScale = new_container_go_scale;
    }

    private void Update() {
        if(rocks_queue_for_destruction.Count > 0){
            Clean_Rock_Queue();
        }
    }

    private void Clean_Rock_Queue(){
        // if(rocks_queue_for_destruction == null || rocks_queue_for_destruction.Count < 1){return;}
        // foreach (Rock_Script item in rocks_queue_for_destruction)
        // {
        //     item.Pop_Rock();
        // }
        // rocks_queue_for_destruction.Clear();
        StartCoroutine(Timed_Rock_Pop(rocks_queue_for_destruction));
    }

    IEnumerator Timed_Rock_Pop(List<Rock_Script> new_rock_queue){
        List<Rock_Script> instanced_rock_queue = new List<Rock_Script>();
        foreach (Rock_Script item in new_rock_queue)
        {
            instanced_rock_queue.Add(item);
        }
        rocks_queue_for_destruction.Clear();
        foreach (Rock_Script item in instanced_rock_queue)
        {
            item.Pop_Rock();

            yield return new WaitForSeconds(.05f);
        }
    }

    private void Gen_Map_Coords(int seed){
        System.Random rand = new System.Random(seed);
        float x_offset = rand.Next(5 * Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.poi_size, 15* Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.poi_size);
        float y_offset = rand.Next(5 * Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.poi_size, 15* Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.poi_size);

        List<float> noise_list = new List<float>();
        for (int x = -map_x_size/2; x <= map_x_size/2; x++)
        {   
            for (int y = -map_y_size/2; y <= map_y_size/2; y++)
            {
                Vector2 new_coord = new Vector2(x,y);
                map_coord_dict[new_coord] = new Grid_Data(null, new Vector3(x * map_unit_spacing, y * map_unit_spacing, 0), Mathf.PerlinNoise((x-map_x_size)/x_offset,(y-map_y_size)/y_offset) * 100f); 

                noise_list.Add(map_coord_dict[new_coord].noise_data);
            }
        }
        avg_map_noise = noise_list.Average();
        max_map_noise = noise_list.Max();
        min_map_noise = noise_list.Min();
        // Debug.Log(max_map_noise);
        // Debug.Log(avg_map_noise);
        // Debug.Log(min_map_noise);
        // Debug.Log(Mathf.Lerp(max_map_noise,min_map_noise,void_percentage/100f));
        // Debug.Log(Mathf.Lerp(max_map_noise,min_map_noise,wall_percentage/100f));
        //Global_Vars.Print_Map_Dict<Vector2, Grid_Data>(map_coord_dict);
    }

    private void Gen_Map_Residents(){
        foreach (KeyValuePair<Vector2, Grid_Data> coord in map_coord_dict)
        {
            if(coord.Key.x == 0 && coord.Key.y == 0)
            {
                current_player.Place_Resident(coord.Key);
                continue;
            }
            if(coord.Value.noise_data < Mathf.Lerp(max_map_noise,min_map_noise,void_percentage/100f)){
                GameObject new_go = coord.Value.noise_data > Mathf.Lerp(max_map_noise,min_map_noise,wall_percentage/100f) ||  coord.Key.x == map_x_size/2 || coord.Key.x == -map_x_size/2 || coord.Key.y == map_y_size/2 || coord.Key.y == -map_y_size/2
                ? Instantiate(TestWall, parent: TestObjectContainer.transform) : Instantiate(TestRock, parent: TestObjectContainer.transform);
                GridResident_Script new_gr = new_go.GetComponent<GridResident_Script>();
                new_gr.Place_Resident(coord.Key);
            }
        }
    }

    private void Deliver_Rock_Types(){
        foreach (KeyValuePair<Vector2, Grid_Data> coord in map_coord_dict)
        {
            if(coord.Value.resident != null && coord.Value.resident.matchable){
                Rock_Script rock = (Rock_Script)coord.Value.resident;
                rock.Change_Rock_Types(Rock_Types_Storage_Script.rock_types_controller_singleton.rock_so_list[Global_Vars.rand_num_gen.Next(0,Rock_Types_Storage_Script.rock_types_controller_singleton.rock_so_list.Count)]);
            }
        }
    }

    public void Pregame_Board_Check_X(){
            List<Vector2> direction_list = new List<Vector2>{new Vector2(1,0),new Vector2(0,1)}; 

            Rock_Script base_res = null;
            Rock_Script next_res = null;

            List<Rock_Script> small_compare_list = new List<Rock_Script>();
            List<Rock_Script> output_list = new List<Rock_Script>();

            int current_x = -(map_x_size/2);
            int current_y = -(map_y_size/2);

            Vector2 grid_pos_to_check = new Vector2(-(map_x_size/2), -(map_y_size/2));

            while(map_coord_dict.ContainsKey(grid_pos_to_check))
            {
                while(map_coord_dict.ContainsKey(grid_pos_to_check))
                {
                    if(
                        map_coord_dict[grid_pos_to_check].resident == null ||
                        !map_coord_dict[grid_pos_to_check].resident.matchable)
                        {small_compare_list.Clear();}
                    else{
                        next_res = (Rock_Script)map_coord_dict[grid_pos_to_check].resident;
                        if(small_compare_list.Count == 0){
                            small_compare_list.Add(next_res);
                            base_res = next_res;
                        }
                        else{
                            if(next_res.primary_rock_type.rock_type == base_res.primary_rock_type.rock_type){
                                // print("match found");
                                small_compare_list.Add(next_res);
                                if(small_compare_list.Count >= required_match_number){
                                    for (int i = 0; i < small_compare_list.Count; i++)
                                    {
                                        output_list.Add(small_compare_list[i]);
                                    }
                                    small_compare_list.Clear(); 
                                }   
                            }else{
                                small_compare_list.Clear();
                            }

                            base_res = (Rock_Script)map_coord_dict[grid_pos_to_check].resident;
                        }
                    }
                    current_x += 1;
                    grid_pos_to_check = new Vector2(current_x, current_y);
                }
                current_x = -(map_x_size/2);
                current_y += 1;
                grid_pos_to_check = new Vector2(current_x, current_y);
            }
            foreach (Rock_Script item in output_list)
            {
                rocks_queue_for_destruction.Add(item);
            }
        }

    public void Pregame_Board_Check_Y(){
        List<Vector2> direction_list = new List<Vector2>{new Vector2(1,0),new Vector2(0,1)}; 

        Rock_Script base_res = null;
        Rock_Script next_res = null;

        List<Rock_Script> small_compare_list = new List<Rock_Script>();
        List<Rock_Script> output_list = new List<Rock_Script>();

        int current_x = -(map_x_size/2);
        int current_y = -(map_y_size/2);

        Vector2 grid_pos_to_check = new Vector2(-(map_x_size/2), -(map_y_size/2));

        while(map_coord_dict.ContainsKey(grid_pos_to_check))
        {
            while(map_coord_dict.ContainsKey(grid_pos_to_check))
            {
                if(
                    map_coord_dict[grid_pos_to_check].resident == null ||
                    !map_coord_dict[grid_pos_to_check].resident.matchable)
                    {small_compare_list.Clear();}
                else{
                    next_res = (Rock_Script)map_coord_dict[grid_pos_to_check].resident;
                    if(small_compare_list.Count == 0){
                        small_compare_list.Add(next_res);
                        base_res = next_res;
                    }
                    else{
                        if(next_res.primary_rock_type.rock_type == base_res.primary_rock_type.rock_type){
                            small_compare_list.Add(next_res);
                            if(small_compare_list.Count >= required_match_number){
                                for (int i = 0; i < small_compare_list.Count; i++)
                                {
                                    output_list.Add(small_compare_list[i]);
                                }
                                small_compare_list.Clear(); 
                            }   
                        }else{
                            small_compare_list.Clear();
                        }

                        base_res = (Rock_Script)map_coord_dict[grid_pos_to_check].resident;
                    }
                }
                current_y += 1;
                grid_pos_to_check = new Vector2(current_x, current_y);
            }
            current_y = -(map_y_size/2);
            current_x += 1;
            grid_pos_to_check = new Vector2(current_x, current_y);
        }
        foreach (Rock_Script item in output_list)
        {
            rocks_queue_for_destruction.Add(item);
        }
    }

    public void Show_Ingame_Menu(){

    }

    // void OnDestroy() {
    //     Level_Events.board_changed_event -= Check_For_Pattern;
    // }
}
