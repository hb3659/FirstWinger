using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePointAccumulator
{
    private int gamePoint = 0;

    public int GamePoint
    {
        get { return gamePoint; }
    }

    public void Accumulator(int _value)
    {
        gamePoint += _value;

        PlayerStatePanel playerStatePanel = PanelManager.GetPanel(typeof(PlayerStatePanel)) as PlayerStatePanel;
        playerStatePanel.SetScore(gamePoint);
    }

    public void Reset()
    {
        gamePoint = 0;
    }
}
