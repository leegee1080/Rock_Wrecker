using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPOI_Script : MonoBehaviour
{
    [field: SerializeField]public MapPOI_ScriptableObject poi_info_so {get; private set;}
    [SerializeField]private GameObject _middlePoint;
    [SerializeField]private SphereCollider _sphereCollider;

    [SerializeField]private MeshRenderer poi_mesh_renderer;
    [SerializeField]private MeshFilter poi_mesh_filter;
    [SerializeField]private Mesh[] asteroid_meshs;
    [SerializeField]private GameObject[] _decoMeshes;
    [SerializeField]private float _decoDistFromCenter;



    public void Init(MapPOI_ScriptableObject new_poi_info_so)
    {
        poi_info_so = new_poi_info_so;
        poi_mesh_filter.mesh = asteroid_meshs[new_poi_info_so.mesh_index];
        transform.position = new_poi_info_so.map_pos;
        poi_mesh_renderer.transform.localScale *= new_poi_info_so.poi_size * 0.8f;
        _sphereCollider.radius *= new_poi_info_so.poi_size * 0.8f;
        name = poi_info_so.name;
        gameObject.transform.Rotate(new Vector3(0,0,poi_info_so.rotate_speed));
        Spawn_Deco();
    }

    private void Update()
    {
        gameObject.transform.Rotate(Vector3.up * Time.deltaTime * poi_info_so.rotate_speed*10);
    }

    private void Spawn_Deco()
    {
        Random.InitState(poi_info_so.level_seed);
        for (int i = 0; i < poi_info_so.deco_count; i++)
        {
            GameObject _newDeco = Instantiate(_decoMeshes[(int)Random.Range(0, _decoMeshes.Length-1)], parent: this.transform);
            _newDeco.transform.localPosition = new Vector3
            (
                Random.Range(-_decoDistFromCenter* poi_info_so.poi_size,_decoDistFromCenter* poi_info_so.poi_size),
                Random.Range(-_decoDistFromCenter* poi_info_so.poi_size,_decoDistFromCenter* poi_info_so.poi_size),
                Random.Range(-_decoDistFromCenter* poi_info_so.poi_size,_decoDistFromCenter* poi_info_so.poi_size)
            );
            _newDeco.transform.LookAt(_middlePoint.transform.position, Vector3.down);
        }
    }

    public void Select_POI()
    {
        Levelselect_Controller_Script.levelselect_controller_singletion.Communicate_Selected_POI(this);
    }
}
