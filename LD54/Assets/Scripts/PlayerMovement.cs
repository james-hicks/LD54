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

    private Rigidbody2D _rb;
    private Vector2 _moveInput;

    public bool CanMove = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if(PlayerInstance != null )
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
        _rb.velocity = new Vector2(_moveInput.x * _moveSpeed, _moveInput.y * _moveSpeed);

    }

    private void Update()
    {
        if(HP <= 0)
        {
            Debug.LogWarning("PLAYER HAS DIED");
        }


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
        if(HP + amount > _maxHP)
        {
            HP = _maxHP;
        } else
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

    #region Input Management
    private void OnMove(InputValue inputValue)
    {
        _moveInput = inputValue.Get<Vector2>();
    }

    private void OnInteract(InputValue inputValue)
    {
        _interaction.InteractWithObject();
    }
    #endregion
}
