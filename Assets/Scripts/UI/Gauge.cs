using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    public void SetHP(float _currentValue, float _MaxValue)
    {
        if (_currentValue > _MaxValue)
            _currentValue = _MaxValue;

        slider.value = _currentValue / _MaxValue;
    }
}
