using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform _camTransform;
    float _mouseX;
    float _mouseY;
    private void Start()
    {

    }

    private void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _mouseX = mouseX + transform.localEulerAngles.y;
        _mouseX = (_mouseX > 180f) ? _mouseX - 360f : _mouseX;

        _mouseY = _mouseY + mouseY;
        _mouseY = (_mouseY > 180f) ? _mouseY - 360f : _mouseY;

        transform.position = _camTransform.position;
        transform.localEulerAngles = new Vector3(-_mouseY, _mouseX, 0);
        _camTransform.parent.eulerAngles = new Vector3(0, _mouseX, 0);
    }
}
