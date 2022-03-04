public class GenericObjectPooler<T> where T: new()
{
    public T[] objectArray; 
    public GenericObjectPooler(int _amountToPool)
    {
        objectArray = new T[_amountToPool];
        for (int i = 0; i < _amountToPool; i++)
        {
            objectArray[i] = new T();
        }
    }
}
