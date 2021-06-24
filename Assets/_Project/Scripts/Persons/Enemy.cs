using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PersonBase
{
    [HideInInspector]
    public List<Player> _mTargets;
    private Player _currentTarget;

    private bool _isAttack;
    private float _attackTime = .5f;
    private bool _isStart;
    private float _maxSpeed;



    private void Start()
    {
        animator = GetComponent<Animator>();
        speed = Data.Get.LevelData.GetCurrentlevel().EnemySpeed;
        speed *= Random.Range(.95f, 1.05f);
        _maxSpeed = speed * Random.Range(1.05f, 1.2f);
        _mTargets = new List<Player>();
        StartCoroutine(StartMove());
    }

    private IEnumerator StartMove()
    {
        while (!GameManager.Instance.GameStarted())
        { yield return null; }

        yield return new WaitForSeconds(2);

        if (pathCreator == null)
            GameManager.Instance.AddPerson(this);

        isMove = true;
        _isStart = true;
        SetAnimation(PersonAnimations.Idle);
    }

    #region Triggers && Collisions

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Bullet")
            Dead();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player p = other.GetComponent<Player>();
            if (p == null) return;

            AddTarget(p);
        }
        if (other.tag == "Obstacle")
            if (_currentTarget == null)
                isMove = false;

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Player p = other.GetComponent<Player>();
            if (p == null) return;

            RemoveTarget(p);
        }
        if (other.tag == "Obstacle")
            isMove = true;

    }


    #endregion

    #region Update
    private void Update()
    {
        //if (!GameManager.Instance.GameStarted()) return;

        if (!_isStart) return;

        if (_isAttack)
            SetAnimation(PersonAnimations.Attack);
        else if (isMove)
            SetAnimation(PersonAnimations.Walk);
        else
            SetAnimation(PersonAnimations.Idle);

        UpdateTarget();

        if (_currentTarget != null)
        {
            Vector3 lookPos = _currentTarget.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 7);
            transform.position += transform.forward * Time.deltaTime * _maxSpeed;
            Move(false);
        }
        else
        {
            if (pathCreator != null)
                Move(true);
            else isMove = false;
        }


    }
    #endregion

    #region Set Target   
    private void UpdateTarget()
    {
        if (_mTargets == null || _mTargets.Count == 0 || _isAttack) { _currentTarget = null; return; }

        if (_currentTarget == null)
        {
            _currentTarget = _mTargets[0];
        }


        float distance = Vector3.Distance(transform.position, _currentTarget.transform.position);

        foreach (Player p in _mTargets)
        {
            if (!p.isTarget && p.gameObject.activeSelf)
            {
                float dis = Vector3.Distance(transform.position, p.transform.position);
                if (dis < distance)
                {
                    distance = dis;
                    _currentTarget.isTarget = false;
                    _currentTarget = p;
                    _currentTarget.isTarget = true;
                    //_EnemyAnimations = EnemyAnimations.Null;
                }
            }
        }

        if (!_currentTarget.gameObject.activeSelf) { _currentTarget = null; return; }

        if (distance <= .46f)
        {
            Attack();
        }

    }
    public void AddTarget(Player p)
    {
        if (!_isStart) return;

        if (!_mTargets.Contains(p))
            _mTargets.Add(p);
    }
    private void RemoveTarget(Player p)
    {
        if (GameManager.Instance.GameStatus != GameStatus.Normal || !_isStart) return;

        if (_mTargets.Contains(p))
            _mTargets.Remove(p);
    }

    #endregion

    #region Attack
    private void Attack()
    {
        if (!_isStart || _currentTarget == null) return;

        SetAnimation(PersonAnimations.Attack);

        _isAttack = true;

        _mTargets.Remove(_currentTarget);
        _currentTarget.Dead();
        _currentTarget = null;

        StartCoroutine(ReadyForAttack());
    }

    private IEnumerator ReadyForAttack()
    {
        yield return new WaitForSeconds(_attackTime);
        _isAttack = false;
        _PersonAnimations = PersonAnimations.Null;
    }
    #endregion

    public void Dead()
    {
        if (_currentTarget != null)
        {
            _currentTarget.isTarget = false;
            _currentTarget = null;
        }

        isMove = false;
        _isStart = false;
        _isAttack = false;

        _mTargets = null;

        GameManager.Instance.RemovePerson(this);
        SetAnimation(PersonAnimations.Dead);
    }




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 3.5f);

        if (_currentTarget != null)
            Gizmos.DrawLine(transform.position, _currentTarget.transform.position);
    }

}
