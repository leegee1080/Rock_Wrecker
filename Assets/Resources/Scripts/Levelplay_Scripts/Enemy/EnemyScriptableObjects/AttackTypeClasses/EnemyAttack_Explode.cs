using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack_Explode : EnemyAttack_Base
{
    [SerializeField] private GameObject _explodeGameObject;
    public override void Attack(Vector2Int gridPos)
    {
        base.Attack(gridPos);
        _explodeGameObject.SetActive(true);
        _parentScript._bodyGO.SetActive(false);
        _parentScript.Change_Level_Actor_State(Level_Actor_States_Enum.Dead);
    }
}
