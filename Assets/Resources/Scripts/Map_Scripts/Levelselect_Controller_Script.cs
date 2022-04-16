using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Levelselect_Controller_Script : MonoBehaviour
{
    public static Levelselect_Controller_Script levelselect_controller_singletion;

    private List<MapPOI_ScriptableObject> map_poi_list;
    [SerializeField] private MapPOI_Script map_poi_go;

    [SerializeField] public Animator _selectionHighlightAnimator;
    [SerializeField] private GameObject _shipGameObject;

    [Header("Selection Vars")]
    [SerializeField] public int DistPerFuelCost;
    public TMP_Text DroneCountText;
    [SerializeField] private float selection_visual_offset;
    [SerializeField] private Vector3 selection_original_scale;
    [SerializeField] private float selection_visual_camera_zoom_ratio;
    [SerializeField]private MapPOI_Script selected_poi;
    [SerializeField]public MapPOI_Script OccupiedPOI;
    [SerializeField]private GameObject selection_highlight_go;

    [Header("State Vars")]
    public MapUI_Script UIScript;
    private LevelselectStatesAbstractClass _currentStateClass;
    [SerializeField]private LevelselectStatesEnum _currentStateEnum;
    public Animator CinematicAnimator;

    [Header("Line Vars")]
    [SerializeField]Color _defaultColor;
    [SerializeField]Color _warnColor;
    [SerializeField]Color _alertColor;
    [SerializeField] private LineRenderer[] _fuelLineRend;

    [Header("Coroutines")]
    private IEnumerator _runningBlinkRountine;
    private IEnumerator _runningCloudPlacerRountine;
    private IEnumerator _runningDustPlacerRountine;

    [Header("GameJuice")]
    public GameObject DecoContainer;

    public float MapDecoDistFromCam;
    [SerializeField]private GameObject _spaceDustPrefab;
    [SerializeField]private GameObject _spaceCloudPrefab;
    public GameObjectPooler<PoolableGameObject> SpaceCloudsDecoPooler;
    public GameObjectPooler<PoolableGameObject> SpaceDustDecoPooler;



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
    //     if(GUI.Button(new Rect(150, 200, 50, 50),"Select Center POI"))
    //     {
    //         SelectCenterPOI();
    //     }
    // }

    void Awake() => levelselect_controller_singletion = this;

    private void Start()
    {
        SpaceCloudsDecoPooler = new GameObjectPooler<PoolableGameObject>(10,_spaceCloudPrefab,DecoContainer);
        SpaceDustDecoPooler = new GameObjectPooler<PoolableGameObject>(10,_spaceDustPrefab,DecoContainer);
        StartMapParticlePlacers();

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

        if(selection_highlight_go.GetComponent<MapSelectCursor_Script>().FuelCostTooHigh)
        {
            selection_highlight_go.GetComponent<MapSelectCursor_Script>().StartBlinkCoroutine();
            return;
        }

        if(Overallgame_Controller_Script.overallgame_controller_singleton.PlayerDrones <= 0)
        {
            BlinkGameObject(DroneCountText.gameObject);
            return;
        }

        Overallgame_Controller_Script.overallgame_controller_singleton.PlayerDrones -= 1;
        DroneCountText.text = Overallgame_Controller_Script.overallgame_controller_singleton.PlayerDrones + "";
        OccupiedPOI = selected_poi;
        ChangeState(LevelselectStatesEnum.Deploy);
        print("launched mission to: "+ selected_poi.name);
        // Loading_Controller_Script.loading_controller_singleton.Load_Next_Scene(Scene_Enums.levelplay);
    }

    public void Communicate_Selected_POI(MapPOI_Script tapped_poi){
        selected_poi = tapped_poi;
        Overallgame_Controller_Script.overallgame_controller_singleton.selected_level = tapped_poi.poi_info_so;
        _selectionHighlightAnimator.Play("Base Layer.MapSelectionOpen", 0 ,0);
        _selectionHighlightAnimator.GetComponent<MapSelectCursor_Script>().UpdateInfo(tapped_poi.poi_info_so);
    }

    public void SelectCenterPOI()
    {
        MapPOI_Script nearestCenterPOI = transform.GetChild(0).GetComponent<MapPOI_Script>();

        foreach (Transform childPOI in gameObject.transform)
        {
             if((Vector3.Distance(childPOI.transform.position, Vector3.zero) < (Vector3.Distance(nearestCenterPOI.transform.position, Vector3.zero))))
             {  
                 nearestCenterPOI = childPOI.GetComponent<MapPOI_Script>();
                 continue;
             }
        }
        OccupiedPOI = nearestCenterPOI;
        Communicate_Selected_POI(nearestCenterPOI);
        Camera.main.transform.position = new Vector3(OccupiedPOI.gameObject.transform.position.x,OccupiedPOI.gameObject.transform.position.y, Camera.main.transform.position.z);
        MoveShipGameObject();
    }

    public void Center_Screen()
    {
        Camera.main.transform.position = new Vector3(OccupiedPOI.gameObject.transform.position.x,OccupiedPOI.gameObject.transform.position.y, -50);
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
    public void UpdateFuelLines()
    {
        foreach (LineRenderer line in _fuelLineRend)
        {
            line.transform.position = OccupiedPOI.transform.position;
        }

        int lineIndex = 0;
        foreach (MapPOI_ScriptableObject childPOI in map_poi_list)
        {   
            if(lineIndex > _fuelLineRend.Length){lineIndex =0;}

            if(childPOI.name == OccupiedPOI.poi_info_so.name){continue;}

            

            int _dist = (int)((Vector2.Distance(OccupiedPOI.poi_info_so.map_pos, childPOI.map_pos)/DistPerFuelCost));
            if(_dist < Overallgame_Controller_Script.overallgame_controller_singleton.PlayerFuel)
            {  
                _fuelLineRend[lineIndex].gameObject.SetActive(true);
                _fuelLineRend[lineIndex].SetPosition(0, OccupiedPOI.poi_info_so.map_pos);
                // _fuelLineRend[lineIndex].SetPosition(1, new Vector3(childPOI.transform.position.x -5, childPOI.transform.position.y, childPOI.transform.position.z));
                _fuelLineRend[lineIndex].SetPosition(1, childPOI.map_pos);
                _fuelLineRend[lineIndex].startColor = _defaultColor;
                _fuelLineRend[lineIndex].endColor = _defaultColor;


                _fuelLineRend[lineIndex].gameObject.name = "Pointing to: "+ childPOI.name;

                lineIndex += 1;
                continue;
            }

            if(_dist == Overallgame_Controller_Script.overallgame_controller_singleton.PlayerFuel)
            {  
                _fuelLineRend[lineIndex].gameObject.SetActive(true);
                _fuelLineRend[lineIndex].SetPosition(0, OccupiedPOI.poi_info_so.map_pos);
                _fuelLineRend[lineIndex].SetPosition(1, childPOI.map_pos);
                _fuelLineRend[lineIndex].startColor = _warnColor;
                _fuelLineRend[lineIndex].endColor = _warnColor;

                _fuelLineRend[lineIndex].gameObject.name = "Pointing to: "+ childPOI.name;

                lineIndex += 1;
                continue;
            }

            if(_dist < 10)
            {  
                _fuelLineRend[lineIndex].gameObject.SetActive(true);
                _fuelLineRend[lineIndex].SetPosition(0, OccupiedPOI.poi_info_so.map_pos);
                _fuelLineRend[lineIndex].SetPosition(1, childPOI.map_pos);
                _fuelLineRend[lineIndex].startColor = _alertColor;
                _fuelLineRend[lineIndex].endColor = _alertColor;

                _fuelLineRend[lineIndex].gameObject.name = "Pointing to: "+ childPOI.name;

                lineIndex += 1;
                continue;
            }
        }
    }
    public void FinishToShipTrans()
    {
        UIScript.OpenShipMiniMenu();
    }

    public void StartMapParticlePlacers()
    {
        _runningCloudPlacerRountine = CloudPlacer();
        _runningDustPlacerRountine = DustPlacer();


        StartCoroutine(_runningCloudPlacerRountine);
        StartCoroutine(_runningDustPlacerRountine);
    }

    public void StopMapParticlesPlacers()
    {
        StopCoroutine(_runningCloudPlacerRountine);
        StopCoroutine(_runningDustPlacerRountine);
    }

    private IEnumerator DustPlacer()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(1);
            GameObject GO = SpaceDustDecoPooler.CallNext();
            GO.SetActive(true);
            GO.transform.position = new Vector3(Camera.main.gameObject.transform.position.x, Camera.main.gameObject.transform.position.y, MapDecoDistFromCam);
        }
    }
    private IEnumerator CloudPlacer()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(10);
            GameObject GO = SpaceCloudsDecoPooler.CallNext();
            GO.SetActive(true);
            GO.transform.position = new Vector3(Camera.main.gameObject.transform.position.x, Camera.main.gameObject.transform.position.y, MapDecoDistFromCam);
        }
    }

    public void BlinkGameObject(GameObject _obj)
    {
        if(_runningBlinkRountine != null){StopCoroutine(_runningBlinkRountine);}
        _runningBlinkRountine = BlinkUIGO(_obj);
        StartCoroutine(_runningBlinkRountine);
    }

    private IEnumerator BlinkUIGO(GameObject _obj)
    {
        for (int i = 0; i < 5; i++)
        {
            _obj.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.25f);
            _obj.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.25f);
        }
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

        _cont.DroneCountText.text = Overallgame_Controller_Script.overallgame_controller_singleton.PlayerDrones + "";
        Playerinput_Controller_Script.playerinput_controller_singleton.camera_controls_allowed = false;
        Playerinput_Controller_Script.playerinput_controller_singleton.camera_follow_allowed = false;
        _cont.CinematicAnimator.SetBool("InShip", false);

        _cont.SelectCenterPOI();
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

        GameObject GO = _cont.SpaceCloudsDecoPooler.CallNext();
        GO.SetActive(true);
        GO.transform.position = new Vector3(Camera.main.gameObject.transform.position.x, Camera.main.gameObject.transform.position.y, _cont.MapDecoDistFromCam);
        GO = _cont.SpaceCloudsDecoPooler.CallNext();
        GO.SetActive(true);
        GO.transform.position = new Vector3(Camera.main.gameObject.transform.position.x, Camera.main.gameObject.transform.position.y, _cont.MapDecoDistFromCam);
        _cont.StartMapParticlePlacers();

        _cont.DroneCountText.text = Overallgame_Controller_Script.overallgame_controller_singleton.PlayerDrones + "";
        _cont.UIScript.CloseShipMiniMenu();
        _cont.CinematicAnimator.SetBool("InShip", false);
        _cont.UpdateFuelLines();
    }   
    public override void OnExitState(Levelselect_Controller_Script _cont)
    {
        _cont.StopMapParticlesPlacers();
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