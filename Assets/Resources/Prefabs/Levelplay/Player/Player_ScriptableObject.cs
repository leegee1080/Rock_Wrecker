using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Scriptable Objects/New Player")]
public class Player_ScriptableObject : ScriptableObject
{
    [field: SerializeField]public new string name{get;private set;}
    [field: SerializeField]public GameObject mainMesh{get;private set;}
    [field: SerializeField]public GameObject deathMesh{get;private set;}
}
