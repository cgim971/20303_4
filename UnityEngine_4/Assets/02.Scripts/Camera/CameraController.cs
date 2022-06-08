using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _camTransform;

    [SerializeField] private float _sensX;
    [SerializeField] private float _sensY;

    float mouseX;
    float mouseY;

    float _xRotation;
    float _yRotation;

    float multiplier = 0.01f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        MyInput();

        transform.localEulerAngles = new Vector3(_xRotation, _yRotation, 0);
        _camTransform.parent.localEulerAngles = new Vector3(0, _yRotation, 0);
    }

    private void MyInput()
    {
        transform.position = _camTransform.position;

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        _yRotation += mouseX * _sensX * multiplier;
        _xRotation -= mouseY * _sensY * multiplier;

        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
    }
}
