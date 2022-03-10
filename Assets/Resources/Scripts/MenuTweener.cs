using UnityEngine;

public class MenuTweener : MonoBehaviour
{
    [SerializeField]private Vector3 originLocation;
    [SerializeField]private GameObject objectToMove;
    [SerializeField]private Vector3 moveToLocation;
    [SerializeField]private float moveToTime;


    // void Start()
    // {
        
    // }
    private void OnEnable()
    {
        gameObject.transform.position = originLocation;
        iTween.MoveTo(objectToMove,moveToLocation,moveToTime);
    }
}
