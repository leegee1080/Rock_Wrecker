using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Matchable Rock Secondary", menuName = "Scriptable Objects/New Matchable Rock Secondary")]
public class Secondary_Rock_ScriptableObject : ScriptableObject
{
    [field: SerializeField]public new string name{get;private set;} 
    [field: SerializeField]public int score_bonus{get;private set;}
    [field: SerializeField]public Secondary_Rock_Types_Enum secondary_type{get;private set;}
    [field: SerializeField]public Color main_color{get;private set;}
    [field: SerializeField]public Gradient main_color_glow{get;private set;}
    [field: SerializeField]public Mesh main_mesh{get;private set;}

}
