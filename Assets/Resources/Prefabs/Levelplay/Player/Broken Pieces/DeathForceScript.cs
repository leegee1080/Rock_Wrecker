using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathForceScript : MonoBehaviour
{
    public float radius = 5.0F;
    public float power = 10.0F;
    private void Awake()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        StartCoroutine(Playerinput_Controller_Script.playerinput_controller_singleton.Shake_Camera(1.5f,0.08f));

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
            }
        }
    }
}
