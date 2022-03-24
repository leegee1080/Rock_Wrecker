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
    void Attack(Vector2Int gridPos);
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
    [HideInInspector]public GameObject _bodyGO;
    private GameObject _spawnParticleGO;


    //data
    public Timer<bool, bool> decisionTimer;
    public float move_timer = 0;
    public float spawnTime = 0;
    public Timer<Level_Actor_States_Enum, Level_Actor_States_Enum> spawnTimer;
    private EnemyStatesAbstractClass _currentEnemyStateClass;
    public Animator enemyAnimator;
    public EnemyAttack_Base _enemyAttackClass;
    public EnemyBeh_Base _enemyBehClass;



    public override void Start()
    {
        base.Start();
        name = MyDataSO.name;
        StartSpawn();
    }

    public void StartSpawn()
    {
        name = MyDataSO.name;
        _type = MyDataSO.enemyType;
        _bodyGO = Instantiate(MyDataSO.bodyGO, parent: gameObject.transform);
        _spawnParticleGO = Instantiate(MyDataSO.spawnParticleGO, parent: gameObject.transform);
        enemyAnimator = _bodyGO.GetComponent<Animator>();

        _currentEnemyStateClass = new EnemyState_Start();
        _currentEnemyStateClass.OnEnterState(this);

        _enemyAttackClass = GetComponent<EnemyAttack_Base>();
        _enemyAttackClass.GatherData();
        _enemyBehClass = GetComponent<EnemyBeh_Base>();
        _enemyBehClass.GatherData();

        spawnTimer = new Timer<Level_Actor_States_Enum, Level_Actor_States_Enum>(spawnTime, Change_Level_Actor_State, Level_Actor_States_Enum.Normal);
        decisionTimer = new Timer<bool, bool>(MyDataSO.decisionTime, MakeDecision, true);
    }

    private bool MakeDecision(bool behSelected)
    {
        _enemyBehClass.ProcessEnemyTurn();
        decisionTimer = new Timer<bool, bool>(MyDataSO.decisionTime, MakeDecision, true);
        return true;
    }

    public override Level_Actor_States_Enum Change_Level_Actor_State(Level_Actor_States_Enum new_state)
    {
        if(new_state == current_state){return current_state;}
        if(_currentEnemyStateClass != null){ _currentEnemyStateClass.OnExitState(this);}
        switch (new_state)
        {
            case Level_Actor_States_Enum.Setup:
                _currentEnemyStateClass = new EnemyState_Start();
                break;
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

    public void Move(int Direction, float moveTimerAddition = 0)
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

        if(Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[desired_coord.x][desired_coord.y].resident != null && Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[desired_coord.x][desired_coord.y].resident.moveable)
        {
            move_timer = 0.25f + moveTimerAddition;
            enemyAnimator.SetTrigger("Attack");
            _enemyAttackClass.Attack(desired_coord);
            Change_Level_Actor_State(Level_Actor_States_Enum.Moving);
            return;
        }

        if(Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[desired_coord.x][desired_coord.y].resident == null)
        {
            move_timer = 0.1f + moveTimerAddition;
            enemyAnimator.SetBool("Run", true);
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

}
