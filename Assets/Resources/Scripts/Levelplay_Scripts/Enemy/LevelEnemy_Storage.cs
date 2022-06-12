using UnityEngine;
using System.Collections.Generic;

public class LevelEnemy_Storage : MonoBehaviour
{
    public GameObject[] PossibleEnemies1;
    public GameObject[] PossibleEnemies2;
    public GameObject[] PossibleEnemies3;
    public GameObject[] PossibleEnemies4;
    public GameObject[] LateSpawnEnemies;

    private List<GameObject[]> _arrayPossibleEnemyArrays = new List<GameObject[]>{};

    [SerializeField]private Transform _actorParentGO;

    public LevelEnemy_Script SpawnEnemy(Vector2Int newGridPos, int index, bool ghostSpawn = false)//set index to less than 0 to be random
    {
        GameObject newEnemy;

        //select spawn based on level dificulty, do this check every time becuase I would like to change the level diff in game
         _arrayPossibleEnemyArrays.Add(PossibleEnemies1);
         _arrayPossibleEnemyArrays.Add(PossibleEnemies2);
         _arrayPossibleEnemyArrays.Add(PossibleEnemies3);
         _arrayPossibleEnemyArrays.Add(PossibleEnemies4);

        GameObject[] selectedEnemiesArray;
        GameObject selectedEnemyGo;

        //change the spawn list to a special list for late game
        if(LateSpawnEnemies != null && Levelplay_Controller_Script.levelplay_controller_singleton.CurrentLevelState == LevelStatesEnum.GetToEscape)
        {
            
            selectedEnemiesArray = LateSpawnEnemies;
            if(index < 0 || index >= selectedEnemiesArray.Length){index  = Global_Vars.rand_num_gen.Next(0, selectedEnemiesArray.Length);}
            selectedEnemyGo = selectedEnemiesArray[index];
            if(selectedEnemyGo != null)
            {
                Debug.Log("Spawned Ghost");

                newEnemy = Instantiate(selectedEnemiesArray[index],parent: _actorParentGO);
                newEnemy.GetComponent<LevelEnemy_Script>().Place_Resident(newGridPos);

                selectedEnemiesArray[index] = null;
                return newEnemy.GetComponent<LevelEnemy_Script>();
            }

        }
        selectedEnemiesArray = _arrayPossibleEnemyArrays[Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.poi_difficulty];

        if(index < 0 || index >= selectedEnemiesArray.Length){index  = Global_Vars.rand_num_gen.Next(0, selectedEnemiesArray.Length);}

        selectedEnemyGo = selectedEnemiesArray[index];
        if(selectedEnemyGo != null)
        {
            Debug.Log("Spawned Enemy");
            newEnemy = Instantiate(selectedEnemiesArray[index],parent: _actorParentGO);
            newEnemy.GetComponent<LevelEnemy_Script>().Place_Resident(newGridPos);
            return newEnemy.GetComponent<LevelEnemy_Script>();
        }
        return null;
    }
}
