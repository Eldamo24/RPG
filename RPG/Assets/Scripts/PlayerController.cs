using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("GameObjects")] 
    private GameObject _playerParent;

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
        _anim = GetComponent<Animator>();
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
        Vector3 movement = _playerInputs.x * Vector3.right + _playerInputs.y * Vector3.forward;
        movement.Normalize();
        if (_isGrounded && movement != Vector3.zero)
        {
            if(_isRunning)
            {
                _moveSpeed = runSpeed;
                _anim.SetTrigger("run");
            }
            if (!_isRunning)
            {
                _moveSpeed = walkSpeed;
                _anim.SetTrigger("walk");
            }
        }
        if(movement == Vector3.zero)
        {
            _anim.SetTrigger("idle");
        }
        _playerParent.transform.Translate(movement * _moveSpeed * Time.deltaTime, Space.World);
    }

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
