using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement PlayerInstance;
    public int HP = 2;
    private int _maxHP = 2;
    public int AP = 1;

    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private PlayerInteraction _interaction;
    [SerializeField] private Animator _playerAnimator;

    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private bool _died;

    public bool CanMove = true;

    // Dashing Variables
    public bool canDash = true;
    public bool isDashing;
    private float dashingPower = 12f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (PlayerInstance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            PlayerInstance = this;
        }
    }

    private void Start()
    {
        OnPlayerHPChanged?.Invoke(HP);
    }

    private void FixedUpdate()
    {
        if (!CanMove) return;
        if (isDashing) return;
        _rb.velocity = new Vector2(_moveInput.x * _moveSpeed, _moveInput.y * _moveSpeed);

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void Update()
    {
        if (HP <= 0)
        {
            Debug.LogWarning("PLAYER HAS DIED");
            Die();
        }

        if(_moveInput.x != 0 || _moveInput.y != 0)
        {
            _playerAnimator.SetBool("Moving", true);
        }
        else
        {
            _playerAnimator.SetBool("Moving", false);
        }

        if (isDashing) return;


#if UNITY_EDITOR
        // Debug Health Change Check
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ChangeHealth(-1);
        } else if (Input.GetKeyDown(KeyCode.F2))
        {
            ChangeHealth(1);
        } else if (Input.GetKeyDown(KeyCode.F3))
        {
            ChangeAttackPower(-1);
        } else if (Input.GetKeyDown(KeyCode.F4))
        {
            ChangeAttackPower(1);
        }
#endif
    }

    public void IncreaseMaxHealth()
    {
        _maxHP++;
        ChangeHealth(_maxHP);
    }

    public void ChangeHealth(int amount)
    {
        if (HP + amount > _maxHP)
        {
            HP = _maxHP;
        }
        else
        {
            HP += amount;
        }
        OnPlayerHPChanged?.Invoke(HP);
    }
    public static Action<int> OnPlayerHPChanged;

    public void ChangeAttackPower(int amount)
    {
        AP += amount;
        OnPlayerAPChanged?.Invoke(AP);

    }
    public static Action<int> OnPlayerAPChanged;


    private void Die()
    {
        if(_died) return;
        _died = true;

        transform.position = Vector3.zero;
        ChangeHealth(_maxHP);
        _died = false;

    }

    #region Input Management
    private void OnMove(InputValue inputValue)
    {
        _moveInput = inputValue.Get<Vector2>();
    }

    private IEnumerator Dash()
    {
        if (_moveInput.x + _moveInput.y == 0f) yield break;
        canDash = false;
        isDashing = true;
        Debug.Log("Dashing");
        _rb.velocity = new Vector2(_moveInput.x * dashingPower, _moveInput.y * dashingPower);
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        Debug.Log("Dash Back Up");
    }

    private void OnInteract(InputValue inputValue)
    {
        _interaction.InteractWithObject();
    }
    #endregion
}
