using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform _enemyGFX;
    [Header("Enemy Stats")]
    public int HP = 2;
    public int AP = 1;
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _attackCooldown = 2f;


    [Header("Pathfinding Settings")]
    [SerializeField] private float _nextWaypointDistance = 1f;
    [SerializeField] private float _attackDistance = 1.5f;
    [SerializeField] private float _awareDistance = 15f;

    [Header("State Debug")]
    public State CurrentState;

    public Transform target => PlayerMovement.PlayerInstance.transform;

    private Path _path;
    private int _currentWaypoint = 0;
    private bool _reachedEndofPath = false;
    private Seeker _seeker;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        CurrentState = State.Idle;
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;
            _currentWaypoint = 0;
        }
    }

    private void UpdatePath()
    {
        if (CurrentState != State.Chase) return;

        if (_seeker.IsDone())
        {
            _seeker.StartPath(_rb.position, target.position, OnPathComplete);
        }

    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(_rb.position, target.position) <= _awareDistance)
        {
            CurrentState = State.Chase;
        }
        else if (Vector2.Distance(_rb.position, target.position) > _awareDistance)
        {
            CurrentState = State.Idle;
        }


        if (Vector2.Distance(_rb.position, target.position) <= _attackDistance)
        {
            CurrentState = State.Attack;
        }
        else
        {
            attackDelay = 0f;
        }
        
        if (HP <= 0)
        {
            CurrentState = State.Death;
        }


        switch (CurrentState)
        {
            case State.Idle:
                break;
            case State.Attack:
                Attack();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Death:
                Death();
                break;
        }

    }

    private float attackDelay = 0f;
    protected virtual void Attack()
    {
        // Attack player
        attackDelay += Time.deltaTime;
        if(attackDelay >= _attackCooldown)
        {
            PlayerMovement.PlayerInstance.ChangeHealth(-AP);
            Debug.Log($"{gameObject.name} has attacked {PlayerMovement.PlayerInstance.gameObject.name} dealing {AP} damage.");
            attackDelay = 0f;
        }

    }

    protected virtual void Chase()
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

    protected virtual void Death()
    {
        // Die
        Debug.Log(gameObject.name + " has Died.");
    }
}

public enum State
{
    Idle,
    Chase,
    Attack,
    Death
}
