using UnityEngine;

public class PlayerFPSController : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _cameraSensitivity = 2.0f;
    [SerializeField] private float _walkSpeed = 2.0f;
    [SerializeField] private float _runSpeed = 3.5f;
    [SerializeField] private float _jumpHeight = 1.0f;
    [SerializeField] private float _gravityValue = -9.81f;
    [SerializeField] private float _maxCoyoteTime = 0.1f;

    public float CameraSensitivity
    {
        get => _cameraSensitivity;
        set => _cameraSensitivity = value;
    }

    private PlayerInput Input;

    private Vector3 m_PlayerVelocity;
    private bool m_IsGrounded;
    private float m_CoyoteTime;
    private float m_RotationX = 0;

    private void Start()
    {
        Input = new PlayerInput();
        Input.Enable();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CoyoteTime();
        CheckGrounded();
        Move();
        JumpCheck();
        Gravity();
        Look();
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

    private void Look()
    {
        m_RotationX += -Input.Player.Mouse.ReadValue<Vector2>().y * _cameraSensitivity;
        m_RotationX = Mathf.Clamp(m_RotationX, -85f, 60f);
        _playerCamera.transform.localRotation = Quaternion.Euler(m_RotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.Player.Mouse.ReadValue<Vector2>().x * _cameraSensitivity, 0);
    }
}