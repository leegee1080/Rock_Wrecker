using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullableObject_Script : MonoBehaviour
{
    [SerializeField]private float _cameraDistThreshold;
    [SerializeField]private GameObject[] _objectsToCull;
    [SerializeField]private bool _culled;
    [SerializeField]private float _cameraDist;
    void Start()
    {
        Input_Control_Events.cameraViewEvent += UpdateCull;
    }

    void UpdateCull(Vector3 cameraPos)
    {
        _cameraDist = Vector3.Distance(this.gameObject.transform.position,cameraPos);

        if(_cameraDist > _cameraDistThreshold && !_culled)
        {
            foreach (GameObject item in _objectsToCull)
            {
                item.SetActive(false);
                _culled = true;
            }
            return;
        }

        if(_cameraDist <= _cameraDistThreshold && _culled)
        {
            foreach (GameObject item in _objectsToCull)
            {
                item.SetActive(true);
                _culled = false;
            }
            return;
        }
    }

    private void OnDestroy()
    {
        Input_Control_Events.cameraViewEvent -= UpdateCull;
    }
}
