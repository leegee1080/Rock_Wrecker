using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Floor_Script : MonoBehaviour
{
    public List<Vector3> newVertices = new List<Vector3>();

    public List<int> newTriangles = new List<int>();

    public List<Vector2> newUV = new List<Vector2>();


    private Mesh mesh;

    public void Build_Floor_Mesh()
    {
    mesh = GetComponent<MeshFilter> ().mesh;

    float x = transform.position.x;
    float y = transform.position.y;
    float z = transform.position.z;

    // IEnumerable<Vector3> sorted_list =
    //     from vect in Levelplay_Controller_Script.levelplay_controller_singleton.wall_coord_list
    //     orderby vect //"ascending" is default
    //     select vect;

    List<Vector3> sorted_vect_list = new List<Vector3>(Levelplay_Controller_Script.levelplay_controller_singleton.wall_coord_list.OrderBy(vect => vect.x));

    foreach (Vector3 coord in sorted_vect_list)
    {
        newVertices.Add(coord);
    }


    newTriangles.Add(0);
    newTriangles.Add(1);
    newTriangles.Add(2);
    newTriangles.Add(2);
    newTriangles.Add(1);
    newTriangles.Add(3);

    mesh.Clear ();
    mesh.vertices = newVertices.ToArray();
    mesh.triangles = newTriangles.ToArray();
    // mesh.uv = newUV.ToArray();
    mesh.Optimize ();
    mesh.RecalculateNormals ();
    }
}
