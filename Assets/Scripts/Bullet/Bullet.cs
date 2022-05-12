using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const float LIFETIME = 15.0f;       // 총알의 생존 시간

    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;
    [SerializeField]
    private float speed = 0f;
    private float firedTime;
    private bool needMove = false;      // 이동 플래그
    private bool hit = false;           // 부딪혔는지 알기위한 플래그
    private Actor owner;

    [SerializeField]
    private int damage = 1;

    public string FilePath
    {
        get;
        set;
    }

    private void Update()
    {
        if (ProcessDisappearCondition())
            return;

        UpdateMove();
    }

    public void UpdateMove()
    {
        if (needMove)
        {
            Vector3 moveVector = moveDirection.normalized * speed * Time.deltaTime;
            moveVector = AdjustMove(moveVector);
            transform.position += moveVector;
        }
    }

    // 발사 주체, 발사 지점, 발사 방향, 발사 속도
    public void Fire(Actor _owner, Vector3 _firePosition, Vector3 _direction, float _speed, int _damage)
    {
        owner = _owner;
        transform.position = _firePosition;
        moveDirection = _direction;
        speed = _speed;
        damage = _damage;

        needMove = true;
        firedTime = Time.time;
    }

    public Vector3 AdjustMove(Vector3 _moveVector)
    {
        // 시작과 끝이 존재하는 선
        // 레이캐스트는 끝이 없는 무한한 선을 그림
        RaycastHit hitInfo;
        if (Physics.Linecast(transform.position, transform.position + _moveVector, out hitInfo))
        {
            _moveVector = hitInfo.point - transform.position;
            OnBulletCollision(hitInfo.collider);
        }

        return _moveVector;
    }

    public void OnBulletCollision(Collider collider)
    {
        if (hit)
            return;

        if (collider.gameObject.layer == LayerMask.NameToLayer("EnemyBullet")
            || collider.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
            return;

        Actor actor = collider.GetComponentInParent<Actor>();
        if (actor && actor.IsDead || actor.gameObject.layer == owner.gameObject.layer)
            return;

        actor.OnBulletHit(owner, damage, transform.position);

        Collider myCollider = GetComponentInChildren<Collider>();
        myCollider.enabled = false;

        hit = true;
        needMove = false;

        //GameObject go = SystemManager.Instance.EffectMng.GenerateEffect(EffectManager.BulletDisappearFXIndex, transform.position);
        GameObject go = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EffectMng.GenerateEffect(EffectManager.BulletDisappearFXIndex, transform.position);
        go.transform.localScale = new Vector3(.2f, .2f, .2f);
        Disappear();
    }

    public void OnTriggerEnter(Collider other)
    {
        OnBulletCollision(other);
    }

    public bool ProcessDisappearCondition()
    {
        if (transform.position.x > 15.0f || transform.position.x < -15.0f
            || transform.position.y > 15.0f || transform.position.y < -15.0f)
        {
            Disappear();
            return true;
        }
        else if (Time.time - firedTime > LIFETIME)
        {
            Disappear();
            return true;
        }

        return false;
    }

    public void Disappear()
    {
        //SystemManager.Instance.BulletMng.Remove(this);
        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().BulletMng.Remove(this);
    }
}
