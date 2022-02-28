using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropShip_Script : MonoBehaviour
{

    [SerializeField]private Animator animator_ani;
    [SerializeField]private GameObject dropship_mesh_go;
    [SerializeField]private ParticleSystem explosion_ps;
    [SerializeField]private ParticleSystem door_ps;
    public void Place_Dropship(Vector2Int location_to_place, Vector2Int location_to_face)
    {
        dropship_mesh_go.SetActive(true);
        
        gameObject.transform.position = Levelplay_Controller_Script.levelplay_controller_singleton.Find_Grid_Data(location_to_place).actual_pos;

        Vector2Int face_direction = location_to_face - location_to_place;

        if(face_direction == Vector2Int.up)
        {
            gameObject.transform.Rotate(new Vector3(0,0,180));
        }
        else if(face_direction == Vector2Int.down)
        {
            //do nothing
        }
        else if(face_direction == Vector2Int.left)
        {
            gameObject.transform.Rotate(new Vector3(0,0,270));
        }
        else if(face_direction == Vector2Int.right)
        {
            gameObject.transform.Rotate(new Vector3(0,0,90));
        }
    }

    public void Open_Door()
    {
        door_ps.Play();
    } 
    public void Crash()
    {
        StartCoroutine(Playerinput_Controller_Script.playerinput_controller_singleton.Shake_Camera(1f,0.1f));
        explosion_ps.Play();
    } 
    public void Launch()
    {
        animator_ani.SetTrigger("Launch");
    }
}
