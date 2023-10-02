using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderTree : MonoBehaviour
{
    [SerializeField] private float _attackCooldown = 5f;
    [SerializeField] private float _warningTime = 2f;
    [SerializeField] private float _branchTime = 2f;
    public bool attacking = false;
    public TreeStates TreeState;

    private float _awareDistance = 15;
    private float _attackCooldownRemaining = 0f;
    private Transform _playerPosition => PlayerMovement.PlayerInstance.gameObject.transform;
    public int HP = 50;

    public GameObject warningCircle;
    public GameObject branchAttack;
    // Start is called before the first frame update
    void Start()
    {
        TreeState = TreeStates.Idle;
    }

    void FixedUpdate()
    {
        if (TreeState == TreeStates.Death) return;

        if (Vector2.Distance(transform.position, _playerPosition.position) <= _awareDistance)
        {
            TreeState = TreeStates.Awoken;
        }
        else
        {
            TreeState = TreeStates.Idle;
        }

        if (TreeState == TreeStates.Idle) return;

        if (!attacking)
        {
            _attackCooldownRemaining += Time.deltaTime;
        }

        if (_attackCooldownRemaining >= _attackCooldown)
        {
            TreeState = TreeStates.Attack;
            _attackCooldownRemaining = 0f;
        }

        if (HP <= 0)
        {
            TreeState = TreeStates.Death;
        }

        switch (TreeState)
        {
            case TreeStates.Awoken:
                break;
            case TreeStates.Attack:
                StartCoroutine(Attack());
                TreeState = TreeStates.Awoken;
                break;
            case TreeStates.Death:
                Death();
                break;
        }
    }

    private IEnumerator Attack()
    {
        attacking = true;
        // Spawn a warning circle
        Vector3 pos = _playerPosition.position;
        GameObject warningObj = Instantiate(warningCircle, pos, Quaternion.identity);
        yield return new WaitForSeconds(_warningTime);
        // Spawn branch
        Destroy(warningObj);
        GameObject branchObj = Instantiate(branchAttack, pos, Quaternion.identity);
        yield return new WaitForSeconds(_branchTime);
        Destroy(branchObj);
        attacking = false;
    }

    void Death()
    {
        Debug.Log("Elder Tree has Died.");
    }
}

public enum TreeStates
{
    Idle,
    Awoken,
    Attack,
    Death
}
