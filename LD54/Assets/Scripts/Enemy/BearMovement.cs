using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;

public class BearMovement : EnemyMovement
{
    [Header("Bear Attacks and Stats")]
    public BearStates BearState;
    public bool isCharging = false;
    [SerializeField] private float _chargePower = 25f;
    [SerializeField] private int _chargeFrequency = 4;
    [SerializeField] private float _chargeCooldown = 10f;
    [SerializeField] private float _chargeWarmup = 2f;
    [SerializeField] private float _stunDuration = 3f;
    [SerializeField] private Transform _chargeIndicator;
    private float _chargeCooldownRemaining;


    [Header("Bear Sounds")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip _attack;
    [SerializeField] private AudioClip _death;
    [SerializeField] private AudioClip _charge;

    public override void Start()
    {
        BearState = BearStates.Idle;
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    public override void UpdatePath()
    {
        if (BearState != BearStates.Chase) return;

        if (_seeker.IsDone())
        {
            _seeker.StartPath(_rb.position, target.position, OnPathComplete);
        }

    }

    public override void FixedUpdate()
    {
        if (Vector2.Distance(_rb.position, target.position) <= _awareDistance && !isCharging)
        {
            BearState = BearStates.Chase;
        }
        else if (Vector2.Distance(_rb.position, target.position) > _awareDistance)
        {
            BearState = BearStates.Idle;
        }


        if (Vector2.Distance(_rb.position, target.position) <= _attackDistance)
        {
            BearState = BearStates.Attack;
        }


        if (HP <= 0)
        {
            BearState = BearStates.Death;
            _path = null;
        }

        
        if (BearState == BearStates.Chase)
        {
            if (Random.Range(0, 10) <= _chargeFrequency && _chargeCooldownRemaining >= _chargeCooldown)
            {
                _chargeCooldownRemaining = 0;
                BearState = BearStates.Charge;
            }
        }
        else if (isCharging)
        {
            BearState = BearStates.Charge;
            if (_rb.velocity.magnitude < 0.5 && _rb.velocity.magnitude > -0.5 && charging)
            {
                Debug.Log("Done Charging");
                charging = false;
                BearState = BearStates.Stunned;
            }
        }


        if(charging)
        {
            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(4, 4), 0);
            foreach (Collider2D col in hitColliders)
            {
                if (col.TryGetComponent(out PlayerMovement player))
                {
                    PlayerMovement.PlayerInstance.ChangeHealth(-2);
                    Debug.Log($"{gameObject.name} has attacked {PlayerMovement.PlayerInstance.gameObject.name} dealing {AP} damage.");
                }
            }
        }


        switch (BearState)
        {
            case BearStates.Idle:
                break;
            case BearStates.Attack:
                StartAttack();
                break;
            case BearStates.Chase:
                Chase();
                _chargeCooldownRemaining += Time.deltaTime;
                break;
            case BearStates.Death:
                Death();
                break;
            case BearStates.Charge:
                Charge();
                _path = null;
                break;
            case BearStates.Stunned:
                Stunned();
                break;
        }

        _enemyAnimator.SetBool("Move", _rb.velocity.magnitude > 0.5f || _rb.velocity.magnitude < -0.5f);
    }

    public override void Attack()
    {
        source.clip = _attack;
        source.Play();
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(4, 4), 0);
        foreach (Collider2D col in hitColliders)
        {
            if (col.TryGetComponent(out PlayerMovement player))
            {
                PlayerMovement.PlayerInstance.ChangeHealth(-AP);
                Debug.Log($"{gameObject.name} has attacked {PlayerMovement.PlayerInstance.gameObject.name} dealing {AP} damage.");
            }
        }
        _enemyAnimator.SetBool("Attack", false);
    }

    private bool charging = false;
    private Vector3 ChargeDirection;
    public void Charge()
    {
        if (isCharging) return;
        isCharging = true;

        Debug.Log("Start Charge Warmup");
        _rb.velocity = Vector2.zero;
        ChargeDirection = target.position - transform.position;
        _chargeIndicator.gameObject.SetActive(true);
        _enemyAnimator.SetBool("Charge", true);
        Vector3 chargeDirectionArrow = (_chargeIndicator.transform.position - target.position).normalized;
        _chargeIndicator.rotation = Quaternion.FromToRotation(Vector3.down, chargeDirectionArrow);
    }

    public void ChargeAttack()
    {
        Debug.Log("Start Charge Action");
        source.clip = _charge;
        source.Play();
        _enemyAnimator.SetBool("Charge", false);
        _chargeIndicator.gameObject.SetActive(false);
        charging = true;
        _rb.velocity = new Vector2(ChargeDirection.x * _chargePower, ChargeDirection.y * _chargePower);
    }

    private bool isStunned = false;
    public void Stunned()
    {
        if(isStunned) return;
        isStunned = true;

        StartCoroutine(Stun());
    }

    private IEnumerator Stun()
    {
        Debug.Log("Stunned");
        _enemyAnimator.SetBool("Stunned", true);
        yield return new WaitForSeconds(_stunDuration);
        _enemyAnimator.SetBool("Stunned", false);
        isStunned = false;
        isCharging = false;
    }

    public override void Death()
    {
        // Die
        if (_hasDied) return;
        source.clip = _death;
        source.Play();
        Debug.Log(gameObject.name + " has Died.");
        _enemyAnimator.SetBool("Death", true);
        Destroy(gameObject, 3f);
        _hasDied = true;

        foreach (var drop in _drops)
        {
            int rng = UnityEngine.Random.Range(0, 10);
            if (drop.DropRate >= rng)
            {
                GameObject a = Instantiate(drop.ItemDrop.SpawnablePrefab, transform.position, Quaternion.identity);
                a.transform.position += new Vector3(UnityEngine.Random.Range(0.2f, 1f), UnityEngine.Random.Range(0.2f, 1f), 0);
                if (a.GetComponent<ItemPickup>() != null) a.GetComponent<ItemPickup>().SetItem(drop.ItemDrop);
                if (a.GetComponent<AbilityPickup>() != null) a.GetComponent<AbilityPickup>().SetItem(drop.ItemDrop);
            }
        }
    }
}

public enum BearStates
{
    Idle,
    Chase,
    Attack,
    Charge,
    Stunned,
    Death
}
