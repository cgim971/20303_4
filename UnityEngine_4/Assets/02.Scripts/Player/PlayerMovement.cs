using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController _characterController;

    [Header("Movement Property")]
    Vector3 moveAmount = Vector3.zero;
    public float _speed = 5f;
    public float _jumpPower = 8f;
    public float _gravity = 20f;
    float _vecGravity = 0;
    float h;
    float v;

    [Header("Animation Property")]
    AnimationType _animationType = AnimationType.NONE;
    Animator _animator;
    bool _isAttacking = false;


    enum AnimationType
    {
        NONE,
        IDLE,
        WALK,
        RUN,
        ATTACK,
        DEATH
    }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_characterController == null) return;

        Move();
        SetGravity();
        
        if (Input.GetMouseButtonDown(0))
        {
            _animationType = AnimationType.ATTACK;
        }

        PlayAnimation();
    }

    void PlayAnimation()
    {
        if (_isAttacking) return;

        switch (_animationType)
        {
            case AnimationType.IDLE:
                _animator.SetBool("isMoving", false);
                break;
            case AnimationType.WALK:
            case AnimationType.RUN:
                _animator.SetFloat("H", h);
                _animator.SetFloat("V", v);
                _animator.SetBool("isMoving", true);
                break;
            case AnimationType.ATTACK:
                _animator.SetTrigger("Attack");
                _isAttacking = true;
                break;
            case AnimationType.DEATH:
                _animator.SetTrigger("Death");
                break;
        }
    }

    void Move()
    {
        Vector3 forward = transform.forward;
        forward.y = 0;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        // animation Set
        _animationType = (h == 0 && v == 0) ? AnimationType.IDLE : AnimationType.WALK;

        Vector3 targetDirection = h * right + v * forward;
        targetDirection.Normalize();

        Vector3 vecGravity = new Vector3(0, _vecGravity, 0);
        moveAmount = targetDirection * _speed;
        moveAmount += vecGravity;

        _characterController.Move(moveAmount * Time.deltaTime);
    }

    void SetGravity()
    {
        if (_characterController.isGrounded) _vecGravity = 0;
        else _vecGravity -= _gravity * Time.deltaTime;
    }
        
    public void FinishAttack()
    {
        _isAttacking = false;
    }
}
