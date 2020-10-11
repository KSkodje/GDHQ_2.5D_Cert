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

    [SerializeField] private List<string> _keyCards = new List<string>();
    private bool _climbingLadder = false;
    private Vector3 _ladderTop = Vector3.zero;

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
        if (_climbingLadder)
        {
            /*if (Input.GetKeyDown(KeyCode.Space))
            {
                _climbingLadder = false;
                _anim.SetBool("LadderClimb", _climbingLadder);
                _grabbingLedge = false;
                _jumping = true;
            }*/
            LadderClimb();
            return;
        }
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
            if (_hInput != 0 && !_grabbingLedge)
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

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _anim.SetTrigger("Rolling");
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
        else if (!_grabbingLedge && !_climbingLadder)
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
    }

    public void StandUpComplete()
    {
        _grabbingLedge = false;
        _anim.SetBool("GrabbingLedge", _grabbingLedge);
    }

    public void PickupCollectible(Collectible.CollectibleType collectibleType, string identifier)
    {
        if (collectibleType == Collectible.CollectibleType.Keycard)
        {
            if (_keyCards.Contains(identifier)) return;
            _keyCards.Add(identifier);
        }
    }

    public bool CheckCollectible(ControlPanel.RequiredCollectibleType requiredCollectibleType, string requiredIdentifier)
    {
        if (requiredCollectibleType == ControlPanel.RequiredCollectibleType.Keycard)
        {
            if (_keyCards.Contains(requiredIdentifier)) return true;
        }
        
        return false;
    }

    public void StartLadderClimb(Transform ladderPos, Vector3 ladderTop, Ledge currentLedge)
    {
        // Animations could possibly be refined by using Behavior scripts to place Player
        
        //(-0.3, 40.9, 145.5) - Ladder: (-0.1, 40.6, 146.5)
        // distance should be about -1f from left side
        Vector3 playerPos = transform.position;
        _climbingLadder = true;
        float clingDistance = 1f;
        float distance = playerPos.z - ladderPos.position.z;
        Debug.Log(distance);
        if (distance < 0)
        {
            transform.position = new Vector3(playerPos.x,playerPos.y, ladderPos.position.z - clingDistance);
        }

        if (distance > 0)
        {
            transform.position = new Vector3(playerPos.x,playerPos.y, ladderPos.position.z + clingDistance);
        }
        
        transform.position += new Vector3(0,0,0);
        _anim.SetBool("LadderClimb", _climbingLadder);
        _activeLedge = currentLedge;
        
        _ladderTop = new Vector3(ladderTop.x, ladderTop.y, playerPos.z);
        
        //TODO: Add logic to fix facing and to allow limiting which direction can be climbed
    }

    public void LadderClimb()
    {
        if (_ledgegrabChecker.position.y >= _ladderTop.y)
        {
            _climbingLadder = false;
            _anim.SetBool("LadderClimb", _climbingLadder);
            _grabbingLedge = true;
        }

        transform.position = Vector3.MoveTowards(transform.position, _ladderTop, 3 * Time.deltaTime);
    }
}
