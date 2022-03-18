using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyTypes
{
    Normal,
    Killable
}

interface IcollectParent
{
    void GatherData();
}

interface Iattack
{
    void Attack();
}

interface IBeh
{
    void ProcessEnemyTurn();
}


// LevelEnemy_Script requires the GameObject to have a AttackTypeClass component and a BehTypeClass component
[RequireComponent(typeof(EnemyAttack_Base))]
[RequireComponent(typeof(EnemyBeh_Base))]
public class LevelEnemy_Script : LevelActor_Script
{
    //stats
    public EnemyScriptableObject MyDataSO;
    public EnemyTypes _type;
    private GameObject _bodyGO;
    private GameObject _spawnParticleGO;


    //data
    public float move_timer = 0;
    public float spawnTimer = 0;
    private EnemyStatesAbstractClass _currentEnemyStateClass;
    public Animator enemyAnimator;
    public EnemyAttack_Base _enemyAttackClass;
    public EnemyBeh_Base _enemyBehClass;



    public override void Start()
    {
        base.Start();
        StartSpawn();
    }

    public void StartSpawn()
    {
        name = MyDataSO.name;
        _type = MyDataSO.enemyType;
        _bodyGO = Instantiate(MyDataSO.bodyGO, parent: gameObject.transform);
        _spawnParticleGO = Instantiate(MyDataSO.spawnParticleGO, parent: gameObject.transform);
        enemyAnimator = _bodyGO.GetComponent<Animator>();

        //grab stats from SO
        //grab animator from instanced GO
        _currentEnemyStateClass = new EnemyState_Start();
        _currentEnemyStateClass.OnEnterState(this);

        _enemyAttackClass = GetComponent<EnemyAttack_Base>();
        _enemyBehClass = GetComponent<EnemyBeh_Base>();
    }

    

    public override Level_Actor_States_Enum Change_Level_Actor_State(Level_Actor_States_Enum new_state)
    {

        if(_currentEnemyStateClass != null){ _currentEnemyStateClass.OnExitState(this);}
        switch (new_state)
        {
            case Level_Actor_States_Enum.Normal:
                _currentEnemyStateClass = new EnemyState_Normal();
                break;
            case Level_Actor_States_Enum.Dead:
                _currentEnemyStateClass = new EnemyState_Dead();
                break;
            case Level_Actor_States_Enum.Frozen:
                _currentEnemyStateClass = new EnemyState_Frozen();
                break;
            case Level_Actor_States_Enum.Pause:
                _currentEnemyStateClass = new EnemyState_Pause();
                break;
            case Level_Actor_States_Enum.Moving:
                _currentEnemyStateClass = new EnemyState_Moving();
                break;
            default:
                return current_state;
        }
        last_state = current_state;
        current_state = new_state;
        _currentEnemyStateClass.OnEnterState(this);
        return current_state;
    }

    private void Update()
    {
        _currentEnemyStateClass.OnUpdateState(this);
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
                Debug.LogError("No direction int passed to enemay!" + name);
                return;
        }
        Levelplay_Controller_Script.levelplay_controller_singleton.CheckToCull();

        if(Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[desired_coord.x][desired_coord.y].resident != null && Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[desired_coord.x][desired_coord.y].resident.moveable)
        {
            move_timer = 0.25f;
            if(_enemyAttackClass != null){_enemyAttackClass.Attack();}
            return;
        }

        if(desired_coord.x > 0 && desired_coord.y > 0 && desired_coord.x < Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array.Length && desired_coord.y < Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[desired_coord.x].Length && Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[desired_coord.x][desired_coord.y].resident == null)
        {
            move_timer = 0.10f;
            enemyAnimator.SetBool("Run", true);
            Check_For_Exit_Tile(desired_coord);
            Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[grid_pos.x][grid_pos.y].resident = null;
            PlaceResidentTween(desired_coord,move_timer,false);
            Change_Level_Actor_State(Level_Actor_States_Enum.Moving);
            return;
        }
        
    }

    public override void Local_Board_Changed()
    {
        return; // no need to check enemy for matches
    }

    private void Check_For_Exit_Tile(Vector2Int desired_coord)
    {
        if(grid_pos == Levelplay_Controller_Script.levelplay_controller_singleton.player_start_gridpos){Levelplay_Controller_Script.levelplay_controller_singleton.Player_Left_Exit();}
        if(desired_coord == Levelplay_Controller_Script.levelplay_controller_singleton.player_start_gridpos){Levelplay_Controller_Script.levelplay_controller_singleton.Player_Enter_Exit();}
    }
}
