using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBurstToTarget : MonoBehaviour
{
    public RectTransform uGuiElement;
    public Vector2 targetPosition;
    [SerializeField] private Vector2 burstLocation;
    public float animationTime;
    public float burstTime;
    public float burstMagnitude;
    void Start()
    {
        // iTween.ValueTo(uGuiElement.gameObject, iTween.Hash(
        //     "from", uGuiElement.anchoredPosition,
        //     "to", burstLocation,
        //     "time", burstMagnitude,
        //     "onupdatetarget", gameObject, 
        //     "onupdate", "MoveGuiElement",
        //     "easetype", iTween.EaseType.easeOutSine));

        // iTween.ValueTo(uGuiElement.gameObject, iTween.Hash(
        //     "from", burstLocation,
        //     "to", targetPosition,
        //     "delay", burstTime,
        //     "time", animationTime,
        //     "onupdatetarget", gameObject, 
        //     "onupdate", "MoveGuiElement"));

    }

    private void Update()
    {
        uGuiElement.transform.localPosition += new Vector3(burstMagnitude,0,0);
    }

    // public void MoveGuiElement(Vector2 position)
    // {
    //     uGuiElement.anchoredPosition = position;
    // }
}
