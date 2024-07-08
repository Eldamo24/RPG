using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("GameObjects")] 
    private GameObject _playerParent;

    [Header("References")] 
    private Transform _orientation;
    private Transform _player;
    private Transform _playerObj;
    private Rigidbody _rb;
    private Camera _cam;
    private float _rotationSpeed = 7f;
    
    [Header("Animation")] 
    private Animator _anim;
    
    [Header("Movement variables")]
    private Vector2 _playerInputs;
    private float _moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private bool _isRunningWithKey, _isRunningWithStick, _isRunning;
    
    [Header("CheckGrounded")]
    private bool _isGrounded;
    private float _groundDistance = 0.01f;
    [SerializeField] private LayerMask _groundLayer;    

    void Start()
    {
        _playerParent = GameObject.FindWithTag(("Player"));
        _orientation = GameObject.Find("Orientation").GetComponent<Transform>();
        _player = GetComponent<Transform>();
        _playerObj = GameObject.Find("PlayerObj").GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();
        _cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        _anim = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        _isGrounded = Physics.CheckSphere(transform.position, _groundDistance, _groundLayer);
        Movement();
        if (_playerInputs != Vector2.zero)
        {
            _moveSpeed = walkSpeed;
        }
        if (_isRunningWithKey || _isRunningWithStick)
        {
            _isRunning = true;
        }
        else if (!_isRunningWithKey && !_isRunningWithStick)
        {
            _isRunning = false;
        }
    }

    private void Movement()
    {
        Vector3 viewDir = _player.position -
                          new Vector3(_cam.transform.position.x, _player.position.y, _cam.transform.position.z);
        _orientation.forward = viewDir.normalized;
        Vector3 inputDir = _playerInputs.x * _orientation.right + _playerInputs.y * _orientation.forward;
        if (inputDir != Vector3.zero)
        {
            _playerObj.forward =
                Vector3.Slerp(_playerObj.forward, inputDir.normalized, Time.deltaTime * _rotationSpeed);
            _rb.MovePosition(_player.position + inputDir * Time.deltaTime * _moveSpeed);
        }
    }
    // private void Movement()
    // {
    //     Vector3 movement = _playerInputs.x * Vector3.right + _playerInputs.y * Vector3.forward;
    //     movement.Normalize();
    //     if (_isGrounded && movement != Vector3.zero)
    //     {
    //         if(_isRunning)
    //         {
    //             _moveSpeed = runSpeed;
    //             _anim.SetTrigger("run");
    //         }
    //         if (!_isRunning)
    //         {
    //             _moveSpeed = walkSpeed;
    //             _anim.SetTrigger("walk");
    //         }
    //     }
    //     if(movement == Vector3.zero)
    //     {
    //         _anim.SetTrigger("idle");
    //     }
    //     _playerParent.transform.Translate(movement * _moveSpeed * Time.deltaTime, Space.World);
    // }

    public void MoveInput(InputAction.CallbackContext context)
    {
        _playerInputs = context.ReadValue<Vector2>();
    }

    public void Running(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isRunningWithKey = true;
        }
        if(context.performed)    
        {
            _isRunningWithKey = true;
        }
        if (context.canceled)
        {
            _isRunningWithKey = false;
        }
    }

    public void SetIsRunning(bool isRunning)
    {
        _isRunningWithStick = isRunning;
    }
}
