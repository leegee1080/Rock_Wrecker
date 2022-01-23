using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Data{

    public GridResident_Script resident{get; set;}
    public Vector3 actual_pos{get; private set;}

    public Grid_Data(GridResident_Script new_resident, Vector3 new_actual_pos){
        resident = new_resident;
        actual_pos = new_actual_pos;
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
    [SerializeField]private int map_x_size;
    [SerializeField]private int map_y_size;
    [SerializeField]private int map_unit_spacing;
    public Dictionary<Vector2, Grid_Data> map_coord_dict {get; private set;} = new Dictionary<Vector2, Grid_Data>();
    public GameObject TestRock;
    public GameObject TestWall;
    public GameObject TestObjectContainer;


    [Header("Gameplay Vars")]
    [SerializeField]private LevelPlayer_Script current_player_serialized;
    public LevelPlayer_Script current_player {get; private set;}


    void Awake() => levelplay_controller_singleton = this;

    private void Start() {
        current_player = current_player_serialized;

        Playerinput_Controller_Script.playerinput_controller_singleton.camera_controls_allowed = true;

        map_x_size = Global_Vars.level_starting_x_size * Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.poi_size;
        map_y_size = Global_Vars.level_starting_y_size * Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.poi_size;

        Gen_Map_Coords();
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

    private void Gen_Map_Coords(){
        for (int x = -map_x_size/2; x <= map_x_size/2; x++)
        {   
            for (int y = -map_y_size/2; y <= map_y_size/2; y++)
            {
                Vector2 new_coord = new Vector2(x,y);
                map_coord_dict[new_coord] = new Grid_Data(null, new Vector3(x * map_unit_spacing, y * map_unit_spacing, 0)); 
                if(x == 0 && y == 0)
                {
                    current_player.Place_Resident(new_coord);
                    continue;
                }
                GameObject new_go = Mathf.Abs(x) == (map_x_size/2) || Mathf.Abs(y) == (map_y_size/2) ? Instantiate(TestWall, parent: TestObjectContainer.transform) : Instantiate(TestRock, parent: TestObjectContainer.transform);
                GridResident_Script new_gr = new_go.GetComponent<GridResident_Script>();
                new_gr.Place_Resident(new_coord);
                // new_go.transform.position = map_coord_dict[new_coord].actual_pos;
            }
        }
        
        //Global_Vars.Print_Map_Dict<Vector2, Grid_Data>(map_coord_dict);
    }

    public void Show_Ingame_Menu(){
        
    }

}
