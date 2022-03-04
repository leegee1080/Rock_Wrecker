using System.Collections;
using UnityEngine;

public class PoolableGameObject : MonoBehaviour
{
    [SerializeField]private float _timerAmount;
    [SerializeField]private Vector3 _homeLocation;

    private void OnEnable() {
        StartCoroutine(ReloadPooledObject());
    }

    private IEnumerator ReloadPooledObject()
    {
        gameObject.transform.position = _homeLocation;
        if(_timerAmount == -1){StopCoroutine(ReloadPooledObject());}
        yield return new WaitForSecondsRealtime(_timerAmount);
        gameObject.SetActive(false);
    }
}
