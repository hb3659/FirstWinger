using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    NONE = -1,  // 사용 전
    READY = 0,  // 준비 완료
    APPEAR,     // 등장
    BATTLE,     // 전투 중
    DEAD,       // 사망
    DISAPPEAR,  // 퇴장
}
public class Enemy : Actor
{
    [SerializeField]
    private EnemyState currentState = EnemyState.NONE;

    private const float maxSpeed = 10.0f;
    private const float maxSpeedTime = .5f;

    [SerializeField]
    private Vector3 targetPosition;
    [SerializeField]
    private float currentSpeed;
    private Vector3 currentVelocity;
    private float moveStartTime = 0.0f;

    [SerializeField]
    private Transform fireTransform;
    [SerializeField]
    private GameObject bulletObject;
    [SerializeField]
    private float bulletSpeed = 1f;

    private float lastBattleUpdateTime = 0.0f;
    [SerializeField]
    private int fireRemainCount = 1;
    public int gamePoint = 10;

    private Vector3 appearPoint;
    private Vector3 disappearPoint;
    private float lastActionUpdateTime;

    public string FilePath
    {
        get;
        set;
    }

    protected override void UpdateActor()
    {
        switch (currentState)
        {
            case EnemyState.READY:
                UpdateReady();
                break;
            case EnemyState.APPEAR:
                UpdateSpeed();
                UpdateMove();
                break;
            case EnemyState.BATTLE:
                UpdateBattle();
                break;
            case EnemyState.DEAD:
                break;
            case EnemyState.DISAPPEAR:
                UpdateSpeed();
                UpdateMove();
                break;
        }
    }

    public void UpdateSpeed()
    {
        currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, Time.time - moveStartTime / maxSpeedTime);
    }

    public void UpdateMove()
    {
        float distance = Vector3.Distance(targetPosition, transform.position);
        if (distance == 0)
        {
            Arrived();
            return;
        }
        currentVelocity = (targetPosition - transform.position).normalized * currentSpeed;

        // 속도 = 거리 / 시간
        // 시간 = 거리 / 속도
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, distance / currentSpeed, maxSpeed);
    }

    public void Reset(SquadronMemberStruct _data)
    {
        //maxHP = _data.maxHP;
        //currentHP = maxHP;
        //damage = _data.damage;
        //bulletSpeed = _data.bulletSpeed;
        //fireRemainCount = _data.fireRemainCount;
        //gamePoint = _data.gamePoint;

        //appearPoint = _data.appearPoint;
        //disappearPoint = _data.disappearPoint;

        //currentState = EnemyState.READY;
        //lastActionUpdateTime = Time.time;
        EnemyStruct enemyStruct = SystemManager.Instance.EnemyTb.GetEnemy(_data.enemyID);

        maxHP = enemyStruct.maxHP;
        currentHP = maxHP;
        damage = enemyStruct.damage;
        crashDamage = enemyStruct.crashDamage;
        bulletSpeed = enemyStruct.bulletSpeed;
        fireRemainCount = enemyStruct.fireRemainCount;
        gamePoint = enemyStruct.gamePoint;

        appearPoint = new Vector3(_data.appearPointX, _data.appearPointY, 0);
        disappearPoint = new Vector3(_data.disappearPointX, _data.disappearPointY, 0);

        currentState = EnemyState.READY;
        lastActionUpdateTime = Time.time;
    }

    public void Arrived()
    {
        currentSpeed = 0.0f;

        if (currentState == EnemyState.APPEAR)
        {
            currentState = EnemyState.BATTLE;
            lastActionUpdateTime = Time.time;
        }
        else //if (currentState == EnemyState.DISAPPEAR)
        {
            currentState = EnemyState.NONE;
            SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EnemyMng.RemoveEnemy(this);
        }
    }

    public void Appear(Vector3 _targetPos)
    {
        targetPosition = _targetPos;
        currentSpeed = maxSpeed;

        currentState = EnemyState.APPEAR;
        moveStartTime = Time.time;
    }

    public void Disappear(Vector3 _targetPos)
    {
        targetPosition = _targetPos;
        currentSpeed = maxSpeed;

        currentState = EnemyState.DISAPPEAR;
        moveStartTime = Time.time;
    }

    public void UpdateReady()
    {
        if (Time.time - lastActionUpdateTime > 1.0f)
            Appear(appearPoint);
    }

    public void UpdateBattle()
    {
        if (Time.time - lastBattleUpdateTime > 1.0f)
        {
            if (fireRemainCount > 0)
            {
                Fire();
                fireRemainCount--;
            }
            else
                Disappear(disappearPoint);

            lastBattleUpdateTime = Time.time;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();

        if (player)
        {
            if (!player.IsDead)
            {
                BoxCollider box = (BoxCollider)other;
                Vector3 crashPosition = player.transform.position + box.center;
                crashPosition.x += box.size.x * .5f;

                player.OnCrash(this, CrashDamage, crashPosition);
            }
        }
    }

    public void Fire()
    {
        Bullet bullet = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().BulletMng.Generate(BulletManager.EnemyBulletIndex);
        bullet.Fire(this, fireTransform.position, -fireTransform.right, bulletSpeed, damage);
    }

    protected override void DecreseHP(Actor _attacker, int _value, Vector3 _damagePosition)
    {
        base.DecreseHP(_attacker, _value, _damagePosition);

        Vector3 damagePoint = _damagePosition + Random.insideUnitSphere * .5f;
        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().DamageMng.Generate(DamageManager.EnemyDamageIndex, damagePoint, _value, Color.magenta);
    }

    protected override void OnDead(Actor _killer)
    {
        base.OnDead(_killer);

        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().GamePointAcc.Accumulator(gamePoint);
        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EnemyMng.RemoveEnemy(this);
        currentState = EnemyState.DEAD;
    }
}
