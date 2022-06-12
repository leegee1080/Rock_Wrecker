using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

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

public enum Match_Direction_EnumS
{
    up,
    right,
    down,
    left
}

public enum LevelStatesEnum
{
    Setup,
    Playing,
    Paused,
    GetToEscape,
    CleanupWin,
    CleanupLose,
    PlayerOnExit
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
    #region game pause toggle event
    public static event System.Action<bool> pause_toggle_event;
    public static void Invoke_Pause_Toggle_Event(bool new_pause_state)
    {
        pause_toggle_event?.Invoke(new_pause_state);
    }
    #endregion
}

public class Grid_Data{

    public Vector2Int grid_pos{get; private set;}
    public GridResident_Script resident{get; set;}
    public bool playable;
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

[Serializable]public class Timer<T, P> // first is the function return and second is the parameter passed
{
    private Func<P, T> timer_finished_func;

    private P _timerFinishedParameter;

    public float timer_max_amount;
    public float timer_amount;
    public bool timer_finished_bool;
    public bool timer_paused_bool;
    public bool repeatable;


    public Timer(float new_timer_amount, Func<P, T> new_timer_finished_func, P _newFuncParameter)
    {
        timer_finished_bool= false;
        timer_paused_bool = false;
        repeatable = false;
        timer_max_amount = new_timer_amount;
        timer_finished_func = new_timer_finished_func;
        _timerFinishedParameter = _newFuncParameter;
        timer_amount = timer_max_amount;
    }

    public void Reset_Timer()
    {
        timer_amount = timer_max_amount;
        timer_finished_bool = false;
    }

    public void Pause_Timer(bool new_timer_state)
    {
        timer_paused_bool = new_timer_state;
    }

    public bool Decrement_Timer(float decrement_amount)
    {
        if(timer_paused_bool){return false;}
        if(timer_finished_bool){return timer_finished_bool;}

        timer_amount -= decrement_amount;
        if(timer_amount <= 0)
        {
            timer_finished_func(_timerFinishedParameter);
            if(!repeatable){timer_finished_bool = true;}else{timer_amount = timer_max_amount;}
        }
        
        return timer_finished_bool;
    }


}

public class Levelplay_Controller_Script : MonoBehaviour
{
    public static Levelplay_Controller_Script levelplay_controller_singleton;

    [Header("Canvas Elements")]
    [SerializeField]public GameObject ingame_menu_container;
    [SerializeField]private ParticleSystem CollectSparkle;
    [SerializeField]private GameObject score_menu_container;
    [SerializeField]private Animator UI_animator;
    [SerializeField]private ScoreItem_Script[] ui_scoreitems = new ScoreItem_Script[4];
    [SerializeField]public TMP_Text timer_text;
    public Timer<bool, LevelStatesEnum> timer_text_ref; 

    [Header("Music")]
    [SerializeField]private string[] _musicArray;
    [SerializeField]public string _selectedMusic;


    [Header("Game Objects")]
    public GameObject MainCanvas;
    public GameObject TestRock;
    public GameObject TestWall;
    public GameObject TestExit;
    public DropShip_Script drop_ship;
    public GameObject floor_go;
    public GameObject floor_container;
    public GameObject rock_go;
    public GameObject rock_container;
    public GameObject wall_go;
    public GameObject wall_container;
    public GameObject actor_container;
    public GameObject rockExplosion_GO;
    public GameObject dustPoof_GO;
    public GameObject dustMote_GO;
    public GameObject auraPiller_GO;
    public GameObject scoreParticles_GO;
    public BoxCollider2D scoreCollector_GO;
    public Light CameraSpotLight;


    [Header("Level Gen Vars")]
    [SerializeField]private int map_x_size;
    [SerializeField]private int map_y_size;
    [SerializeField]private int map_unit_spacing;
    [field: SerializeField]public Grid_Data[][] x_lead_map_coord_array{get; private set;}
    public List<Vector2Int> wall_coord_list = new List<Vector2Int>();
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
    public LevelStatesEnum CurrentLevelState;
    public LevelStatesEnum LastLevelState;
    private LevelplayStatesAbstractClass _currentStateClass;
    [SerializeField]private Vector3 camera_offset;
    [SerializeField]private float level_setup_time;
    public Timer<bool, LevelStatesEnum> level_setup_timer;
    [SerializeField]private float level_escape_time;
    public Timer<bool, LevelStatesEnum> level_escape_timer;
    [SerializeField]private float level_end_time;
    public Timer<bool, LevelStatesEnum> level_end_timer;
    [SerializeField]private float level_exit_time;
    public Timer<bool, LevelStatesEnum> level_exit_timer;
    private bool player_left_exit;
    public bool player_on_exit;
    public int[] resources_collected_array = new int[4];
    [SerializeField]private LevelPlayer_Script current_player_serialized;
    public LevelPlayer_Script current_player {get; private set;}
    [SerializeField] public Vector2Int player_start_gridpos;
    [SerializeField] private LevelEnemy_Storage _enemyStorageScript;
    [SerializeField]public float _enemySpawnTime;
    public Timer<bool, float> enemySpawn_Timer;
    [SerializeField]private int _scoreQueue;
    [SerializeField]private float _scoreQueueDecSpeed;
    private IEnumerator _scoreQueueEnumerator;
    private IEnumerator _motePlacerEnumerator;


    [Header("Object Pools")]
    [SerializeField] int _playerViewCullDist;
    [SerializeField]private GameObject _pooledParticlesParent;
    public GameObjectPooler<PoolableGameObject> DustMotePool;
    public GameObjectPooler<PoolableGameObject> DustPoofPool;
    public GameObjectPooler<PoolableGameObject> RockExplosionPool;
    public GameObjectPooler<PoolableGameObject> ScoreParticlesPool;


    [Header("Matching Vars")]
    public List<Rock_Script> rocks_queue_for_destruction;
    [field:SerializeField] public int required_match_number{get; private set;}


    void Awake() => levelplay_controller_singleton = this;

    private void Start()
    {
        _selectedMusic = _musicArray[Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.poi_difficulty];

        _currentStateClass = new LevelplayState_Setup();
        _currentStateClass.OnEnterState(this);

        drop_ship.Launch();

        current_player = null;
        current_player = Instantiate(current_player_serialized, parent: actor_container.transform);

        Playerinput_Controller_Script.playerinput_controller_singleton.camera_controls_allowed = true;

        map_x_size = Global_Vars.level_starting_x_size * Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.poi_size;
        map_y_size = Global_Vars.level_starting_y_size * Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.poi_size;

        //create object pools
        DustPoofPool = new GameObjectPooler<PoolableGameObject>(10,dustPoof_GO,_pooledParticlesParent);
        RockExplosionPool = new GameObjectPooler<PoolableGameObject>(10,rockExplosion_GO,_pooledParticlesParent);
        DustMotePool = new GameObjectPooler<PoolableGameObject>(20,dustMote_GO,_pooledParticlesParent);
        ScoreParticlesPool = new GameObjectPooler<PoolableGameObject>(20,scoreParticles_GO,_pooledParticlesParent);
        

        UnityEngine.Random.InitState(Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.level_seed);

        Gen_Map_Coords(Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.level_seed);
        Find_Max_Play_Area(Find_Map_Breadth_Search_Seed());
        Gen_Map_Residents();
        Deliver_Rock_Types();
        Deliver_Secondary_Rock_Types(Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.lode_dia, Secondary_Rock_Types_Enum.Diamond);
        Deliver_Secondary_Rock_Types(Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.lode_top, Secondary_Rock_Types_Enum.Topaz);
        Deliver_Secondary_Rock_Types(Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.lode_rub, Secondary_Rock_Types_Enum.Ruby);
        Pregame_Board_Check();
        Clean_Rock_Queue(true);

        Find_Player_Spawn();
        current_player.gameObject.SetActive(false);
        // floor_go.GetComponent<Floor_Script>().Build_Floor_Mesh();
        // floor_go.transform.position = new Vector3(Find_Grid_Data(player_start_gridpos).actual_pos.x,Find_Grid_Data(player_start_gridpos).actual_pos.y,Find_Grid_Data(player_start_gridpos).actual_pos.z  +1);

        Camera.main.transform.position = new Vector3(Find_Grid_Data(player_start_gridpos).actual_pos.x, Find_Grid_Data(player_start_gridpos).actual_pos.y -10, Find_Grid_Data(player_start_gridpos).actual_pos.z - 20); 
        Playerinput_Controller_Script.playerinput_controller_singleton.camera_controls_allowed = false;
        Playerinput_Controller_Script.playerinput_controller_singleton.auto_camera_move_speed = 0.006f;
        Playerinput_Controller_Script.playerinput_controller_singleton.desired_camera_pos_offset = camera_offset;
        Playerinput_Controller_Script.playerinput_controller_singleton.follow_target = current_player.gameObject;


        // if(Playerinput_Controller_Script.playerinput_controller_singleton.on_screen_controls_allowed == false) {Playerinput_Controller_Script.playerinput_controller_singleton.Toggle_On_Screen_Controls();}
        _enemySpawnTime = _enemySpawnTime - (Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.poi_difficulty * 5);
        

        level_setup_timer = new Timer<bool, LevelStatesEnum>(level_setup_time, ChangeLevelState, LevelStatesEnum.Playing);

        level_escape_time = Overallgame_Controller_Script.overallgame_controller_singleton.CurrentPlayer.player_time;

        level_escape_timer = new Timer<bool, LevelStatesEnum>(level_escape_time, ChangeLevelState, LevelStatesEnum.GetToEscape);
        level_end_timer = new Timer<bool, LevelStatesEnum>(level_end_time, ChangeLevelState, LevelStatesEnum.CleanupLose);
        level_exit_timer = new Timer<bool, LevelStatesEnum>(level_exit_time, ChangeLevelState, LevelStatesEnum.CleanupWin);
        enemySpawn_Timer = new Timer<bool, float>(_enemySpawnTime, SpawnEnemy, _enemySpawnTime);
        enemySpawn_Timer.repeatable = true;
        level_exit_timer.Pause_Timer(true);

        timer_text_ref = level_setup_timer;

        //extras
        _scoreQueueEnumerator = ScoreEffects();
        _motePlacerEnumerator = MotePlacer();
        StartCoroutine(MotePlacer());
        StartCoroutine(_scoreQueueEnumerator);

        TutorialObject_Script.singleton.FindandPlayTutorialObject("start_play");
    }

    private void Update()
    {
        _currentStateClass.OnUpdateState(this);
    }

    public bool ChangeLevelState(LevelStatesEnum _newState)
    {
        if(CurrentLevelState == _newState){return false;}
        if(_currentStateClass != null){_currentStateClass.OnExitState(this);}
        switch (_newState)
        {
            case LevelStatesEnum.Setup:
                _currentStateClass = new LevelplayState_Setup();
                break;
            case LevelStatesEnum.Playing:
                _currentStateClass = new LevelplayState_Playing();
                break;
            case LevelStatesEnum.Paused:
                _currentStateClass = new LevelplayState_Paused();
                break;
            case LevelStatesEnum.GetToEscape:
                _currentStateClass = new LevelplayState_GetToEscape();
                break;
            case LevelStatesEnum.CleanupLose:
                if(player_on_exit){_currentStateClass = new LevelplayState_CleanupWin();break;}
                _currentStateClass = new LevelplayState_CleanupLose();
                break;
            case LevelStatesEnum.CleanupWin:
                _currentStateClass = new LevelplayState_CleanupWin();
                break;
            case LevelStatesEnum.PlayerOnExit:
                _currentStateClass = new LevelplayState_OnExit();
                break;
            default:
                Debug.LogWarning("Incorrect state passed to ChangeLevelState!");
                return false;
        }
        LastLevelState = CurrentLevelState;
        _currentStateClass.OnEnterState(this);
        CurrentLevelState = _newState;
        return true;
    }

    public Grid_Data Find_Grid_Data(Vector2Int grid_pos)
    {
        if(grid_pos.x < 0 || grid_pos.x >= x_lead_map_coord_array.Length || grid_pos.y < 0 || grid_pos.y >= x_lead_map_coord_array[grid_pos.x].Length)
        {
            return null;
        }
        return x_lead_map_coord_array[grid_pos.x][grid_pos.y];
    }

    public bool SpawnEnemy(float timeReset)
    {
        
        IEnumerable<Grid_Data> emptyGridPos = 
            from row in x_lead_map_coord_array
            from cell in row
            where cell.resident == null && cell.playable && Vector3.Distance(cell.actual_pos, current_player.transform.position) > 5 && Vector3.Distance(cell.actual_pos, current_player.transform.position) < _playerViewCullDist
            select cell;
        
        if(emptyGridPos.Count() == 0)
        {
            return false;
        }
        

        _enemyStorageScript.SpawnEnemy(emptyGridPos.ElementAt((int)Global_Vars.rand_num_gen.Next(0,emptyGridPos.Count())).grid_pos,-1);
        return true;
    }

    public void Show_Exit()
    {
        if(TestExit.activeSelf){PlaceExitAura(Find_Grid_Data(player_start_gridpos).actual_pos); return;}
        TestExit.SetActive(true);
        TestExit.transform.position = Find_Grid_Data(player_start_gridpos).actual_pos;
        PlaceExitAura(Find_Grid_Data(player_start_gridpos).actual_pos);
    }

    public void Player_Enter_Exit()
    {
        ChangeLevelState(LevelStatesEnum.PlayerOnExit);
    }

    public void Player_Left_Exit()
    {
        if(!player_left_exit){player_left_exit = true; Show_Exit();}
        player_on_exit = false;
        if(!level_escape_timer.timer_finished_bool){ChangeLevelState(LevelStatesEnum.Playing);return;}
        ChangeLevelState(LevelStatesEnum.GetToEscape);
    }

#region Scoring
    public void Clean_Rock_Queue(bool _instantPop)
    {
        List<Rock_Script> instanced_rock_queue = new List<Rock_Script>();
        foreach (Rock_Script item in rocks_queue_for_destruction)
        {
            instanced_rock_queue.Add(item);
        }
        rocks_queue_for_destruction.Clear();

        if(_instantPop){Pregame_Rock_Pop(instanced_rock_queue); return;}

        StartCoroutine(Playerinput_Controller_Script.playerinput_controller_singleton.Shake_Camera(instanced_rock_queue.Count/4,0.08f));
        StartCoroutine(Timed_Rock_Pop(instanced_rock_queue));
    }

    private void Pregame_Rock_Pop(List<Rock_Script> new_rock_queue)
    {
        foreach (Rock_Script item in new_rock_queue)
        {
            item.DeleteRock();
        }
    }

    IEnumerator Timed_Rock_Pop(List<Rock_Script> new_rock_queue)
    {
        foreach (Rock_Script item in new_rock_queue)
        {
            GameObject _rockExpl = RockExplosionPool.CallNext();
            _rockExpl.SetActive(true);            
            _rockExpl.transform.position = Find_Grid_Data(item.grid_pos).actual_pos;

            GameObject _scoreParticles = ScoreParticlesPool.CallNext();
            _scoreParticles.SetActive(true);            
            _scoreParticles.transform.position = Find_Grid_Data(item.grid_pos).actual_pos;

            
            if(item.secondary_rock_type.secondary_type != Secondary_Rock_Types_Enum.none){resources_collected_array[(int)item.secondary_rock_type.secondary_type] += 1;}
            item.Pop_Rock();

            yield return new WaitForSeconds(.05f);
        }

    }

    public void AddScore()
    {
        _scoreQueue += 1;
        // resources_collected_array[0] += 1;
        
    }

    IEnumerator ScoreEffects()
    {
        string[] _scoreSounds = {"Game_ScoreUpShort1", "Game_ScoreUpShort2", "Game_ScoreUpShort3", "Game_ScoreUpShort4", "Game_ScoreUpShort5"};
        int _soundIndex = _scoreSounds.Length-1;
        while(true)
        {
            yield return new WaitForSeconds(_scoreQueueDecSpeed);
            if(_scoreQueue <= 0){continue;}

            CollectSparkle.Play();
            _scoreQueue -= 1;
            resources_collected_array[0] += 1;
            Sound_Events.Play_Sound(_scoreSounds[_soundIndex]);
            _soundIndex -= 1;
            if(_soundIndex <= 0){_soundIndex= _scoreSounds.Length-1;}

            if(score_menu_container.activeSelf)
            {
                foreach (ScoreItem_Script item in ui_scoreitems)
                {
                    item.Update_My_Score(resources_collected_array);
                }
            }
        }
    }
#endregion

#region  MenuItems
    public void Show_Ingame_Inventory()
    {
        if(!UI_animator.GetBool("Open"))
        {
            Sound_Events.Play_Sound("Game_Click");
            UI_animator.SetBool("Open", true);
            foreach (ScoreItem_Script item in ui_scoreitems)
            {
                item.Update_My_Score(resources_collected_array);
            }
            return;
        }
        Sound_Events.Play_Sound("Game_ClickOff");
        UI_animator.SetBool("Open", false);
        
    }

    public bool Exit_Level_To_Map(bool unUsed)
    {
        Sound_Events.Stop_Sound(_selectedMusic);
        Overallgame_Controller_Script.overallgame_controller_singleton.SaveCurrentPlayer();
        ScnTrans_Script.singleton.ScnTransOut(Scene_Enums.levelselect);
        // Loading_Controller_Script.loading_controller_singleton.Load_Next_Scene(Scene_Enums.levelselect);
        return true; //unUsed
    }
    public void Exit_Level_To_MainMenu()
    {
        Sound_Events.Stop_Sound(_selectedMusic);
        ScnTrans_Script.singleton.ScnTransOut(Scene_Enums.mainmenu);
        // Loading_Controller_Script.loading_controller_singleton.Load_Next_Scene(Scene_Enums.mainmenu);
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
        player_start_gridpos = wall_coord_list.ElementAt((int)UnityEngine.Random.Range(0,wall_coord_list.Count()));

        for (int i = 0; i < wall_coord_list.Count(); i++)
        {
            foreach (Grid_Data neh in Find_Grid_Data(player_start_gridpos).Return_Neighbors())
            {
                if(neh != null && neh.resident != null && neh.resident.matchable)
                {

                    Rock_Script matchable = (Rock_Script)neh.resident;
                    matchable.DeleteRock();
                    

                    drop_ship.Place_Dropship(player_start_gridpos, neh.grid_pos);
                    current_player.Place_Resident(neh.grid_pos);
                    CheckToCull();
                    player_on_exit = true;
                    current_player.Change_Level_Actor_State(Level_Actor_States_Enum.Frozen);
                    player_start_gridpos = neh.grid_pos;
                    return;
                }
            }
            player_start_gridpos = wall_coord_list.ElementAt((int)Global_Vars.rand_num_gen.Next(0,wall_coord_list.Count()));
        }
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
            if(item.grid_pos.x <= 0 || item.grid_pos.x >= map_x_size-1|| item.grid_pos.y <= 0 || item.grid_pos.y >= map_y_size-1) //make sure there is no edge matchables
            {
                new_go = Instantiate(TestWall, parent: wall_container.transform);
                new_go.GetComponent<GridResident_Script>().Place_Resident(item.grid_pos);
                wall_coord_list.Add(item.grid_pos);
                continue;
            }
            new_go = Instantiate(TestRock, parent: rock_container.transform);
            
            new_go.GetComponent<GridResident_Script>().Place_Resident(item.grid_pos);
            
            if(item.grid_pos.y <= 0){new_go.GetComponent<Wall_Script>().FadeRock();}

            Instantiate(floor_go, new_go.transform.position, Quaternion.AngleAxis(Global_Vars.rand_num_gen.Next(0,180), Vector3.forward), floor_container.transform);

            item.playable = true;
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
                    new_go = Instantiate(TestWall, parent: wall_container.transform);
                    new_go.GetComponent<GridResident_Script>().Place_Resident(neighbor.grid_pos);

                    Vector2Int northVector = new Vector2Int(new_go.GetComponent<GridResident_Script>().grid_pos.x,new_go.GetComponent<GridResident_Script>().grid_pos.y + 1);
                    if
                    (   
                        Find_Grid_Data(northVector) != null &&
                        Find_Grid_Data(northVector).resident != null &&
                        Find_Grid_Data(northVector).resident.matchable
                    )
                    {new_go.GetComponent<Wall_Script>().FadeRock();}
                    wall_coord_list.Add(neighbor.grid_pos);
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
                        Rock_Types_Storage_Script.rock_types_controller_singleton.rock_so_list[UnityEngine.Random.Range(0,Rock_Types_Storage_Script.rock_types_controller_singleton.rock_so_list.Count)],
                        null
                        );
                }
            }
        }
    }
    private Secondary_Rock_ScriptableObject Determine_Secondary_Rock_Type()
    {
        Secondary_Rock_ScriptableObject temp_sec_rock_type = null;
        int rock_chance = 100 - Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.poi_difficulty;

        if((int)UnityEngine.Random.Range(0,rock_chance) <= 1)
        {
            temp_sec_rock_type = 
            Rock_Types_Storage_Script.rock_types_controller_singleton.secondary_so_list[UnityEngine.Random.Range(0,Rock_Types_Storage_Script.rock_types_controller_singleton.secondary_so_list.Count)];
        }
        return temp_sec_rock_type;
    }
    private void Deliver_Secondary_Rock_Types(int lodeToCheck, Secondary_Rock_Types_Enum rockTypeEnum)
    {
        int rockTypeInt = (int)rockTypeEnum;

        IEnumerable<Grid_Data> residents = 
        from row in x_lead_map_coord_array
        from cell in row
        where cell.resident != null && cell.resident.matchable
        select cell;

        for (int i = 0; i < lodeToCheck; i++)
        {
            int randIndex = (int)UnityEngine.Random.Range(0,residents.Count());

            Rock_Script rockResident = (Rock_Script)Find_Grid_Data(residents.ElementAt(randIndex).grid_pos).resident;

            rockResident.Change_Seconday_Rock_Type(Rock_Types_Storage_Script.rock_types_controller_singleton.secondary_so_list[rockTypeInt]);
        }
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

#region Opti
    public void CheckToCull()
    {
        // Vector2Int _maxCoord = new Vector2Int(current_player.grid_pos.x + _playerViewCullDist, current_player.grid_pos.y + _playerViewCullDist);
        // Vector2Int _minCoord = new Vector2Int(current_player.grid_pos.x - _playerViewCullDist, current_player.grid_pos.y - _playerViewCullDist);;

        IEnumerable<GridResident_Script> grid_res = 
            from row in x_lead_map_coord_array
            from cell in row
            where cell.resident != null
            select cell.resident;
        
        foreach (GridResident_Script res in grid_res)
        {

            // if(res.grid_pos.x > _maxCoord.x || res.grid_pos.y > _maxCoord.y || res.grid_pos.x < _minCoord.x || res.grid_pos.y < _minCoord.y)
            // {
            //     res.gameObject.SetActive(false);
            // }
            // res.gameObject.SetActive(true);
            if(Vector3.Distance(current_player.gameObject.transform.position, res.gameObject.transform.position) > _playerViewCullDist)
            {
                res.gameObject.SetActive(false);
                continue;
            }
            res.gameObject.SetActive(true);
        }
    }
    
    private void OnDestroy()
    {
        StopCoroutine(_scoreQueueEnumerator);
        StopCoroutine(_motePlacerEnumerator);
    }
#endregion

#region Juice
    private void PlaceExitAura(Vector3 _newPos)
    {
        auraPiller_GO.SetActive(true);
        auraPiller_GO.transform.position = _newPos;
    }
    private IEnumerator MotePlacer()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(1);
            GameObject _dustmote_GO = DustMotePool.CallNext();
            _dustmote_GO.SetActive(true);
            _dustmote_GO.transform.position = Find_Grid_Data(new Vector2Int(Global_Vars.rand_num_gen.Next(0,map_x_size),Global_Vars.rand_num_gen.Next(0,map_y_size))).actual_pos;
        }
    }
#endregion
}
