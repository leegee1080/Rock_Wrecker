using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class LevelplayStatesAbstractClass
{
    public abstract void OnEnterState(Levelplay_Controller_Script _cont);
    public abstract void OnExitState(Levelplay_Controller_Script _cont);
    public abstract void OnUpdateState(Levelplay_Controller_Script _cont);
}

public class LevelplayState_Setup: LevelplayStatesAbstractClass
{
    public override void OnEnterState(Levelplay_Controller_Script _cont)
    {
        _cont.timer_text.color = new Color32(129,255,200,255);
        _cont.timer_text.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, new Color(129,255,200,1));
    }   
    public override void OnExitState(Levelplay_Controller_Script _cont)
    {
        _cont.current_player.gameObject.SetActive(true);
        _cont.current_player.Change_Level_Actor_State(Level_Actor_States_Enum.Normal);
        _cont.drop_ship.Open_Door();
        
    }   
    public override void OnUpdateState(Levelplay_Controller_Script _cont)
    {
        _cont.level_setup_timer.Decrement_Timer(Time.deltaTime);
    }   
}
public class LevelplayState_Playing: LevelplayStatesAbstractClass
{
    public override void OnEnterState(Levelplay_Controller_Script _cont)
    {
        _cont.timer_text_ref = _cont.level_escape_timer;
        _cont.timer_text.color = new Color32(129,255,200,255);
        _cont.timer_text.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, new Color(129,255,200,1));
        _cont.level_setup_timer.timer_finished_bool = true;
    }   
    public override void OnExitState(Levelplay_Controller_Script _cont)
    {

    }   
    public override void OnUpdateState(Levelplay_Controller_Script _cont)
    {
        if(_cont.rocks_queue_for_destruction.Count > 0)
        {
            _cont.Clean_Rock_Queue(false);
        }
        _cont.level_escape_timer.Decrement_Timer(Time.deltaTime); 
    }   
}
public class LevelplayState_Paused: LevelplayStatesAbstractClass
{
    public override void OnEnterState(Levelplay_Controller_Script _cont)
    {
        _cont.level_setup_timer.Pause_Timer(true);
        _cont.level_escape_timer.Pause_Timer(true);
        _cont.level_end_timer.Pause_Timer(true);
        Level_Events.Invoke_Pause_Toggle_Event(true);
    }   
    public override void OnExitState(Levelplay_Controller_Script _cont)
    {
        _cont.level_setup_timer.Pause_Timer(false);
        _cont.level_escape_timer.Pause_Timer(false);
        _cont.level_end_timer.Pause_Timer(false);
        Level_Events.Invoke_Pause_Toggle_Event(false);
    }   
    public override void OnUpdateState(Levelplay_Controller_Script _cont)
    {
        
    }   
}
public class LevelplayState_GetToEscape: LevelplayStatesAbstractClass
{
    public override void OnEnterState(Levelplay_Controller_Script _cont)
    {
        _cont.Show_Exit();
        _cont.timer_text.color = new Color(255,0,0,1);
        _cont.timer_text.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, new Color(255,0,0,1));
        _cont.timer_text_ref = _cont.level_end_timer;
        _cont.level_setup_timer.timer_finished_bool = true;
        _cont.level_escape_timer.timer_finished_bool = true;
    }   
    public override void OnExitState(Levelplay_Controller_Script _cont)
    {
        
    }   
    public override void OnUpdateState(Levelplay_Controller_Script _cont)
    {
        if(_cont.rocks_queue_for_destruction.Count > 0)
        {
            _cont.Clean_Rock_Queue(false);
        }
        _cont.level_end_timer.Decrement_Timer(Time.deltaTime);   
    }   
}
public class LevelplayState_CleanupLose: LevelplayStatesAbstractClass
{
    public override void OnEnterState(Levelplay_Controller_Script _cont)
    {
        Level_Events.Invoke_Pause_Toggle_Event(true);
        _cont.level_setup_timer.timer_finished_bool = true;
        _cont.level_escape_timer.timer_finished_bool = true;
        _cont.level_end_timer.timer_finished_bool = true;
        Debug.Log("Level Lose");
    }   
    public override void OnExitState(Levelplay_Controller_Script _cont)
    {
        
    }   
    public override void OnUpdateState(Levelplay_Controller_Script _cont)
    {
        
    }   
}
public class LevelplayState_CleanupWin: LevelplayStatesAbstractClass
{
    public override void OnEnterState(Levelplay_Controller_Script _cont)
    {
        Level_Events.Invoke_Pause_Toggle_Event(true);
        _cont.level_setup_timer.timer_finished_bool = true;
        _cont.level_escape_timer.timer_finished_bool = true;
        _cont.level_end_timer.timer_finished_bool = true;
        _cont.Exit_Level_To_Map();
        Debug.Log("Level Win");
    }   
    public override void OnExitState(Levelplay_Controller_Script _cont)
    {
        
    }   
    public override void OnUpdateState(Levelplay_Controller_Script _cont)
    {
        
    }   
}
public class LevelplayState_OnExit: LevelplayStatesAbstractClass
{
    public override void OnEnterState(Levelplay_Controller_Script _cont)
    {
        _cont.player_on_exit = true;
        _cont.level_exit_timer.Pause_Timer(false);
        _cont.timer_text_ref = _cont.level_exit_timer;
        _cont.timer_text.color = new Color(0,255,0,1);
        _cont.timer_text.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, new Color(0,255,0,1));
    }   
    public override void OnExitState(Levelplay_Controller_Script _cont)
    {
        _cont.player_on_exit = false;
        _cont.level_exit_timer.Pause_Timer(true);
        _cont.level_exit_timer.Reset_Timer();
        _cont.timer_text.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, new Color(129,255,200,1));
    }   
    public override void OnUpdateState(Levelplay_Controller_Script _cont)
    {
        _cont.level_exit_timer.Decrement_Timer(Time.deltaTime);
    }   
}
