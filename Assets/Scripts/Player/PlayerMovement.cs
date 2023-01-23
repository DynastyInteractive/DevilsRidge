using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _walkSpeed = 2.0f;
    [SerializeField] private float _runSpeed = 3.5f;
    [SerializeField] private float _jumpHeight = 1.0f;
    [SerializeField] private float _gravityValue = -9.81f;
    [SerializeField] private float _maxCoyoteTime = 0.1f;

    private PlayerInput Input;

    private Vector3 m_PlayerVelocity;
    private bool m_IsGrounded;
    private float m_CoyoteTime;

    private void Start()
    {
        Input = new PlayerInput();
        Input.Enable();
    }

    private void Update()
    {
        if (!IsOwner) return;

        CoyoteTime();
        CheckGrounded();
        Move();
        JumpCheck();
        Gravity();
    }

    private void CoyoteTime()
    {
        if (_characterController.isGrounded)
            m_CoyoteTime = 0;
        else
            m_CoyoteTime += Time.deltaTime;
    }

    private void CheckGrounded()
    {
        m_IsGrounded = m_CoyoteTime < _maxCoyoteTime;
    }

    private void Move()
    {
        if (m_IsGrounded && m_PlayerVelocity.y < 0)
            m_PlayerVelocity.y = 0f;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.Player.Sprint.IsPressed();
        float curSpeedX = (isRunning ? _runSpeed : _walkSpeed) * Input.Player.Move.ReadValue<Vector2>().x;
        float curSpeedY = (isRunning ? _runSpeed : _walkSpeed) * Input.Player.Move.ReadValue<Vector2>().y;

        Vector3 moveDirection = (forward * curSpeedY) + (right * curSpeedX);

        _characterController.Move(Time.deltaTime * moveDirection);
    }

    private void JumpCheck()
    {
        if (m_IsGrounded && Input.Player.Jump.WasPressedThisFrame())
        {
            m_PlayerVelocity.y += Mathf.Sqrt(-_jumpHeight * _gravityValue);
        }

        if (Input.Player.Jump.WasReleasedThisFrame() && m_PlayerVelocity.y > 0f)
        {
            m_PlayerVelocity.y *= 0.5f;
        }
    }

    private void Gravity()
    {
        m_PlayerVelocity.y += (_gravityValue) * Time.deltaTime;
        _characterController.Move(m_PlayerVelocity * Time.deltaTime);
    }
}