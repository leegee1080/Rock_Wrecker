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
        iTween.ValueTo(uGuiElement.gameObject, iTween.Hash(
            "from", uGuiElement.anchoredPosition,
            "to", burstLocation,
            "speed", burstMagnitude,
            "onupdatetarget", gameObject, 
            "onupdate", "MoveGuiElement",
            "oncompletetarget", "BurstEnd"));

    }
    public void MoveGuiElement(Vector2 position)
    {
        uGuiElement.anchoredPosition = position;
    }

    public void BurstEnd()
    {
        iTween.ValueTo(uGuiElement.gameObject, iTween.Hash(
            "from", uGuiElement.anchoredPosition,
            "to", targetPosition,
            "delay", burstTime,
            "time", animationTime,
            "onupdatetarget", gameObject, 
            "onupdate", "MoveGuiElement"));
    }
}
