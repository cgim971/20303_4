using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{


    private CharacterController _characterController;

    public float _speed = 5f;
    float _vecGravity = 0;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (_characterController == null) return;

        Move();
        SetGravity();
    }

    void Move()
    {

        Vector3 forward = transform.forward;
        forward.y = 0;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 targetDirection = h * right + v * forward;
        targetDirection.Normalize();

        Vector3 vecGravity = new Vector3(0, _vecGravity, 0);
        Vector3 moveAmount = targetDirection * _speed * Time.deltaTime;
        moveAmount += vecGravity;

        _characterController.Move(moveAmount);
    }

    void SetGravity()
    {
        if (_characterController.isGrounded)
        {
            _vecGravity = 0;
        }
        else
        {
            _vecGravity -= 9.8f * Time.deltaTime;
        }
    }
}
