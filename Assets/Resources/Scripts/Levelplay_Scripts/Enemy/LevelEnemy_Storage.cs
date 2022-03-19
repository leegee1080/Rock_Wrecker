using UnityEngine;

public class LevelEnemy_Storage : MonoBehaviour
{
    public GameObject[] PossibleEnemies;

    [SerializeField]private Transform _actorParentGO;

    public LevelEnemy_Script SpawnEnemy(Vector2Int newGridPos, int index)
    {
        GameObject newEnemy = Instantiate(PossibleEnemies[index],parent: _actorParentGO);
        newEnemy.GetComponent<LevelEnemy_Script>().Place_Resident(newGridPos);
        return newEnemy.GetComponent<LevelEnemy_Script>();
    }
}
