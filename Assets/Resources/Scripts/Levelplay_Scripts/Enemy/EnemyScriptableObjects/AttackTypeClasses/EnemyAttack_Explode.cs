using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack_Explode : EnemyAttack_Base
{
    [SerializeField] private GameObject _explodeGameObject;
    public override bool Attack(Vector2Int gridPos)
    {
        bool outcome = true;
        Sound_Events.Play_Sound("Game_Bomb");
        outcome = Levelplay_Controller_Script.levelplay_controller_singleton.Find_Grid_Data(gridPos).resident.Attacked(_parentScript);
        _explodeGameObject.SetActive(true);
        _parentScript._bodyGO.SetActive(false);
        _parentScript.Change_Level_Actor_State(Level_Actor_States_Enum.Dead);
        return outcome;
    }
}
