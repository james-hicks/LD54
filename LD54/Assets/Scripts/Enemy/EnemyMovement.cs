using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using System;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] protected Transform _enemyGFX;
    [Header("Enemy Stats")]
    public int HP = 2;
    public int AP = 1;
    [SerializeField] protected float _speed = 4f;
    [SerializeField] protected float _attackCooldown = 2f;
    [SerializeField] protected List<Drops> _drops = new List<Drops>();
    [SerializeField] protected Animator _enemyAnimator;


    [Header("Pathfinding Settings")]
    [SerializeField] protected float _nextWaypointDistance = 1f;
    [SerializeField] protected float _attackDistance = 1.5f;
    [SerializeField] protected float _awareDistance = 15f;

    [Header("State Debug")]
    public BaseState CurrentState;

    public Transform target => PlayerMovement.PlayerInstance.transform;

    protected Path _path;
    protected int _currentWaypoint = 0;
    protected bool _reachedEndofPath = false;
    protected Seeker _seeker;
    protected Rigidbody2D _rb;
    protected bool _hasDied = false;

    public virtual void Awake()
    {
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Start()
    {
        CurrentState = BaseState.Idle;
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    public virtual void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;
            _currentWaypoint = 0;
        }
    }

    public virtual void UpdatePath()
    {
        if (CurrentState != BaseState.Chase) return;

        if (_seeker.IsDone())
        {
            _seeker.StartPath(_rb.position, target.position, OnPathComplete);
        }

    }

    public virtual void FixedUpdate()
    {
        if (Vector2.Distance(_rb.position, target.position) <= _awareDistance)
        {
            CurrentState = BaseState.Chase;
        }
        else if (Vector2.Distance(_rb.position, target.position) > _awareDistance)
        {
            CurrentState = BaseState.Idle;
        }


        if (Vector2.Distance(_rb.position, target.position) <= _attackDistance)
        {
            CurrentState = BaseState.Attack;
        }
        else
        {
            _enemyAnimator.SetBool("Attack", false);
        }
        
        if (HP <= 0)
        {
            CurrentState = BaseState.Death;
            _path = null;
        }


        switch (CurrentState)
        {
            case BaseState.Idle:
                break;
            case BaseState.Attack:
                StartAttack();
                break;
            case BaseState.Chase:
                Chase();
                break;
            case BaseState.Death:
                Death();
                break;
        }

        _enemyAnimator.SetBool("Move", _rb.velocity.magnitude > 0.5f || _rb.velocity.magnitude < -0.5f);
        //if (_rb.velocity.magnitude > 0.5f || _rb.velocity.magnitude < -0.5f)
        //{
        //    _enemyAnimator.SetBool("Move", true);
        //}
    }
    public virtual void StartAttack()
    {
        // Attack player
        _enemyAnimator.SetBool("Attack", true);
    }

    public virtual void Attack()
    {
        PlayerMovement.PlayerInstance.ChangeHealth(-AP);
        Debug.Log($"{gameObject.name} has attacked {PlayerMovement.PlayerInstance.gameObject.name} dealing {AP} damage.");
    }

    public virtual void Chase()
    {
        if (_path == null) return;
        if (_currentWaypoint >= _path.vectorPath.Count)
        {
            _reachedEndofPath = true;
            return;
        }
        else
        {
            _reachedEndofPath = false;
        }

        Vector2 direction = ((Vector2)_path.vectorPath[_currentWaypoint] - _rb.position).normalized;
        Vector2 force = direction * _speed * 100 * Time.deltaTime;

        _rb.AddForce(force);

        float distance = Vector2.Distance(_rb.position, _path.vectorPath[_currentWaypoint]);

        if (distance < _nextWaypointDistance)
        {
            _currentWaypoint++;
        }
    }

    public virtual void Death()
    {
        // Die
        if (_hasDied) return;
        Debug.Log(gameObject.name + " has Died.");
        _enemyAnimator.SetBool("Death", true);
        Destroy(gameObject, 3f);
        _hasDied = true;

        foreach(var drop in _drops)
        {
            int rng = UnityEngine.Random.Range(0, 10);
            if(drop.DropRate >= rng)
            {
                GameObject a = Instantiate(drop.ItemDrop.SpawnablePrefab, transform.position, Quaternion.identity);
                a.GetComponent<ItemPickup>().SetItem(drop.ItemDrop);
            }
        }
    }

    private void OnDestroy()
    {
        UnitDeath.Invoke(gameObject);
    }

    public Action<GameObject> UnitDeath;
}

public enum BaseState
{
    Idle,
    Chase,
    Attack,
    Death
}

[Serializable]
public class Drops
{
    public int DropRate;
    public Item ItemDrop;
}
