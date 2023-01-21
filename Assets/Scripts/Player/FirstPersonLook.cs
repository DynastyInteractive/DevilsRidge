using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private Camera _camera;
    [Range(1f, 100f)]
    [SerializeField] private float _cameraSensitivity = 60f;
    [SerializeField] private Vector3 _cameraPositionOffset = new Vector3(0f, 0.5f, 0.05f);
    [SerializeField] private float _maxLookUpAngle = 90f;
    [SerializeField] private float _maxLookDownAngle = 75f;

    private PlayerInput Input;

    private float m_RotationX = 0;

    public float CameraSensitivity
    {
        get => _cameraSensitivity;
        set => _cameraSensitivity = value;
    }

    private void Start()
    {
        Input = new PlayerInput();
        Input.Enable();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        _camera.transform.position = _parent.position + _cameraPositionOffset;
    }

    private void Rotate()
    {
        float lookSpeed = _cameraSensitivity / 200f;
        m_RotationX += -Input.Player.Look.ReadValue<Vector2>().y * lookSpeed;
        m_RotationX = Mathf.Clamp(m_RotationX, -_maxLookUpAngle, _maxLookDownAngle);
        _camera.transform.localRotation = Quaternion.Euler(m_RotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.Player.Look.ReadValue<Vector2>().x * lookSpeed, 0);
    }
}
