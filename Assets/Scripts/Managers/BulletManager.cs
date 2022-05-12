using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public const int PlayerBulletIndex = 0;
    public const int EnemyBulletIndex = 1;

    [SerializeField]
    private PrefabCacheData[] bulletFiles;

    Dictionary<string, GameObject> fileCacheDictionary = new Dictionary<string, GameObject>();

    public void Start()
    {
        Prepare();
    }

    public GameObject BulletLoad(string _resourcePath)
    {
        GameObject go = null;

        // 캐시 확인
        if (fileCacheDictionary.ContainsKey(_resourcePath))
            go = fileCacheDictionary[_resourcePath];
        else
        {
            // 캐시에 없으므로 로드
            go = Resources.Load<GameObject>(_resourcePath);

            if (!go)
            {
                Debug.LogError("BulletLoad Error! path = " + _resourcePath);
                return null;
            }

            // 로드 후 캐시에 적재
            fileCacheDictionary.Add(_resourcePath, go);
        }

        return go;
    }

    public void Prepare()
    {
        for (int index = 0; index < bulletFiles.Length; index++)
        {
            GameObject go = BulletLoad(bulletFiles[index].filePath);
            SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().BulletCacheSystem.GenerateCache(bulletFiles[index].filePath, go, bulletFiles[index].cacheCount);
        }
    }

    public Bullet Generate(int _index)
    {
        string filePath = bulletFiles[_index].filePath;
        GameObject go = SystemManager.Instance.GetCurrentSceneMain
            <InGameSceneMain>().BulletCacheSystem.Achieve(filePath);

        Bullet bullet = go.GetComponent<Bullet>();
        bullet.FilePath = filePath;

        return bullet;
    }

    public bool Remove(Bullet bullet)
    {
        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().BulletCacheSystem.Restore(bullet.FilePath, bullet.gameObject);
        return true;
    }
}
