using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public const int BulletDisappearFXIndex = 0;
    public const int ActorDeadFXIndex = 1;

    [SerializeField]
    private PrefabCacheData[] effectFiles;

    Dictionary<string, GameObject> fileCacheDictionary = new Dictionary<string, GameObject>();

    public void Start()
    {
        Prepare();
    }

    public GameObject GenerateEffect(int _index, Vector3 _position)
    {
        if (_index < 0 || _index >= effectFiles.Length)
        {
            Debug.LogError("GenerateEfffect error! out of range! index = " + _index);
            return null;
        }

        string filePath = effectFiles[_index].filePath;
        GameObject go = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EffectCacheSystem.Achieve(filePath);
        go.transform.position = _position;

        AutoCachableEffect effect = go.GetComponent<AutoCachableEffect>();
        effect.FilePath = filePath;

        return go;
    }

    public GameObject EffectLoad(string _resourcePath)
    {
        GameObject go = null;

        if (fileCacheDictionary.ContainsKey(_resourcePath))
            go = fileCacheDictionary[_resourcePath];
        else
        {
            go = Resources.Load<GameObject>(_resourcePath);
            if (!go)
            {
                Debug.LogError("Load error! path = " + _resourcePath);
                return null;
            }

            // 로드 후 캐시에 적재
            fileCacheDictionary.Add(_resourcePath, go);
        }

        return go;
    }

    public void Prepare()
    {
        for (int index = 0; index < effectFiles.Length; index++)
        {
            GameObject go = EffectLoad(effectFiles[index].filePath);
            SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EffectCacheSystem.GenerateCache(effectFiles[index].filePath, go, effectFiles[index].cacheCount);
        }
    }

    public bool RemoveEffect(AutoCachableEffect _effect)
    {
        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EffectCacheSystem.Restore(_effect.FilePath, _effect.gameObject);
        return true;
    }
}
