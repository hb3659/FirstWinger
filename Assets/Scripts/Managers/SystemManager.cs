using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    private static SystemManager instance = null;

    public static SystemManager Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private EnemyTable enemyTb = new EnemyTable();

    public EnemyTable EnemyTb
    {
        get { return enemyTb; }
    }

    private BaseSceneMain currentSceneMain;
    public BaseSceneMain CurrentSceneMain
    {
        set { currentSceneMain = value; }
    }

    public void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("SystemManager error! Singleton error!");
            Destroy(gameObject);

            return;
        }

        instance = this;
        // ���� �Ѿ�� ������� ����
        DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        BaseSceneMain baseSceneMain = GameObject.FindObjectOfType<BaseSceneMain>();
        Debug.Log("OnSceneLoaded! baseSceneMain.name = " + baseSceneMain.name);
        SystemManager.Instance.CurrentSceneMain = baseSceneMain;
    }

    // BaseSceneMain�� ��ӹ��� Ŭ������ ��� ������
    public T GetCurrentSceneMain<T>() where T : BaseSceneMain
    {
        return currentSceneMain as T;
    }
}
