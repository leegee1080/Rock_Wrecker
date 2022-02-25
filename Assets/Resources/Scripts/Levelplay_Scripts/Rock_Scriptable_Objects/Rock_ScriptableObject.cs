using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Matchable Rock", menuName = "Scriptable Objects/New Matchable Rock")]
public class Rock_ScriptableObject : ScriptableObject
{
    [field: SerializeField]public new string name{get;private set;}
    [field: SerializeField]public Rock_Types_Enum rock_type{get;private set;}
    [field: SerializeField]public Color main_color{get;private set;}
    [field: SerializeField]public Mesh main_mesh{get;private set;}
}
