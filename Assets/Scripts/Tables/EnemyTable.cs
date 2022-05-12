using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct EnemyStruct
{
    public int index;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MarshalTableConstant.charBufferSize)]
    public string filePath;
    public int maxHP;
    public int damage;
    public int crashDamage;
    public int bulletSpeed;
    public int fireRemainCount;
    public int gamePoint;
}

public class EnemyTable : TableLoader<EnemyStruct>
{
    Dictionary<int, EnemyStruct> tableDatas = new Dictionary<int, EnemyStruct>();

    public void Start()
    {
        Load();
    }

    protected override void AddData(EnemyStruct _data)
    {
        base.AddData(_data);

        Debug.Log("data.index = " + _data.index);
        Debug.Log("data.filePath = " + _data.filePath);
        Debug.Log("data.maxHP = " + _data.maxHP);
        Debug.Log("data.damage = " + _data.damage);
        Debug.Log("data.crashDamage = " + _data.crashDamage);
        Debug.Log("data.bulletSpeed= " + _data.bulletSpeed);
        Debug.Log("data.fireRemainCount = " + _data.fireRemainCount);
        Debug.Log("data.gamePoint = " + _data.gamePoint);

        tableDatas.Add(_data.index, _data);
    }

    public EnemyStruct GetEnemy(int _index)
    {
        if (!tableDatas.ContainsKey(_index))
        {
            Debug.LogError("GetEnemy error! index = " + _index);
            return default(EnemyStruct);
        }

        return tableDatas[_index];
    }
}
