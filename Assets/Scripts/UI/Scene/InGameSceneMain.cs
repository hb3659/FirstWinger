using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    READY = 0,
    RUNNING,
    END,
}

public class InGameSceneMain : BaseSceneMain
{
    private const float GameReadyInterval = 3.0f;

    private GameState currentGameState = GameState.READY;
    public GameState CurrentGameState
    {
        get { return currentGameState; }
    }

    [SerializeField]
    private Player player;
    public Player Hero
    {
        get
        {
            if (!player)
                Debug.LogError("Main player is not set!");

            return player;
        }
        set { player = value; }
    }

    private GamePointAccumulator gamepointAcc = new GamePointAccumulator();
    public GamePointAccumulator GamePointAcc
    {
        get { return gamepointAcc; }
    }

    [SerializeField]
    private EffectManager effectMng;
    public EffectManager EffectMng
    {
        get { return effectMng; }
    }

    [SerializeField]
    private EnemyManager enemyMng;
    public EnemyManager EnemyMng
    {
        get { return enemyMng; }
    }

    [SerializeField]
    private BulletManager bulletMng;
    public BulletManager BulletMng
    {
        get { return bulletMng; }
    }

    [SerializeField]
    private DamageManager damageMng;
    public DamageManager DamageMng
    {
        get { return damageMng; }
    }

    private PrefabCacheSystem enemyCacheSystem = new PrefabCacheSystem();
    public PrefabCacheSystem EnemyCacheSystem
    {
        get { return enemyCacheSystem; }
    }

    private PrefabCacheSystem bulletCacheSystem = new PrefabCacheSystem();
    public PrefabCacheSystem BulletCacheSystem
    {
        get { return bulletCacheSystem; }
    }

    private PrefabCacheSystem effectCacheSystem = new PrefabCacheSystem();
    public PrefabCacheSystem EffectCacheSystem
    {
        get { return effectCacheSystem; }
    }

    private PrefabCacheSystem damageCacheSystem = new PrefabCacheSystem();
    public PrefabCacheSystem DamageCacheSystem
    {
        get { return damageCacheSystem; }
    }

    [SerializeField]
    private SquadronManager squadronMng;
    public SquadronManager SquadronMng
    {
        get { return squadronMng; }
    }

    private float sceneStartTime;

    [SerializeField]
    private Transform mainBGQuadTransform;
    public Transform MainBGQaudTransform
    {
        get { return mainBGQuadTransform; }
    }

    protected override void OnStart()
    {
        sceneStartTime = Time.time;
    }

    protected override void UpdateScene()
    {
        base.UpdateScene();

        float currentTime = Time.time;

        if(currentGameState == GameState.READY)
        {
            if(currentTime - sceneStartTime > GameReadyInterval)
            {
                SquadronMng.StartGame();
                currentGameState = GameState.RUNNING;
            }
        }
    }
}
