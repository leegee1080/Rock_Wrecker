using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum scene_enums{
    loading,
    mainmenu,
    levelselect,
    levelplay,
    endcredits
}

public class Overallgame_Controller_Script : MonoBehaviour
{
    public static Overallgame_Controller_Script overallgame_controller_singleton;

    [Header("Global Player Stats")]
    public int player_score;
    public GameObject player_model;
    public List<MapPOI_ScriptableObject> main_map = new List<MapPOI_ScriptableObject>();



    [Header("Scene Loading")]
    public scene_enums chosen_scene_enum;

    private void Awake()
    {
        if (overallgame_controller_singleton == null)
        {
            overallgame_controller_singleton = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this);
    }
}
