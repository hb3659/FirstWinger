using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    private static Dictionary<Type, BasePanel> PanelDictionary = new Dictionary<Type, BasePanel>();

    public static bool RegistPanel(Type _panelClassType, BasePanel _basePanel)
    {
        if (PanelDictionary.ContainsKey(_panelClassType))
        {
            Debug.LogError("RegistPanel error! already exist type! PanelClassType = " + _panelClassType);
            return false;
        }

        Debug.Log("RegistPanel is called! Type = " + _panelClassType);

        PanelDictionary.Add(_panelClassType, _basePanel);
        return true;
    }

    public static bool UnregistPanel(Type _panelClassType)
    {
        if (!PanelDictionary.ContainsKey(_panelClassType))
        {
            Debug.LogError("UnregistPanel error! Can't find type! PanelClassType = " + _panelClassType.ToString());
            return false;
        }

        PanelDictionary.Remove(_panelClassType);
        return true;
    }

    public static BasePanel GetPanel(Type _panelClassType)
    {
        if (!PanelDictionary.ContainsKey(_panelClassType))
        {
            Debug.LogError("GetPanel error! Can't find type! PanelClassType = " + _panelClassType.ToString());
            return null;
        }

        return PanelDictionary[_panelClassType];
    }
}
