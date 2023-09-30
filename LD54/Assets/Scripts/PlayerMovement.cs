using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private PlayerInteraction _interaction;

    private Rigidbody2D _rb;
    private Vector2 _moveInput;

    public bool CanMove = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!CanMove) return;
        _rb.velocity = new Vector2(_moveInput.x * _moveSpeed, _moveInput.y * _moveSpeed);
    }

    private void OnMove(InputValue inputValue)
    {
        _moveInput = inputValue.Get<Vector2>();
    }

    private void OnInteract(InputValue inputValue)
    {
        _interaction.InteractWithObject();
    }
}
