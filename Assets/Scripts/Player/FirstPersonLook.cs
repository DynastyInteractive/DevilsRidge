using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class FirstPersonLook : NetworkBehaviour
{
    [SerializeField] Transform _parent;
    [SerializeField] Camera _camera;
    [Range(1f, 100f)]
    [SerializeField] float _cameraSensitivity = 60f;
    [SerializeField] Vector3 _cameraPositionOffset = new Vector3(0f, 0.5f, 0.05f);
    [SerializeField] float _maxLookUpAngle = 90f;
    [SerializeField] float _maxLookDownAngle = 75f;

    PlayerInput Input;

    float _rotationX = 0;

    public float CameraSensitivity
    {
        get => _cameraSensitivity;
        set => _cameraSensitivity = value;
    }

    void Start()
    {
        if (!IsOwner)
        {
            _camera.gameObject.SetActive(false);
        }

        Input = new PlayerInput();
        Input.Enable();
    }

    void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnDisable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void LateUpdate()
    {
        if (!IsOwner) return;
        Move();
        Rotate();
    }

    void Move()
    {
        _camera.transform.position = _parent.position + _cameraPositionOffset;
    }

    void Rotate()
    {
        float lookSpeed = _cameraSensitivity / 200f;
        _rotationX += -Input.Player.Look.ReadValue<Vector2>().y * lookSpeed;
        _rotationX = Mathf.Clamp(_rotationX, -_maxLookUpAngle, _maxLookDownAngle);
        _camera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.Player.Look.ReadValue<Vector2>().x * lookSpeed, 0);
    }
}
