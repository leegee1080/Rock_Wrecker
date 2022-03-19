using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack_Base :MonoBehaviour, Iattack, IcollectParent
{
    protected LevelEnemy_Script _parentScript;
    public virtual void Attack(Vector2Int gridPos)
    {
        Levelplay_Controller_Script.levelplay_controller_singleton.Find_Grid_Data(gridPos).resident.Attacked(_parentScript);
        return;
    }

    public virtual void GatherData()
    {
        _parentScript = GetComponent<LevelEnemy_Script>();
    }
}
