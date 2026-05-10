using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerController : PlayerComponent
{
    public enum MovementState
    {
        Walking,
        Sprinting,
    }

    public MovementState State
    {
        get;
        private set;
    }
    
    [Header("Movement")] 
    [SerializeField] private float _walkSpeed = 1.0f;
    [SerializeField] private float _sprintSpeed = 2.0f;

    [Header("Gravity")] 
    [SerializeField] private bool _grounded;
    [SerializeField] private float _groundCheckLength = 0.2f;
    
    [Header("Input")]
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _sprintAction;
    [SerializeField] private InputActionReference _jumpAction;
    
    private Rigidbody _rigidbody;
    private Animator _animator;
    
    private PlayerCamera _camera;
    private PlayerModel _model;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        
        _camera = GetComponentInChildren<PlayerCamera>();
        _model = GetComponentInChildren<PlayerModel>();
    }

    private void Start()
    {
        if (!IsOwner)
        {
            return;
        }
        
        _sprintAction.action.started += OnSprintInput;
        _sprintAction.action.canceled += OnSprintInput;
        
        _jumpAction.action.started += OnJumpAction;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        
        if (!IsOwner)
        {
            return;
        }
        
        _sprintAction.action.started -= OnSprintInput;
        _sprintAction.action.canceled -= OnSprintInput;
        
        _jumpAction.action.started -= OnJumpAction;
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }

        _animator.SetFloat("Velocity", _rigidbody.linearVelocity.sqrMagnitude);
        
        _grounded = IsGrounded();
        ProcessInput();
    }

    private void ProcessInput()
    {
        Vector3 input = _moveAction.action.ReadValue<Vector3>();
        if (input != Vector3.zero)
        {
            Vector3 forward = _camera.GetForwardPlane();
            Vector3 right = _camera.GetRightPlane();
            
            Vector3 movement = (forward * input.z + right * input.x).normalized;
            float speed = State == MovementState.Walking ? _walkSpeed : _sprintSpeed;
            
            Move(movement, speed);
            //_camera.HeadBob(input, speed);
        }
    }

    private void OnSprintInput(InputAction.CallbackContext context)
    {
        State = context.started ? MovementState.Sprinting : MovementState.Walking;
    }
    
    private void OnJumpAction(InputAction.CallbackContext context)
    {
        
    }

    private void Move(Vector3 direction, float speed)
    {
        Vector3 movement = direction * speed;
        _rigidbody.AddForce(movement, ForceMode.Force);
        
        _model.SetModelDirection(direction);
    }

    private bool IsGrounded()
    {
        Debug.DrawRay(transform.position, Vector3.down * _groundCheckLength, Color.red);
        return Physics.Raycast(transform.position, Vector3.down, _groundCheckLength);
    }
}
