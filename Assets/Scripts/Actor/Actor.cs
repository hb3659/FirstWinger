using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Actor : NetworkBehaviour
{
    [SerializeField]
    protected int maxHP = 100;
    [SerializeField]
    protected int currentHP;
    [SerializeField]
    protected int damage = 1;

    [SerializeField]
    protected int crashDamage = 100;
    [SerializeField]
    private bool isDead = false;

    public int CrashDamage
    {
        get { return crashDamage; }
    }

    public bool IsDead
    {
        get { return isDead; }
    }

    private void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        currentHP = maxHP;
    }

    private void Update()
    {
        UpdateActor();
    }

    protected virtual void UpdateActor()
    {

    }

    // 공격한 대상이 누구인지 알기 위함
    public virtual void OnBulletHit(Actor _attacker, int _damage, Vector3 _hitPosition)
    {
        Debug.Log("OnBulletHit damage : " + _damage);
        DecreseHP(_attacker, _damage, _hitPosition);
    }

    public virtual void OnCrash(Actor _attacker, int _damage, Vector3 _crashPosition)
    {
        Debug.Log("OnCrash attacker : " + _attacker.name + " damage : " + _damage);
        DecreseHP(_attacker, _damage, _crashPosition);
    }

    protected virtual void DecreseHP(Actor _attacker, int _value, Vector3 _damagePosition)
    {
        if (isDead)
            return;

        currentHP -= _value;

        if (currentHP < 0)
            currentHP = 0;

        if (currentHP == 0)
            OnDead(_attacker);
    }

    protected virtual void OnDead(Actor _killer)
    {
        Debug.Log(name + " OnDead");
        isDead = true;

        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EffectMng.GenerateEffect(EffectManager.ActorDeadFXIndex, transform.position);
    }
}
