using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController _characterController;
    public float speed = 5f;
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 forward = transform.forward;
        forward.y = 0;
        Vector3 right = new Vector3(-forward.z, 0, forward.x);

        Vector3 pos = (forward * v + right * h) * speed * Time.deltaTime;

        _characterController.Move(pos);
    }
}
