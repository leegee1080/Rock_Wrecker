using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObject_Script : MonoBehaviour
{
    [SerializeField]Animator _ani;
    [SerializeField]GameObject _bot;

    public Vector3 TalkPos;
    public Vector3 HidePos;
    // Start is called before the first frame update
    void Start()
    {
        StartTutorial();
        _bot.transform.position = HidePos;
    }

    public void StartTutorial()
    {
        
    }
    public void StopTutorial()
    {

    }

    public void ShowTutorialScreen()
    {

    }
    public void HideTutorialScreen()
    {
        
    }
}
