using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExPlayerMovement : MonoBehaviour
{
    private CharacterController _characterController;
    private Vector3 _playerVelocity;
    private bool _isGroundedPlayer;

    [Header("Player Speed")]
    private float _playerCurrentSpeed = 2.0f;
    private float _playerWalkSpeed = 2.0f;
    private float _playerRunSpeed = 5.0f;

    private float _jumpHeight = 1.0f;
    private float _gravityValue = -9.81f;

    private void Start()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        _isGroundedPlayer = _characterController.isGrounded;
        if (_isGroundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _playerCurrentSpeed = _playerWalkSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) _playerCurrentSpeed = _playerRunSpeed;

        _characterController.Move(move * Time.deltaTime * _playerCurrentSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        if (Input.GetButtonDown("Jump") && _isGroundedPlayer)
        {
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
        }

        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _characterController.Move(_playerVelocity * Time.deltaTime);
    }


}
