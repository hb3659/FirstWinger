using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : Actor
{
    [SerializeField]
    private NetworkIdentity networkIdentity = null;
    [SerializeField]
    [SyncVar]
    private Vector3 moveVector = Vector3.zero;
    [SerializeField]
    private float speed;
    [SerializeField]
    private BoxCollider boxCollider;
    [SerializeField]
    private Transform fireTransform;
    [SerializeField]
    private GameObject bulletObject;
    [SerializeField]
    private float bulletSpeed;

    protected override void Initialize()
    {
        PlayerStatePanel playerStatePanel = PanelManager.GetPanel(typeof(PlayerStatePanel)) as PlayerStatePanel;
        playerStatePanel.SetHP(currentHP, maxHP);

        if (isLocalPlayer)
            SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().Hero = this;
    }

    protected override void UpdateActor()
    {
        UpdateMove();
    }

    public void UpdateMove()
    {
        if (moveVector.sqrMagnitude == 0)
            return;

        moveVector = AdjustVector(moveVector);
        //transform.position += moveVector;
        CmdMove(moveVector);
    }

    [Command]
    public void CmdMove(Vector3 _moveVector)
    {
        this.moveVector = _moveVector;
        transform.position += moveVector;
        base.SetDirtyBit(1);
    }

    public void ProcessInput(Vector3 _moveDirection)
    {
        moveVector = _moveDirection * speed * Time.deltaTime;
    }

    public Vector3 AdjustVector(Vector3 _moveVector)
    {
        Transform mainBGQuadTransform = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().MainBGQaudTransform;
        Vector3 result = Vector3.zero;
        result = boxCollider.transform.position + boxCollider.center + moveVector;

        if (result.x - boxCollider.size.x * .5f < -mainBGQuadTransform.localScale.x * .5f)
            _moveVector.x = 0;
        if (result.x + boxCollider.size.x * .5f > mainBGQuadTransform.localScale.x * .5f)
            _moveVector.x = 0;
        if (result.y - boxCollider.size.y * .5f < -mainBGQuadTransform.localScale.y * .5f)
            _moveVector.y = 0;
        if (result.y + boxCollider.size.y * .5f > mainBGQuadTransform.localScale.y * .5f)
            _moveVector.y = 0;

        return _moveVector;
    }

    public void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();

        if (enemy)
        {
            if (!enemy.IsDead)
            {
                BoxCollider box = (BoxCollider)other;
                Vector3 crashPosition = enemy.transform.position + box.center;
                crashPosition.x += box.size.x * .5f;

                enemy.OnCrash(this, CrashDamage, crashPosition);
            }
        }
    }

    public void Fire()
    {
        Bullet bullet = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().BulletMng.Generate(BulletManager.PlayerBulletIndex);
        bullet.Fire(this, fireTransform.position, fireTransform.right, bulletSpeed, damage);
    }

    protected override void DecreseHP(Actor _attacker, int _value, Vector3 _damagePosition)
    {
        base.DecreseHP(_attacker, _value, _damagePosition);

        PlayerStatePanel playerStatePanel = PanelManager.GetPanel(typeof(PlayerStatePanel)) as PlayerStatePanel;
        playerStatePanel.SetHP(currentHP, maxHP);

        Vector3 damagePoint = _damagePosition + Random.insideUnitSphere * .5f;
        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().DamageMng.Generate(DamageManager.PlayerDamageIndex, damagePoint, _value, Color.red);
    }

    protected override void OnDead(Actor _killer)
    {
        base.OnDead(_killer);
        gameObject.SetActive(false);
    }


    // ฤÝน้
    //public System.Action<GameObject> objectAction;

    //public void SetAction(System.Action<GameObject> _action)
    //{
    //    if (objectAction != null)
    //        objectAction = null;


    //    objectAction = _action;
    //}

    //public void OnCallBackObjectAction()
    //{
    //    if (objectAction != null)
    //        objectAction(this.gameObject);
    //}
}
