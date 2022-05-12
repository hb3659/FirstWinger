using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNameConstants
{
    public static string TitleScene = "TitleScene";
    public static string LoadingScene = "LoadingScene";
    public static string InGame = "InGame";
}

public class SceneController : MonoBehaviour
{
    private static SceneController instance;

    public static SceneController Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = GameObject.Find("SceneController");

                if (go == null)
                {
                    // SceneController��� �̸��� ���� ������Ʈ ����
                    go = new GameObject("SceneController");

                    SceneController controller = go.AddComponent<SceneController>();
                    return controller;
                }
                else
                    instance = go.GetComponent<SceneController>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Can't have two instance of singleton");
            DestroyImmediate(this);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        // �� ��ȭ�� ���� �̺�Ʈ �޼ҵ带 ����
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnLoaded;
    }

    public void LoadScene(string _sceneName)
    {
        // LoadSceneMode.Single == ���� ���̾��Ű â�� �ϳ��� ����
        StartCoroutine(LoadSceneAsync(_sceneName, LoadSceneMode.Single));
    }

    public void LoadSceneAsync(string _sceneName)
    {
        // LoadSceneMode.Additive == ���� ���̾��Ű â�� �� �̻� ����
        StartCoroutine(LoadSceneAsync(_sceneName, LoadSceneMode.Additive));
    }

    private IEnumerator LoadSceneAsync(string _sceneName, LoadSceneMode _loadSceneMode)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_sceneName, _loadSceneMode);

        while (!asyncOperation.isDone)
            yield return null;

        Debug.Log("LoadSceneAsync is complete!");
    }

    public void OnActiveSceneChanged(Scene _scene0, Scene _scene1)
    {
        Debug.Log("OnActiveSceneChanged is called! scene0 = " + _scene0 + ", scene1 = " + _scene1);
    }

    public void OnSceneLoaded(Scene _scene, LoadSceneMode _loadSceneMode)
    {
        Debug.Log("OnSceneLoaded is called! scene = " + _scene.name + ", LoadSceneMode = " + _loadSceneMode);

        BaseSceneMain baseSceneMain = GameObject.FindObjectOfType<BaseSceneMain>();
        Debug.Log("OnSceneLoaded! baseSceneMain.name = " + baseSceneMain.name);
        SystemManager.Instance.CurrentSceneMain = baseSceneMain;
    }

    public void OnSceneUnLoaded(Scene _scene)
    {
        Debug.Log("OnSceneUnLoaded is called! scene = " + _scene.name);
    }
}
