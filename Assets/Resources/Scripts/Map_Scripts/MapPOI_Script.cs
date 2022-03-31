using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPOI_Script : MonoBehaviour
{
    [field: SerializeField]public MapPOI_ScriptableObject poi_info_so {get; private set;}
    [SerializeField]private MeshRenderer poi_mesh_renderer;
    [SerializeField]private MeshFilter poi_mesh_filter;
    [SerializeField]private Mesh[] asteroid_meshs;
    [SerializeField]private GameObject[] _decoMeshes;
    [SerializeField]private float _decoDistFromCenter;



    public void Init(MapPOI_ScriptableObject new_poi_info_so)
    {
        poi_info_so = new_poi_info_so;
        Spawn_Deco();
        poi_mesh_filter.mesh = asteroid_meshs[new_poi_info_so.mesh_index];
        transform.position = new_poi_info_so.map_pos;
        transform.localScale *= new_poi_info_so.poi_size;
        name = poi_info_so.name;
        gameObject.transform.Rotate(new Vector3(0,0,poi_info_so.rotate_speed));
    }

    private void Update()
    {
        gameObject.transform.Rotate(Vector3.up * Time.deltaTime * poi_info_so.rotate_speed*10);
    }

    private void Spawn_Deco()
    {
        for (int i = 0; i < poi_info_so.deco_count; i++)
        {
            GameObject _newDeco = Instantiate(_decoMeshes[Global_Vars.rand_num_gen.Next(0, _decoMeshes.Length)], parent: this.transform);
        }
    }

    public void Select_POI()
    {
        Levelselect_Controller_Script.levelselect_controller_singletion.Communicate_Selected_POI(this);
    }
}
