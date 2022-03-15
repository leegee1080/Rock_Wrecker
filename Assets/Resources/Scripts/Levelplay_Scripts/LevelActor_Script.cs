using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Actor_Direction_Enum
{
    up,
    right,
    down,
    left
}

public enum Level_Actor_States_Enum
{   
    Setup,
    Normal,
    Dead,
    Frozen,
    Pause,
    Moving
}

public class LevelActor_Script : GridResident_Script
{

    public Level_Actor_States_Enum current_state;
    public Level_Actor_States_Enum last_state;

    [SerializeField]private Level_Actor_States_Enum _inspectorPlayerState; //do not change this directly
    public Level_Actor_States_Enum CurrentPlayerState
    {
        get { return _inspectorPlayerState; }
        set { _inspectorPlayerState = value; Change_Level_Actor_State(CurrentPlayerState);}
    }
    private LevelplayStatesAbstractClass _currentStateClass;

    public override void Start()
    {
        base.Start();
        Level_Events.pause_toggle_event += Pause_Resident;
    }

    public virtual Level_Actor_States_Enum Change_Level_Actor_State(Level_Actor_States_Enum new_state)
    {
        switch (new_state)
        {
            case Level_Actor_States_Enum.Normal:
                break;
            case Level_Actor_States_Enum.Dead:
                break;
            case Level_Actor_States_Enum.Frozen:
                break;
            case Level_Actor_States_Enum.Pause:
                break;
            case Level_Actor_States_Enum.Moving:
                break;
            default:
                return current_state;
        }
        last_state = current_state;
        current_state = new_state;
        return current_state;
    }

    public void RotateActor(Vector3 eulerAngles)
    {
        if(transform.localEulerAngles == eulerAngles){return;}
        transform.localEulerAngles = eulerAngles;
    }

    public virtual void Pause_Resident(bool new_pause_state)
    {
        if(new_pause_state){Change_Level_Actor_State(Level_Actor_States_Enum.Pause);return;}
        Change_Level_Actor_State(last_state);
    }

    public override void OnDestroy() 
    {
        base.OnDestroy();
        Level_Events.pause_toggle_event -= Pause_Resident;
    }

}
