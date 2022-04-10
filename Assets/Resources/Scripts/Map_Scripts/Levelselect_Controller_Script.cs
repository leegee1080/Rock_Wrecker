using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levelselect_Controller_Script : MonoBehaviour
{
    public static Levelselect_Controller_Script levelselect_controller_singletion;

    private List<MapPOI_ScriptableObject> map_poi_list;
    [SerializeField] private MapPOI_Script map_poi_go;
    [SerializeField] public Animator _selectionHighlightAnimator;
    [SerializeField] private GameObject _shipGameObject;

    [Header("Selection Vars")]
    [SerializeField] private float selection_visual_offset;
    [SerializeField] private Vector3 selection_original_scale;
    [SerializeField] private float selection_visual_camera_zoom_ratio;
    [SerializeField]private MapPOI_Script selected_poi;
    [SerializeField]private GameObject selection_highlight_go;

    [Header("State Vars")]
    public MapUI_Script UIScript;
    private LevelselectStatesAbstractClass _currentStateClass;
    [SerializeField]private LevelselectStatesEnum _currentStateEnum;
    public Animator CinematicAnimator;


    // void OnGUI()
    // {
    //     if(GUI.Button(new Rect(100, 100, 50, 50),LevelselectStatesEnum.Deploy + ""))
    //     {
    //         ChangeState(LevelselectStatesEnum.Deploy);
    //     }
    //     if(GUI.Button(new Rect(100, 150, 50, 50),LevelselectStatesEnum.Select + ""))
    //     {
    //         ChangeState(LevelselectStatesEnum.Select);
    //     }
    //     if(GUI.Button(new Rect(150, 100, 50, 50),LevelselectStatesEnum.Setup + ""))
    //     {
    //         ChangeState(LevelselectStatesEnum.Setup);
    //     }
    //     if(GUI.Button(new Rect(150, 150, 50, 50),LevelselectStatesEnum.Ship + ""))
    //     {
    //         ChangeState(LevelselectStatesEnum.Ship);
    //     }
    // }

    void Awake() => levelselect_controller_singletion = this;

    private void Start()
    {
        map_poi_list = Overallgame_Controller_Script.overallgame_controller_singleton.main_map;

        if(map_poi_list == null){return;}
        foreach(MapPOI_ScriptableObject map_poi_so in map_poi_list){
            MapPOI_Script new_poi = Instantiate(map_poi_go,this.transform);
            new_poi.Init(map_poi_so);
        }

        ChangeState(LevelselectStatesEnum.Setup);
    }

    private void LateUpdate()
    {
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

    public void ChangeState(LevelselectStatesEnum _newState)
    {
        if(_currentStateEnum == _newState){return;}
        if(_currentStateClass != null){_currentStateClass.OnExitState(this);}
        switch (_newState)
        {
            case LevelselectStatesEnum.Setup:
                _currentStateClass = new LevelselectState_Setup();
                break;
            case LevelselectStatesEnum.Select:
                _currentStateClass = new LevelselectState_Select();
                break;
            case LevelselectStatesEnum.Ship:
                _currentStateClass = new LevelselectState_Ship();
                break;
            case LevelselectStatesEnum.Deploy:
                _currentStateClass = new LevelselectState_Deploy();
                break;
            default:
                Debug.LogWarning("You need a enum for this class change (84): " + _newState);
                return;
        }
        _currentStateClass.OnEnterState(this);
        _currentStateEnum = _newState;
    }

    public void Launch_Selected_Level()
    {
        if(selected_poi == null){return;}
        Loading_Controller_Script.loading_controller_singleton.Load_Next_Scene(Scene_Enums.levelplay);
    }

    public void Communicate_Selected_POI(MapPOI_Script tapped_poi){
        selected_poi = tapped_poi;
        Overallgame_Controller_Script.overallgame_controller_singleton.selected_level = tapped_poi.poi_info_so;
        _selectionHighlightAnimator.Play("Base Layer.MapSelectionOpen", 0 ,0);
        _selectionHighlightAnimator.GetComponent<MapSelectCursor_Script>().UpdateInfo(tapped_poi.poi_info_so);
    }

    public void Center_Screen()
    {
        Camera.main.transform.position = new Vector3(0,0,Global_Vars.min_planet_coord);
    }

    public void SwitchToShip()
    {
        ChangeState(LevelselectStatesEnum.Ship);
    }

    public void SwitchToSelect()
    {
        ChangeState(LevelselectStatesEnum.Select);
    }

    public void Back_to_Menu()
    {
        Loading_Controller_Script.loading_controller_singleton.Load_Next_Scene(Scene_Enums.mainmenu);
    }

    public void MoveShipGameObject()
    {
        _shipGameObject.SetActive(true);
        _shipGameObject.transform.position = Camera.main.transform.position;
    }
    public void HideShipGameObject()
    {
        _shipGameObject.SetActive(false);
    }
    public void FinishToMapTrans()
    {
        UIScript.OpenSelectionMiniMenu();
        Playerinput_Controller_Script.playerinput_controller_singleton.camera_controls_allowed = true;
    }
    public void FinishToShipTrans()
    {
        UIScript.OpenShipMiniMenu();
    }
}

public enum LevelselectStatesEnum
{
    Null,
    Setup,
    Select,
    Ship,
    Deploy
}

public abstract class LevelselectStatesAbstractClass
{
    public abstract void OnEnterState(Levelselect_Controller_Script _cont);
    public abstract void OnExitState(Levelselect_Controller_Script _cont);
    public abstract void OnUpdateState(Levelselect_Controller_Script _cont);
}

public class LevelselectState_Setup: LevelselectStatesAbstractClass
{
    public override void OnEnterState(Levelselect_Controller_Script _cont)
    {
        Debug.Log("setup");
        Playerinput_Controller_Script.playerinput_controller_singleton.camera_controls_allowed = false;
        Playerinput_Controller_Script.playerinput_controller_singleton.camera_follow_allowed = false;
        _cont.CinematicAnimator.SetBool("InShip", false);

    }   
    public override void OnExitState(Levelselect_Controller_Script _cont)
    {
        
    }   
    public override void OnUpdateState(Levelselect_Controller_Script _cont)
    {

    }   
}
public class LevelselectState_Select: LevelselectStatesAbstractClass
{
    public override void OnEnterState(Levelselect_Controller_Script _cont)
    {
        Debug.Log("select");
        _cont.UIScript.CloseShipMiniMenu();
        _cont.CinematicAnimator.SetBool("InShip", false);
        
    }   
    public override void OnExitState(Levelselect_Controller_Script _cont)
    {
        
    }   
    public override void OnUpdateState(Levelselect_Controller_Script _cont)
    {

    }   
}
public class LevelselectState_Ship: LevelselectStatesAbstractClass
{
    public override void OnEnterState(Levelselect_Controller_Script _cont)
    {
        Debug.Log("ship");
        Playerinput_Controller_Script.playerinput_controller_singleton.camera_controls_allowed = false;
        _cont.MoveShipGameObject();
        _cont._selectionHighlightAnimator.Play("Base Layer.MapSelectionClose", 0 ,0);
        _cont.CinematicAnimator.SetBool("InShip", true);
        _cont.UIScript.CloseSelectionMiniMenu();
    }   
    public override void OnExitState(Levelselect_Controller_Script _cont)
    {
        
    }   
    public override void OnUpdateState(Levelselect_Controller_Script _cont)
    {

    }   
}
public class LevelselectState_Deploy: LevelselectStatesAbstractClass
{
    public override void OnEnterState(Levelselect_Controller_Script _cont)
    {
        Debug.Log("deploy");
        Playerinput_Controller_Script.playerinput_controller_singleton.camera_controls_allowed = false;
    }   
    public override void OnExitState(Levelselect_Controller_Script _cont)
    {
        
    }   
    public override void OnUpdateState(Levelselect_Controller_Script _cont)
    {

    }   
}