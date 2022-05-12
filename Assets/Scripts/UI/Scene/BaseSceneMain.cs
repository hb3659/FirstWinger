using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSceneMain : MonoBehaviour
{
    private void Awake()
    {
        OnAwake();
    }

    private void Start()
    {
        OnStart();
    }

    private void Update()
    {
        UpdateScene();
    }

    private void OnDestroy()
    {
        Terminate();
    }

    protected virtual void OnAwake()
    {

    }

    protected virtual void OnStart()
    {

    }

    // �ܺο��� �ʱ�ȭ ȣ��
    protected virtual void Initialize()
    {

    }

    protected virtual void UpdateScene()
    {

    }

    protected virtual void Terminate()
    {

    }
}
