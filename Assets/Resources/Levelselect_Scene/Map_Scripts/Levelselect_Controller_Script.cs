using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levelselect_Controller_Script : MonoBehaviour
{
    public static Levelselect_Controller_Script levelselect_controller_singletion;

    private List<MapPOI_ScriptableObject> map_poi_list {get;}
    [SerializeField] private MapPOI_Script map_poi_go;

    void Awake() => levelselect_controller_singletion = this;

    private void Start(){

        if(map_poi_list == null){return;}
        foreach(MapPOI_ScriptableObject map_poi_so in map_poi_list){
            MapPOI_Script new_poi = Instantiate(map_poi_go);
        }
    }
}
