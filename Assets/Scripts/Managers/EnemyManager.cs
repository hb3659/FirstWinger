using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private PrefabCacheData[] enemyFiles;
    [SerializeField]
    private EnemyFactory enemyFactory;

    private List<Enemy> enemies = new List<Enemy>();

    public List<Enemy> Enemies
    {
        get { return enemies; }
    }

    public void Start()
    {
        Prepare();
    }

    public bool GenerateEnemy(SquadronMemberStruct _data)
    {
        string filePath = SystemManager.Instance.EnemyTb.GetEnemy(_data.enemyID).filePath;
        GameObject go = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EnemyCacheSystem.Achieve(filePath);
        go.transform.position = new Vector3(_data.generatePointX, _data.generatePointY, 0);

        Enemy enemy = go.GetComponent<Enemy>();
        enemy.FilePath = filePath;

        enemy.Reset(_data);
        enemies.Add(enemy);

        return true;
    }

    public bool RemoveEnemy(Enemy _enemy)
    {
        if (!enemies.Contains(_enemy))
        {
            Debug.LogError("No exist enemy");
            return false;
        }

        enemies.Remove(_enemy);
        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EnemyCacheSystem.Restore(_enemy.FilePath, _enemy.gameObject);

        return true;
    }

    public void Prepare()
    {
        for (int index = 0; index < enemyFiles.Length; index++)
        {
            GameObject go = enemyFactory.EnemyLoad(enemyFiles[index].filePath);
            SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EnemyCacheSystem.GenerateCache(enemyFiles[index].filePath, go, enemyFiles[index].cacheCount);
        }
    }
}
