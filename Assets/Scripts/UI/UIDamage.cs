using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DamageState
{
    NONE = 0,
    SIZEUP,
    DISPLAY,
    FADEOUT,
}
public class UIDamage : MonoBehaviour
{
    [SerializeField]
    private DamageState damageState = DamageState.NONE;

    private const float SizeUpDuration = .1f;
    private const float DisplayDuration = .5f;
    private const float FadeOutDuration = .2f;

    [SerializeField]
    private Text damageText;

    private Vector3 currentVelocity;
    private float displayStartTime;
    private float FadeOutStartTime;

    public string FilePath
    {
        get;
        set;
    }

    private void Update()
    {
        UpdateDamage();
    }

    //private void OnGUI()
    //{
    //    if (GUILayout.Button("Show"))
    //        ShowDamage(9999);
    //}

    public void ShowDamage(int _damage, Color _color)
    {
        damageText.color = _color;
        damageText.text = _damage.ToString();
        Reset();
        damageState = DamageState.SIZEUP;
    }

    public void Reset()
    {
        transform.localScale = Vector3.zero;
        Color newColor = damageText.color;
        newColor.a = 1.0f;
        damageText.color = newColor;
    }

    public void UpdateDamage()
    {
        if (damageState == DamageState.NONE)
            return;

        switch (damageState)
        {
            case DamageState.SIZEUP:
                transform.localScale = Vector3.SmoothDamp(transform.localScale, Vector3.one, ref currentVelocity, SizeUpDuration);

                if(transform.localScale == Vector3.one)
                {
                    damageState = DamageState.DISPLAY;
                    displayStartTime = Time.time;
                }
                break;
            case DamageState.DISPLAY:
                if(Time.time - displayStartTime > DisplayDuration)
                {
                    damageState = DamageState.FADEOUT;
                    FadeOutStartTime = Time.time;
                }
                break;
            case DamageState.FADEOUT:
                Color newColor = damageText.color;
                newColor.a = Mathf.Lerp(1, 0, (Time.time - FadeOutStartTime) / FadeOutDuration);
                damageText.color = newColor;

                if(newColor.a == 0)
                {
                    damageState = DamageState.NONE;
                    //SystemManager.Instance.DamageManager.Remove(this);
                }
                break;
        }
    }
}
