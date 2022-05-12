using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public const string enemyPath = "Prefabs/Enemy";

    Dictionary<string, GameObject> enemyFileCache = new Dictionary<string, GameObject>();

    public GameObject EnemyLoad(string resourcePath)
    {
        GameObject go = null;

        // ĳ�� Ȯ��
        if (enemyFileCache.ContainsKey(resourcePath))
        {
            go = enemyFileCache[resourcePath];
        }
        else
        {
            // ĳ�ÿ� �����Ƿ� �ε�
            go = Resources.Load<GameObject>(resourcePath);
            if(!go)
            {
                Debug.LogError("Load Error!");
                return null;
            }

            enemyFileCache.Add(resourcePath, go);
        }

        return go;

        //GameObject instantiatedGO = Instantiate<GameObject>(go);
        //return instantiatedGO;
    }
}
