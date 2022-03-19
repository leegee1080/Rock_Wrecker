using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeh_Base :MonoBehaviour, IBeh, IcollectParent
{
    protected LevelEnemy_Script _parentScript;
    public virtual void ProcessEnemyTurn()
    {
        return;
    }


    public virtual void GatherData()
    {
        _parentScript = GetComponent<LevelEnemy_Script>();
    }
}
