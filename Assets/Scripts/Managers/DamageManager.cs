using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public const int EnemyDamageIndex = 0;
    public const int PlayerDamageIndex = 0;

    [SerializeField]
    private Transform canvasTransform;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private PrefabCacheData[] files;

    Dictionary<string, GameObject> fileCache = new Dictionary<string, GameObject>();

    private void Start()
    {
        Prepare();
    }

    public GameObject DamageLoad(string _resourcePath)
    {
        GameObject go = null;

        if (fileCache.ContainsKey(_resourcePath))
            go = fileCache[_resourcePath];
        else
        {
            go = Resources.Load<GameObject>(_resourcePath);

            if (!go)
            {
                Debug.LogError("Load error! path = " + _resourcePath);
                return null;
            }

            fileCache.Add(_resourcePath, go);
        }

        return go;
    }

    public void Prepare()
    {
        for (int index = 0; index < files.Length; index++)
        {
            GameObject go = DamageLoad(files[index].filePath);
            SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().DamageCacheSystem.GenerateCache(files[index].filePath, go, files[index].cacheCount, canvasTransform);
        }
    }

    public GameObject Generate(int _index, Vector3 _position, int _damageValue, Color _color)
    {
        if(_index < 0 || _index >= files.Length)
        {
            Debug.LogError("Generate error! out of range! index = " + _index);
            return null;
        }

        string filePath = files[_index].filePath;
        GameObject go = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().DamageCacheSystem.Achieve(filePath);
        go.transform.position = Camera.main.WorldToScreenPoint(_position);

        UIDamage damage = go.GetComponent<UIDamage>();
        damage.FilePath = filePath;
        damage.ShowDamage(_damageValue, _color);

        return go;
    }

    public bool Remove(UIDamage _damage)
    {
        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().DamageCacheSystem.Restore(_damage.FilePath, _damage.gameObject);
        return true;
    }
}
