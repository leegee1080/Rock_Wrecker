using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCollectorCollider : MonoBehaviour
{
    [SerializeField]private string[] _scoreSounds;
    private int _soundIndex;

    private void Start()
    {
        _soundIndex = _scoreSounds.Length-1;
    }
    private void OnParticleCollision(GameObject other)
    {
        Levelplay_Controller_Script.levelplay_controller_singleton.AddScore();
        Sound_Events.Play_Sound(_scoreSounds[_soundIndex]);
        _soundIndex -= 1;
        if(_soundIndex <= 0){_soundIndex= _scoreSounds.Length-1;}
    }
}

