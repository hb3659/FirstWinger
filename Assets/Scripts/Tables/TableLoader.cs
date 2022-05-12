using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableLoader<TMarshalStruct> : MonoBehaviour
{
    [SerializeField]
    protected string filePath;

    private TableRecordParser<TMarshalStruct> tableRecordParser = new TableRecordParser<TMarshalStruct>();

    public bool Load()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(filePath);
        if (textAsset == null)
        {
            Debug.LogError("Load failed! filePath = " + filePath);
            return false;
        }

        ParseTable(textAsset.text);
        return true;
    }

    public void ParseTable(string _text)
    {
        StringReader reader = new StringReader(_text);

        string line = null;
        bool fieldRead = false;
        while((line = reader.ReadLine()) != null)
        {
            if (!fieldRead)
            {
                fieldRead = true;
                continue;
            }

            TMarshalStruct data = tableRecordParser.ParserRecordLine(line);
            AddData(data);
        }
    }

    protected virtual void AddData(TMarshalStruct _data)
    {

    }
}
