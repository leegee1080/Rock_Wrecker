using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCollectorCollider : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        Levelplay_Controller_Script.levelplay_controller_singleton.AddScore();
    }
}

