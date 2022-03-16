
using UnityEngine;

public abstract class EnemyStatesAbstractClass
{
    public abstract void OnEnterState(LevelEnemy_Script _cont);
    public abstract void OnExitState(LevelEnemy_Script _cont);
    public abstract void OnUpdateState(LevelEnemy_Script _cont);
}
public class EnemyState_Start: EnemyStatesAbstractClass
{
     public override void OnEnterState(LevelEnemy_Script _cont)
    {
        //instance PS and fire it
        //unhide enemy
    }   
    public override void OnExitState(LevelEnemy_Script _cont)
    {
        
    }   
    public override void OnUpdateState(LevelEnemy_Script _cont)
    {
        _cont.spawnTimer -= Time.deltaTime;;
        if(_cont.spawnTimer > 0){return;}
        _cont.Change_Level_Actor_State(Level_Actor_States_Enum.Normal);
    }   
}
public class EnemyState_Frozen: EnemyStatesAbstractClass
{
     public override void OnEnterState(LevelEnemy_Script _cont)
    {
        
    }   
    public override void OnExitState(LevelEnemy_Script _cont)
    {
        
    }   
    public override void OnUpdateState(LevelEnemy_Script _cont)
    {

    }   
}
public class EnemyState_Normal: EnemyStatesAbstractClass
{
    public override void OnEnterState(LevelEnemy_Script _cont)
    {
        
    }   
    public override void OnExitState(LevelEnemy_Script _cont)
    {

    }   
    public override void OnUpdateState(LevelEnemy_Script _cont)
    {

    }   
}
public class EnemyState_Dead: EnemyStatesAbstractClass
{
    public override void OnEnterState(LevelEnemy_Script _cont)
    {
        
    }   
    public override void OnExitState(LevelEnemy_Script _cont)
    {
        
    }   
    public override void OnUpdateState(LevelEnemy_Script _cont)
    {

    }   
}
public class EnemyState_Pause: EnemyStatesAbstractClass
{
    public override void OnEnterState(LevelEnemy_Script _cont)
    {
        
    }   
    public override void OnExitState(LevelEnemy_Script _cont)
    {
        
    }   
    public override void OnUpdateState(LevelEnemy_Script _cont)
    {

    }   
}
public class EnemyState_Moving: EnemyStatesAbstractClass
{
    public override void OnEnterState(LevelEnemy_Script _cont)
    {
        
    }   
    public override void OnExitState(LevelEnemy_Script _cont)
    {
        _cont.enemyAnimator.SetBool("Run", false);
    }   
    public override void OnUpdateState(LevelEnemy_Script _cont)
    {
        if(_cont.move_timer >0){_cont.move_timer -= Time.deltaTime;}
        if(_cont.move_timer<=0){_cont.Change_Level_Actor_State(_cont.last_state);}
    }   
}
