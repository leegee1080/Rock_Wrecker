using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scene_Enums{
    loading,
    mainmenu,
    levelselect,
    levelplay,
    endcredits
}

public class Loading_Controller_Script : MonoBehaviour
{
    public static  Loading_Controller_Script loading_controller_singleton;


    private void Awake()
    {
        if (loading_controller_singleton == null)
        {
            loading_controller_singleton = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this);
    }

    public void Load_Next_Scene(Scene_Enums next_scene = default)
    {
        if(next_scene != default){Overallgame_Controller_Script.overallgame_controller_singleton.chosen_scene_enum = next_scene;}

        //check to make this script doesn't get stuck on the loading scene
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
        {
            StartCoroutine(ILoad_Next_Scene((int)Overallgame_Controller_Script.overallgame_controller_singleton.chosen_scene_enum));
            return;
        }
        StartCoroutine(ILoad_Next_Scene(0));
    }

    private IEnumerator ILoad_Next_Scene(int next_scene)
    {

        AsyncOperation async_scene_load = SceneManager.LoadSceneAsync(next_scene);
        
        while (!async_scene_load.isDone)
        {
            yield return null;
        }
        Debug.Log("scene load done");
    }
}
