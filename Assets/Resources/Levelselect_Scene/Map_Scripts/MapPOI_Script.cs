using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPOI_Script : MonoBehaviour
{

    private MapPOI_ScriptableObject poi_info_so;

    public void Init(MapPOI_ScriptableObject new_poi_info_so){
        poi_info_so = new_poi_info_so;
    }
}
