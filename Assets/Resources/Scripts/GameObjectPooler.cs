using UnityEngine;

public class GameObjectPooler
{
    public GameObject[] gameObjectArray;
    private int index = 0;
    public GameObjectPooler(int _amountToPool,GameObject _original, GameObject _parent)
    {
        gameObjectArray = new GameObject[_amountToPool];
        for (int i = 0; i < _amountToPool; i++)
        {
            gameObjectArray[i] = GameObject.Instantiate(_original, parent: _parent.transform);
        }
    }

    public GameObject CallNext(int _forcedIndex = default)
    {
        if(_forcedIndex != default){index = _forcedIndex;}

        GameObject _chosenGameObject = gameObjectArray[index];
        index = index == gameObjectArray.Length -1 ? 0: index + 1;
        return _chosenGameObject;
    }
}
