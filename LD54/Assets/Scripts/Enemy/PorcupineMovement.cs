using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorcupineMovement : EnemyMovement
{

    public override void FixedUpdate()
    {
        if (Vector2.Distance(_rb.position, target.position) <= _awareDistance)
        {
            CurrentState = BaseState.Chase;
        }
        else if (Vector2.Distance(_rb.position, target.position) > _awareDistance)
        {
            CurrentState = BaseState.Idle;
        }


        if (Vector2.Distance(_rb.position, target.position) <= _attackDistance || _enemyAnimator.GetBool("Attack"))
        {
            CurrentState = BaseState.Attack;
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
    }
    public override void Attack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(3, 3), 0);
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
}
