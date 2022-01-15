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
        name = poi_info_so.name;
    }

    public void Select_POI(){
        Levelselect_Controller_Script.levelselect_controller_singletion.Communicate_Selected_POI(this);
        Highlight_POI();
    }

    public void Highlight_POI(){
        transform.localScale = new Vector3(2,2,2);
    }

    public void Lowlight_POI(){
        transform.localScale = new Vector3(1,1,1);
    }
}
