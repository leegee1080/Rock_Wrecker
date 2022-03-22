using UnityEngine;
using System.Collections.Generic;

public class LevelEnemy_Storage : MonoBehaviour
{
    public GameObject[] PossibleEnemies1;
    public GameObject[] PossibleEnemies2;
    public GameObject[] PossibleEnemies3;

    private List<GameObject[]> _arrayPossibleEnemyArrays = new List<GameObject[]>{};

    [SerializeField]private Transform _actorParentGO;

    public LevelEnemy_Script SpawnEnemy(Vector2Int newGridPos, int index)//set indext to less than 0 to be random
    {
         _arrayPossibleEnemyArrays.Add(PossibleEnemies1);
         _arrayPossibleEnemyArrays.Add(PossibleEnemies2);
         _arrayPossibleEnemyArrays.Add(PossibleEnemies3);

        GameObject[] selectedEnemiesArray = _arrayPossibleEnemyArrays[Overallgame_Controller_Script.overallgame_controller_singleton.selected_level.poi_difficulty -1];

        if(index < 0 || index >= selectedEnemiesArray.Length){index  = Global_Vars.rand_num_gen.Next(0, selectedEnemiesArray.Length);}

        GameObject newEnemy = Instantiate(selectedEnemiesArray[index],parent: _actorParentGO);
        newEnemy.GetComponent<LevelEnemy_Script>().Place_Resident(newGridPos);
        return newEnemy.GetComponent<LevelEnemy_Script>();
    }
}
