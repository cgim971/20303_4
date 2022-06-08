using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float _playerHeight = 2f;


    [Header("Movement")]
    public float _moveSpeed = 6f;
    float movementMultiplier = 10f;
    [SerializeField] float _airMultiplier = 0.4f;

    [Header("Jumping")]
    public float _jumpForce = 5f;

    [Header("Keybinds")]
    [SerializeField] KeyCode _jumpKey = KeyCode.Space;

    [Header("Drag")]
    float _groundDrag = 6f;
    float _airDrag = 2f;


    float _horizontalMovement;
    float _verticalMovement;

    bool _isGrounded;



    Vector3 _moveDirection;

    Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    private void Update()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight / 2 + 0.1f);

        MyInput();
        ControlDrag();

        if (Input.GetKeyDown(_jumpKey) && _isGrounded)
        {
            Jump();
        }
    }

    void MyInput()
    {
        _horizontalMovement = Input.GetAxisRaw("Horizontal");
        _verticalMovement = Input.GetAxisRaw("Vertical");

        _moveDirection = transform.forward * _verticalMovement + transform.right * _horizontalMovement;
        _moveDirection.y = 0;
    }

    void Jump()
    {
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }


    void ControlDrag()
    {
        if (_isGrounded)
        {
            _rb.drag = _groundDrag;
        }
        else
        {
            _rb.drag = _airDrag;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (_isGrounded)
            _rb.AddForce(_moveDirection.normalized * _moveSpeed * movementMultiplier, ForceMode.Acceleration);
        else
            _rb.AddForce(_moveDirection.normalized * _moveSpeed * movementMultiplier * _airMultiplier, ForceMode.Acceleration);
    }


}
