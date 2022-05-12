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

        // 캐시 확인
        if (enemyFileCache.ContainsKey(resourcePath))
        {
            go = enemyFileCache[resourcePath];
        }
        else
        {
            // 캐시에 없으므로 로드
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
