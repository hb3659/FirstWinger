using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSceneMain : BaseSceneMain
{
    public Button startButton;

    public void Awake()
    {
        ButtonListener();   
    }

    public void ButtonListener()
    {
        startButton.onClick.AddListener(OnStartButton);
    }

    public void OnStartButton()
    {
        Debug.Log("OnStartButton");

        SceneController.Instance.LoadScene(SceneNameConstants.LoadingScene);
        //SceneController.Instance.LoadSceneAsync(SceneNameConstants.LoadingScene);
    }
}
