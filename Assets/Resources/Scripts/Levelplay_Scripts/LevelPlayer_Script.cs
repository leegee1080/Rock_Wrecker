using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LevelPlayer_Script : LevelActor_Script
{
    public Player_ScriptableObject data;
    [SerializeField] private GameObject _mainMeshGO;
    [SerializeField] private GameObject _deathMeshGO;
    
    public PlayerInputActions.PlayerControlsActions player_actions{get; private set;}
    public float move_timer = 0;

    private PlayerStatesAbstractClass _currentPlayerStateClass;

    [SerializeField] ParticleSystem _pickUpRock_ps;
    public Animator playerAnimator;

    [SerializeField] GameObject deathParticle;
    [SerializeField] GameObject _shieldParticle;
    [SerializeField] GameObject _shieldBreakParticle;



    public override void Start()
    {
        base.Start();
        name = data.name;
        player_actions = Playerinput_Controller_Script.playerinput_controller_singleton.player_input_actions.PlayerControls;
        _deathMeshGO = data.deathMesh;
        if(Overallgame_Controller_Script.overallgame_controller_singleton.CurrentPlayer.DroneShields > 0)
        {
            _shieldParticle = Instantiate(_shieldParticle, parent: gameObject.transform);
            _shieldParticle.SetActive(true);
        }
        // Input_Control_Events.move_up_event += Moveup_Player;
        // Input_Control_Events.move_down_event += Movedown_Player;
        // Input_Control_Events.move_right_event += Moveright_Player;
        // Input_Control_Events.move_left_event += Moveleft_Player;
    }

    public override Level_Actor_States_Enum Change_Level_Actor_State(Level_Actor_States_Enum new_state)
    {
        if(new_state == current_state){return current_state;}
        if(_currentPlayerStateClass != null){ _currentPlayerStateClass.OnExitState(this);}
        switch (new_state)
        {
            case Level_Actor_States_Enum.Normal:
                _currentPlayerStateClass = new PlayerState_Normal();
                break;
            case Level_Actor_States_Enum.Dead:
                _currentPlayerStateClass = new PlayerState_Dead();
                break;
            case Level_Actor_States_Enum.Frozen:
                _currentPlayerStateClass = new PlayerState_Frozen();
                break;
            case Level_Actor_States_Enum.Pause:
                _currentPlayerStateClass = new PlayerState_Pause();
                break;
            case Level_Actor_States_Enum.Moving:
                _currentPlayerStateClass = new PlayerState_Moving();
                break;
            default:
                return current_state;
        }
        last_state = current_state;
        current_state = new_state;
        _currentPlayerStateClass.OnEnterState(this);
        return current_state;
    }

    private void Update()
    {
        _currentPlayerStateClass.OnUpdateState(this);
    }

    public void Move(int Direction)
    {
        Vector2Int desired_coord = Vector2Int.zero;
        switch (Direction)
        {
             case (int)Actor_Direction_Enum.up:
                RotateActor(Vector3.zero);
                desired_coord = grid_pos + new Vector2Int(0,1);
                break;
            case (int)Actor_Direction_Enum.right:
                RotateActor(new Vector3(0,90,0));
                desired_coord = grid_pos + new Vector2Int(1,0);
                break;
            case (int)Actor_Direction_Enum.down:
                RotateActor(new Vector3(0,180,0));
                desired_coord = grid_pos + new Vector2Int(0,-1);
                break;
            case (int)Actor_Direction_Enum.left:
                RotateActor(new Vector3(0,270,0));
                desired_coord = grid_pos + new Vector2Int(-1,0);
                break;
            default:
                Debug.LogError("No direction int passed to player!");
                return;
        }
        Levelplay_Controller_Script.levelplay_controller_singleton.CheckToCull();

        if(Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[desired_coord.x][desired_coord.y].resident != null && Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[desired_coord.x][desired_coord.y].resident.moveable)
        {
            move_timer = 0.25f;
            Check_For_Exit_Tile(desired_coord);
            playerAnimator.SetTrigger("Melee Attack");
            _pickUpRock_ps.Play();
            Sound_Events.Play_Sound("Game_RockSwap");
            Sound_Events.Delay_Play_Sound("Game_FootStep",0.1f);
            SwapResidentsTweened(this, Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[desired_coord.x][desired_coord.y].resident, 1f);
            Change_Level_Actor_State(Level_Actor_States_Enum.Moving);
            return;
        }

        if(desired_coord.x > 0 && desired_coord.y > 0 && desired_coord.x < Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array.Length && desired_coord.y < Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[desired_coord.x].Length && Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[desired_coord.x][desired_coord.y].resident == null)
        {
            Sound_Events.Play_Sound("Game_FootStep");
            move_timer = 0.10f;
            playerAnimator.SetBool("Run", true);
            Check_For_Exit_Tile(desired_coord);
            Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[grid_pos.x][grid_pos.y].resident = null;
            PlaceResidentTween(desired_coord,move_timer,false);
            Change_Level_Actor_State(Level_Actor_States_Enum.Moving);
            return;
        }
        
    }

    public override bool Attacked(GridResident_Script attacker)
    {
        print("player attacked by: " + attacker.name);
        if(Levelplay_Controller_Script.levelplay_controller_singleton.CurrentLevelState == LevelStatesEnum.CleanupWin || Levelplay_Controller_Script.levelplay_controller_singleton.CurrentLevelState == LevelStatesEnum.CleanupLose){return true;}
        if(current_state == Level_Actor_States_Enum.Dead || current_state == Level_Actor_States_Enum.Pause){return true;}
        if(Overallgame_Controller_Script.overallgame_controller_singleton.CurrentPlayer.DroneShields > 0)
        {
            LevelEnemy_Script enemy = (LevelEnemy_Script)attacker;
            enemy.Change_Level_Actor_State(Level_Actor_States_Enum.Dead);
            BreakShield();
            return false;
        }
        deathParticle.SetActive(true);
        Change_Level_Actor_State(Level_Actor_States_Enum.Dead);
        Levelplay_Controller_Script.levelplay_controller_singleton.Find_Grid_Data(grid_pos).resident = null;
        _mainMeshGO.SetActive(false);
        _deathMeshGO = Instantiate(_deathMeshGO, parent: transform);
        Levelplay_Controller_Script.levelplay_controller_singleton.ChangeLevelState(LevelStatesEnum.CleanupLose);
        return true;
    }

    public void BreakShield()
    {
        Debug.Log("Shield Broken");
        Sound_Events.Play_Sound("Game_ShieldOff");
        _shieldParticle.SetActive(false);
        Instantiate(_shieldBreakParticle, parent: gameObject.transform);
        Overallgame_Controller_Script.overallgame_controller_singleton.CurrentPlayer.DroneShields -= 1;
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
