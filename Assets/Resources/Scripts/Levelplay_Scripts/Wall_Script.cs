using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Script : GridResident_Script
{

    [SerializeField]private Mesh[] wallmesh_list;
    [SerializeField]private Mesh wallmesh_small;
    [SerializeField]private Material transwall_mat;
    [SerializeField]private GameObject wallmesh_object;
    [SerializeField]private MeshFilter primary_filter;
    [SerializeField]private MeshRenderer primary_renderer;

    public override void Start()
    {
        base.Start();
        name = grid_pos.ToString();
        wallmesh_object.transform.Rotate(new Vector3(0,UnityEngine.Random.Range(0,90),0), Space.Self);
        primary_filter.mesh = wallmesh_list [UnityEngine.Random.Range(0, wallmesh_list.Length)];
        
    }

    public void CrushRock()
    {
        primary_filter.mesh = wallmesh_small;
    }
    public void FadeRock()
    {
        primary_renderer.material = transwall_mat;
    }
}
