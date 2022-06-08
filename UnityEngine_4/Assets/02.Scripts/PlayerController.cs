using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float _currentSpeed = 0;
    public float _runSpeed = 10f;
    public float _walkSpeed = 5f;

    private CharacterController _characterController;
    Vector3 _nextPos = Vector3.zero;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }


    private void Update()
    {
        if (_characterController == null) return;

        Move();
        Gravity();
    }


    private void Move()
    {
        Vector3 forward = transform.forward;
        forward.y = 0;
        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        _currentSpeed = _walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) _currentSpeed = _runSpeed;

        _nextPos = (forward * v + right * h) * _currentSpeed * Time.deltaTime;


        _characterController.Move(_nextPos);
    }

    void Gravity()
    {
        if (_characterController.isGrounded)
        {
            _nextPos.y = 0;
        }
        else
        {
            _nextPos.y -= 9.8f * Time.deltaTime;
        }
    }
}
