using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BackgroundScrollData
{
    public Renderer renderForScroll;
    public float speed;
    public float offsetX;
}

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField]
    public BackgroundScrollData[] scrollDatas;

    public void Update()
    {
        UpdateScroll();
    }

    public void UpdateScroll()
    {
        for (int index = 0; index < scrollDatas.Length; index++)
        {
            SetTextureOffset(scrollDatas[index]);
        }
    }

    public void SetTextureOffset(BackgroundScrollData scrollData)
    {
        scrollData.offsetX += (float)(scrollData.speed * Time.deltaTime);
        if (scrollData.offsetX > 1)
            scrollData.offsetX = scrollData.offsetX % 1.0f;

        Vector2 offSet = new Vector2(scrollData.offsetX, 0);
        scrollData.renderForScroll.material.SetTextureOffset("_MainTex", offSet);
    }
}
