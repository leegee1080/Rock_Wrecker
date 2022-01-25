using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPOI_Script : MonoBehaviour
{
    [field: SerializeField]public MapPOI_ScriptableObject poi_info_so {get; private set;}
    [SerializeField]private MeshRenderer poi_renderer;



    public void Init(MapPOI_ScriptableObject new_poi_info_so){
        poi_info_so = new_poi_info_so;
        transform.position = new_poi_info_so.map_pos;
        transform.localScale *= new_poi_info_so.poi_size;
        name = poi_info_so.name;
        gameObject.transform.Rotate(new Vector3(0,0,poi_info_so.rotate_speed));
    }

    private void Update() {
        gameObject.transform.Rotate(Vector3.up * Time.deltaTime * poi_info_so.rotate_speed*10);
    }

    public void Select_POI(){
        Levelselect_Controller_Script.levelselect_controller_singletion.Communicate_Selected_POI(this);
    }
}
