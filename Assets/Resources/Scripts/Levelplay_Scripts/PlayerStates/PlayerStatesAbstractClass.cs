using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerStatesAbstractClass
{
    public abstract void OnEnterState(LevelPlayer_Script _cont);
    public abstract void OnExitState(LevelPlayer_Script _cont);
    public abstract void OnUpdateState(LevelPlayer_Script _cont);
}
public class PlayerState_Frozen: PlayerStatesAbstractClass
{
     public override void OnEnterState(LevelPlayer_Script _cont)
    {
        
    }   
    public override void OnExitState(LevelPlayer_Script _cont)
    {
        
    }   
    public override void OnUpdateState(LevelPlayer_Script _cont)
    {

    }   
}
public class PlayerState_Normal: PlayerStatesAbstractClass
{
    public override void OnEnterState(LevelPlayer_Script _cont)
    {
        
    }   
    public override void OnExitState(LevelPlayer_Script _cont)
    {

    }   
    public override void OnUpdateState(LevelPlayer_Script _cont)
    {
        if(_cont.player_actions.MoveUp.IsPressed())
        {
            _cont.Move((int)Actor_Direction_Enum.up);
        }
        else if(_cont.player_actions.MoveRight.IsPressed())
        {
            _cont.Move((int)Actor_Direction_Enum.right);
        }
        else if(_cont.player_actions.MoveDown.IsPressed())
        {
            _cont.Move((int)Actor_Direction_Enum.down);
        }
        else if(_cont.player_actions.MoveLeft.IsPressed())
        {
            _cont.Move((int)Actor_Direction_Enum.left);
        } 
    }   
}
public class PlayerState_Dead: PlayerStatesAbstractClass
{
    public override void OnEnterState(LevelPlayer_Script _cont)
    {
        
    }   
    public override void OnExitState(LevelPlayer_Script _cont)
    {
        
    }   
    public override void OnUpdateState(LevelPlayer_Script _cont)
    {

    }   
}
public class PlayerState_Pause: PlayerStatesAbstractClass
{
    public override void OnEnterState(LevelPlayer_Script _cont)
    {
        
    }   
    public override void OnExitState(LevelPlayer_Script _cont)
    {
        
    }   
    public override void OnUpdateState(LevelPlayer_Script _cont)
    {

    }   
}
public class PlayerState_Moving: PlayerStatesAbstractClass
{
    public override void OnEnterState(LevelPlayer_Script _cont)
    {
        
    }   
    public override void OnExitState(LevelPlayer_Script _cont)
    {
        _cont.playerAnimator.SetBool("Run", false);
    }   
    public override void OnUpdateState(LevelPlayer_Script _cont)
    {
        if(_cont.move_timer >0){_cont.move_timer -= Time.deltaTime;}
        if(_cont.move_timer<=0){_cont.Change_Level_Actor_State(Level_Actor_States_Enum.Normal);}
    }   
}
