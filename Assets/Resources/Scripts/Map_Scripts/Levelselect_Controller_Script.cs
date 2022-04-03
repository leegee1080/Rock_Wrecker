using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levelselect_Controller_Script : MonoBehaviour
{
    public static Levelselect_Controller_Script levelselect_controller_singletion;

    private List<MapPOI_ScriptableObject> map_poi_list;
    [SerializeField] private MapPOI_Script map_poi_go;
    [SerializeField] private Animator _selectionHighlightAnimator;

    [Header("Selection Vars")]
    [SerializeField] private float selection_visual_offset;
    [SerializeField] private Vector3 selection_original_scale;
    [SerializeField] private float selection_visual_camera_zoom_ratio;


    [SerializeField]private MapPOI_Script selected_poi;
    [SerializeField]private GameObject selection_highlight_go;


    void Awake() => levelselect_controller_singletion = this;

    private void Start(){
        Playerinput_Controller_Script.playerinput_controller_singleton.camera_controls_allowed =true;
        
        map_poi_list = Overallgame_Controller_Script.overallgame_controller_singleton.main_map;

        if(map_poi_list == null){return;}
        foreach(MapPOI_ScriptableObject map_poi_so in map_poi_list){
            MapPOI_Script new_poi = Instantiate(map_poi_go,this.transform);
            new_poi.Init(map_poi_so);
        }
    }

    private void LateUpdate() {
        if(selected_poi == null){return;}
        selection_highlight_go.transform.position = Camera.main.WorldToScreenPoint(selected_poi.transform.position + Vector3.right * selection_visual_offset);
        Vector3 new_sel_hi_go_scale = new Vector3 (-Camera.main.gameObject.transform.position.z,-Camera.main.gameObject.transform.position.z,-Camera.main.gameObject.transform.position.z);
        new_sel_hi_go_scale = new Vector3(
            Mathf.Clamp(selection_original_scale.x - (new_sel_hi_go_scale.x/(selection_visual_camera_zoom_ratio*100)),0.1f,selection_original_scale.x*10),
            Mathf.Clamp(selection_original_scale.y - (new_sel_hi_go_scale.y/(selection_visual_camera_zoom_ratio*100)),0.1f,selection_original_scale.y*10),
            Mathf.Clamp(selection_original_scale.z - (new_sel_hi_go_scale.z/(selection_visual_camera_zoom_ratio*100)),0.1f,selection_original_scale.z*10)
        );
        selection_highlight_go.transform.localScale = new_sel_hi_go_scale;
    }

    public void Launch_Selected_Level(){
        if(selected_poi == null){return;}
        Loading_Controller_Script.loading_controller_singleton.Load_Next_Scene(Scene_Enums.levelplay);
    }

    public void Communicate_Selected_POI(MapPOI_Script tapped_poi){
        selected_poi = tapped_poi;
        Overallgame_Controller_Script.overallgame_controller_singleton.selected_level = tapped_poi.poi_info_so;
        _selectionHighlightAnimator.SetTrigger("Open");
        _selectionHighlightAnimator.GetComponent<MapSelectCursor_Script>().UpdateInfo(tapped_poi.poi_info_so);
    }

    public void Center_Screen(){
        Camera.main.transform.position = new Vector3(0,0,Global_Vars.min_planet_coord);
    }

    public void Back_to_Menu(){
        Loading_Controller_Script.loading_controller_singleton.Load_Next_Scene(Scene_Enums.mainmenu);
    }
}
