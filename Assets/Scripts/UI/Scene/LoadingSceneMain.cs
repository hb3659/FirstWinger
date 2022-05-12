using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingSceneMain : BaseSceneMain
{
    private const float NextSceneInterval = 3.0f;
    private const float TextUpdateInterval = .15f;
    private const string LoadingTextValue = "Loading...";

    [SerializeField]
    public TextMeshProUGUI loadingText;

    private int textIndex = 0;
    private float lastUpdateTime;

    private float sceneStartTime;
    private bool nextSceneCall = false;

    protected override void OnStart()
    {
        sceneStartTime = Time.time;
    }

    protected override void UpdateScene()
    {
        base.UpdateScene();

        float currentTime = Time.time;
        if(currentTime - lastUpdateTime > TextUpdateInterval)
        {
            loadingText.text = LoadingTextValue.Substring(0, textIndex + 1);

            textIndex++;
            if (textIndex >= LoadingTextValue.Length)
                textIndex = 0;

            lastUpdateTime = currentTime;
        }

        if(currentTime - sceneStartTime > NextSceneInterval)
        {
            if (!nextSceneCall)
                GotoNextScene();
        }
    }

    public void GotoNextScene()
    {
        //SceneController.Instance.LoadScene(SceneNameConstants.InGame);
        FWNetworkManager.singleton.StartHost();
        nextSceneCall = true;
    }
}
