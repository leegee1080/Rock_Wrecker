using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelplayStatesAbstractClass
{
    public abstract void OnEnterState(Levelplay_Controller_Script _controllerScript);
    public abstract void OnExitState(Levelplay_Controller_Script _controllerScript);
    public abstract void OnUpdateState(Levelplay_Controller_Script _controllerScript);
}

public class LevelplayState_Setup: LevelplayStatesAbstractClass
{
    public override void OnEnterState(Levelplay_Controller_Script _controllerScript)
    {
        Debug.Log("Enter Setup");
    }   
    public override void OnExitState(Levelplay_Controller_Script _controllerScript)
    {
        Debug.Log("Exit Setup");
    }   
    public override void OnUpdateState(Levelplay_Controller_Script _controllerScript)
    {
        
    }   
}
public class LevelplayState_Playing: LevelplayStatesAbstractClass
{
    public override void OnEnterState(Levelplay_Controller_Script _controllerScript)
    {
        Debug.Log("Enter Play");
        _controllerScript.current_player.gameObject.SetActive(true);
        _controllerScript.drop_ship.Open_Door();
        // _controllerScript.timer_text_ref = _controllerScript.level_escape_timer;
        // _controllerScript.level_setup_timer.timer_finished_bool = true;
        _controllerScript.current_player.Change_Level_Actor_State(Level_Actor_States_Enum.Normal);
    }   
    public override void OnExitState(Levelplay_Controller_Script _controllerScript)
    {
        Debug.Log("Exit Play");
    }   
    public override void OnUpdateState(Levelplay_Controller_Script _controllerScript)
    {
        
    }   
}
public class LevelplayState_Paused: LevelplayStatesAbstractClass
{
    public override void OnEnterState(Levelplay_Controller_Script _controllerScript)
    {

    }   
    public override void OnExitState(Levelplay_Controller_Script _controllerScript)
    {
        
    }   
    public override void OnUpdateState(Levelplay_Controller_Script _controllerScript)
    {
        
    }   
}
public class LevelplayState_Escape: LevelplayStatesAbstractClass
{
    public override void OnEnterState(Levelplay_Controller_Script _controllerScript)
    {

    }   
    public override void OnExitState(Levelplay_Controller_Script _controllerScript)
    {
        
    }   
    public override void OnUpdateState(Levelplay_Controller_Script _controllerScript)
    {
        
    }   
}
public class LevelplayState_Cleanup: LevelplayStatesAbstractClass
{
    public override void OnEnterState(Levelplay_Controller_Script _controllerScript)
    {

    }   
    public override void OnExitState(Levelplay_Controller_Script _controllerScript)
    {
        
    }   
    public override void OnUpdateState(Levelplay_Controller_Script _controllerScript)
    {
        
    }   
}
public class LevelplayState_Exit: LevelplayStatesAbstractClass
{
    public override void OnEnterState(Levelplay_Controller_Script _controllerScript)
    {

    }   
    public override void OnExitState(Levelplay_Controller_Script _controllerScript)
    {
        
    }   
    public override void OnUpdateState(Levelplay_Controller_Script _controllerScript)
    {
        
    }   
}
