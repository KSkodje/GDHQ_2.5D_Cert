using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    private CharacterController _controller;
    private Animator _anim;
    private Ledge _activeLedge = null;
    
    [SerializeField] private Transform _ledgegrabChecker = null;
    
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private float _jumpHeight = 3.0f;
    private const float Gravity = -9.81f;
    private Vector3 _direction, _velocity;
    private bool _isGrounded;
    private bool _jumping = false;

    private float _hInput;
    [SerializeField] private bool _grabbingLedge = false;

    void Start()
    {
        Application.targetFrameRate = 60;
        _controller = GetComponent<CharacterController>();
        _anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        
        // No movement while mid-jump
        
        if (_controller.isGrounded)
        {
            if (_jumping)
            {
                _jumping = false;
                _anim.SetBool("Jumping", _jumping);
            }
            
            _hInput = Input.GetAxisRaw("Horizontal");
            _direction = new Vector3(0, 0, _hInput);
            _anim.SetFloat("Speed", Mathf.Abs(_hInput));

            
            // Flip != mirror
            if (_hInput != 0)
            {
                Vector3 facing = transform.localEulerAngles;
                facing.y = _hInput < 0 ? 180 : 0;
                transform.localEulerAngles = facing;
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _direction.y += _jumpHeight;
                _jumping = true;
                _anim.SetBool("Jumping", _jumping);
            }
        }

        
        
        _direction.y += Gravity * Time.deltaTime;

        if (_grabbingLedge)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _anim.SetTrigger("ClimbUp");
            }
        }
        else if (!_grabbingLedge)
        {
            _controller.Move(_direction * _speed * Time.deltaTime);
        }
    }

    public void GrabLedge(Vector3 handPos, Ledge currentLedge)
    {
        _grabbingLedge = true;
        _anim.SetBool("GrabbingLedge", _grabbingLedge);
        transform.position += handPos - _ledgegrabChecker.position;
        _anim.SetFloat("Speed", 0.0f);
        _anim.SetBool("Jumping", false);

        _activeLedge = currentLedge;
    }


    public void ClimbUpComplete()
    {
        transform.position = _activeLedge.GetStandPos();
        _grabbingLedge = false;
        _anim.SetBool("GrabbingLedge", _grabbingLedge);
    }
}
