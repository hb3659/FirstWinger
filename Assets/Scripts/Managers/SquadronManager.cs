using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadronManager : MonoBehaviour
{
    [SerializeField]
    private SquadronTable[] squadronDatas;
    [SerializeField]
    private SquadronScheduleTable squadronScheduleTable;

    private float gameStartTime;
    private int squadronIndex;

    private bool running = false;

    private void Start()
    {
        squadronDatas = GetComponentsInChildren<SquadronTable>();
        for (int index = 0; index < squadronDatas.Length; index++)
            squadronDatas[index].Load();

        squadronScheduleTable.Load();
    }

    private void Update()
    {
        CheckSquadronGenerateting();
    }

    public void StartGame()
    {
        gameStartTime = Time.time;
        squadronIndex = 0;
        running = true;
        Debug.Log("Game start!");
    }

    public void CheckSquadronGenerateting()
    {
        if (!running)
            return;

        if (Time.time - gameStartTime >= squadronScheduleTable.GetScheduleData(squadronIndex).generateTime)
        {
            GenerateSquadron(squadronDatas[squadronIndex]);
            squadronIndex++;

            if(squadronIndex >= squadronDatas.Length)
            {
                AllSquadronGenerated();
                return;
            }
        }
    }

    public void GenerateSquadron(SquadronTable _table)
    {
        Debug.Log("GenerateSquadron");
        for (int index = 0; index < _table.GetCount(); index++)
        {
            SquadronMemberStruct squadronMember = _table.GetSquadronMember(index);
            SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EnemyMng.GenerateEnemy(squadronMember);
        }
    }

    public void AllSquadronGenerated()
    {
        Debug.Log("AllSquadronGenerated");

        running = false;
    }
}
