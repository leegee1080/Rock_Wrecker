using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock_Types_Storage_Script : MonoBehaviour
{
    public static Rock_Types_Storage_Script rock_types_controller_singleton;



    public List<Rock_Types_Enum> rock_enum_list;
    public List<Secondary_Rock_Types_Enum> secondary_enum_list;
    public List<Rock_ScriptableObject> rock_so_list;
    public List<Secondary_Rock_ScriptableObject> secondary_so_list;

    public Dictionary<Rock_Types_Enum,Rock_ScriptableObject> primary_rock_type_dict {get; private set;}
    public Dictionary<Secondary_Rock_Types_Enum,Secondary_Rock_ScriptableObject> secondary_rock_type_dict {get; private set;}

    void Awake()
    {
        rock_types_controller_singleton = this;
        primary_rock_type_dict = new Dictionary<Rock_Types_Enum, Rock_ScriptableObject>();
        secondary_rock_type_dict = new Dictionary<Secondary_Rock_Types_Enum, Secondary_Rock_ScriptableObject>();

        int list_int = 0;
        foreach (Rock_Types_Enum type in rock_enum_list)
        {
            primary_rock_type_dict[type] = rock_so_list[list_int];
            list_int += 1;
        }
        list_int = 0;
        foreach (Secondary_Rock_Types_Enum type in secondary_enum_list)
        {
            secondary_rock_type_dict[type] = secondary_so_list[list_int];
            list_int += 1;
        }
    }

    // void Start()
    // {
    //     Global_Vars.Print_Map_Dict<Rock_Types_Enum, Rock_ScriptableObject>(primary_rock_type_dict);
    //     print(" ");
    //     print(" ");
    //     print(" ");
    //     Global_Vars.Print_Map_Dict<Secondary_Rock_Types_Enum, Secondary_Rock_ScriptableObject>(secondary_rock_type_dict);
    // }
}
