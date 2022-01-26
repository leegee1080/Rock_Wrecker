using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock_Script : GridResident_Script
{
    [field: SerializeField]public Rock_ScriptableObject primary_rock_type{get; private set;}

    [field: SerializeField]public Secondary_Rock_ScriptableObject secondary_rock_type{get; private set;}


    [SerializeField]private MeshRenderer primary_renderer;
    [SerializeField]private MeshRenderer secondary_renderer;

    [SerializeField]private bool initialized;

    public void Change_Rock_Types(Rock_ScriptableObject new_primary_rock_type, Secondary_Rock_ScriptableObject new_secondary_rock_type = null){
        if(initialized){Level_Events.Invoke_Board_Changed_Event();}
        primary_rock_type = new_primary_rock_type;
        secondary_rock_type = new_secondary_rock_type == null ? Levelplay_Controller_Script.levelplay_controller_singleton.default_secondary_rock_type: new_secondary_rock_type;
        Update_Primary_Rock_Type();
        Update_Secondary_Rock_Type();
        initialized = true;
    }

    public void Update_Primary_Rock_Type(){
        primary_renderer.material.color = primary_rock_type.main_color;
    }

    public void Update_Secondary_Rock_Type(){

    }
}
