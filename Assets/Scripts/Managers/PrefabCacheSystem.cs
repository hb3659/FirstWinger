using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabCacheData
{
    public string filePath;
    public int cacheCount;
}

public class PrefabCacheSystem
{
    Dictionary<string, Queue<GameObject>> cacheDictionary = new Dictionary<string, Queue<GameObject>>();

    public void GenerateCache(string _filePath, GameObject _obj, int _count, Transform _parentTransform = null)
    {
        if (cacheDictionary.ContainsKey(_filePath))
        {
            Debug.LogError("Already cache generated! filePath = " + _filePath);
            return;
        }
        else
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            for (int index = 0; index < _count; index++)
            {
                GameObject go = Object.Instantiate<GameObject>(_obj, _parentTransform);
                go.SetActive(false);
                queue.Enqueue(go);
            }

            cacheDictionary.Add(_filePath, queue);
        }
    }

    public GameObject Achieve(string _filePath)
    {
        if(!cacheDictionary.ContainsKey(_filePath))
        {
            Debug.LogError("Achieve error! no cache generated! filePath = " + _filePath);
            return null;
        }

        if(cacheDictionary[_filePath].Count == 0)
        {
            Debug.LogError("Achieve problem! not enough count!");
            return null;
        }

        GameObject go = cacheDictionary[_filePath].Dequeue();
        go.SetActive(true);

        return go;
    }

    public bool Restore(string _filePath, GameObject obj)
    {
        if (!cacheDictionary.ContainsKey(_filePath))
        {
            Debug.LogError("Restore error! no cache generated! filePath = " + _filePath);
            return false;
        }

        obj.SetActive(false);

        cacheDictionary[_filePath].Enqueue(obj);
        return true;
    }
}
