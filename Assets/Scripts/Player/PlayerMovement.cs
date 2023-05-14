using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] CharacterController _characterController;
    [SerializeField] float _walkSpeed = 2.0f;
    [SerializeField] float _runSpeed = 3.5f;
    [SerializeField] float _jumpHeight = 1.0f;
    [SerializeField] float _gravityValue = -9.81f;
    [SerializeField] float _maxCoyoteTime = 0.1f;

    public PlayerInput Input { get; private set; }

    Vector3 _playerVelocity;
    bool _isGrounded;
    float _coyoteTime;

    void Start()
    {
        Input = new PlayerInput();
        Input.Enable();
    }

    void Update()
    {
        if (!IsOwner) return;

        if (_characterController == null) return;

        CoyoteTime();
        CheckGrounded();
        Move();
        JumpCheck();
        Gravity();
    }

    void CoyoteTime()
    {
        if (_characterController.isGrounded)
            _coyoteTime = 0;
        else
            _coyoteTime += Time.deltaTime;
    }

    void CheckGrounded()
    {
        _isGrounded = _coyoteTime < _maxCoyoteTime;
    }

    void Move()
    {
        if (_isGrounded && _playerVelocity.y < 0)
            _playerVelocity.y = 0f;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.Player.Sprint.IsPressed();
        float curSpeedX = (isRunning ? _runSpeed : _walkSpeed) * Input.Player.Move.ReadValue<Vector2>().x;
        float curSpeedY = (isRunning ? _runSpeed : _walkSpeed) * Input.Player.Move.ReadValue<Vector2>().y;

        Vector3 moveDirection = (forward * curSpeedY) + (right * curSpeedX);

        _characterController.Move(Time.deltaTime * moveDirection);
    }

    void JumpCheck()
    {
        if (_isGrounded && Input.Player.Jump.WasPressedThisFrame())
        {
            _playerVelocity.y += Mathf.Sqrt(-_jumpHeight * _gravityValue);
        }

        if (Input.Player.Jump.WasReleasedThisFrame() && _playerVelocity.y > 0f)
        {
            _playerVelocity.y *= 0.5f;
        }
    }

    void Gravity()
    {
        _playerVelocity.y += (_gravityValue) * Time.deltaTime;
        _characterController.Move(_playerVelocity * Time.deltaTime);
    }
}