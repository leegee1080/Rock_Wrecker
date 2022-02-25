using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreItem_Script : MonoBehaviour
{
    [SerializeField]private TMP_Text text_object;
    [SerializeField]private Secondary_Rock_Types_Enum my_score_type;

    public void Update_My_Score(int[] new_score_array)
    {
        text_object.text = new_score_array[(int)my_score_type] + "";
    }
}
