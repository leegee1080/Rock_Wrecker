using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Scriptable Objects/New Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    [field: SerializeField]public new string name{get;private set;}
    [field: SerializeField]public EnemyTypes enemyType{get;private set;}
    [field: SerializeField]public GameObject bodyGO{get;private set;}
    [field: SerializeField]public GameObject spawnParticleGO{get;private set;} 
    [field: SerializeField]public float decisionTime{get;private set;} 

}

