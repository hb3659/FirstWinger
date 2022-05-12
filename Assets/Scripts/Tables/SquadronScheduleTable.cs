using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct SquadronScheduleDataStruct
{
    public int index;
    public float generateTime;
    public int squadronID;
}
public class SquadronScheduleTable : TableLoader<SquadronScheduleDataStruct>
{
    List<SquadronScheduleDataStruct> tableDatas = new List<SquadronScheduleDataStruct>();

    protected override void AddData(SquadronScheduleDataStruct _data)
    {
        base.AddData(_data);
        tableDatas.Add(_data);
    }

    public SquadronScheduleDataStruct GetScheduleData(int _index)
    {
        if(_index < 0 || _index >= tableDatas.Count)
        {
            Debug.LogError("SquadronScheduleDataStruct error! index = " + _index);
            return default(SquadronScheduleDataStruct);
        }

        return tableDatas[_index];
    }
}
