using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack_Base :MonoBehaviour, Iattack, IcollectParent
{
    protected LevelEnemy_Script _parentScript;
    public void Attack()
    {
        return;
    }

    public void GatherData()
    {
        _parentScript = GetComponent<LevelEnemy_Script>();
    }
}
