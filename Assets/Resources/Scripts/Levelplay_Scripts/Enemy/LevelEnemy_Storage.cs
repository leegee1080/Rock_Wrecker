using UnityEngine;

public class LevelEnemy_Storage : MonoBehaviour
{
    public GameObject[] PossibleEnemies;

    [SerializeField]private Transform _actorParentGO;

    public LevelEnemy_Script SpawnEnemy(Vector2Int newGridPos, int index)
    {
        GameObject newEnemy = Instantiate
            (PossibleEnemies[index],
            Levelplay_Controller_Script.levelplay_controller_singleton.Find_Grid_Data(newGridPos).actual_pos,
            Quaternion.identity,
            _actorParentGO
            );
        newEnemy.transform.Rotate(new Vector3(-90,0,0));
        return newEnemy.GetComponent<LevelEnemy_Script>();
    }
}
