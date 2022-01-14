using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPOI_Script : MonoBehaviour
{

    [SerializeField]private MapPOI_ScriptableObject poi_info_so;

    public void Init(MapPOI_ScriptableObject new_poi_info_so){
        poi_info_so = new_poi_info_so;
        transform.position = new_poi_info_so.map_pos;
        transform.localScale *= new_poi_info_so.poi_size;
    }
}
