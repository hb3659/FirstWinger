using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatePanel : BasePanel
{
    [SerializeField]
    public Text scoreValue;
    [SerializeField]
    public Gauge hpGauge;

    public void SetScore(int _value)
    {
        Debug.Log("SetScore value : " + _value);
        scoreValue.text = _value.ToString();
    }

    public void SetHP(float _currentValue, float _maxValue)
    {
        hpGauge.SetHP(_currentValue, _maxValue);
    }
}
