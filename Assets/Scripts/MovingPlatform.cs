using System;
using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum MovementType
    {
        Automatic,
        Manual
    }
    [SerializeField] private MovementType _movementType = MovementType.Automatic;
    [SerializeField] private Transform _waypointA = null;
    [SerializeField] private Transform _waypointB = null;
    private Transform _currentTarget = null;
    private readonly WaitForSeconds _defaultWait = new WaitForSeconds(5.0f);
    private bool _canMove = false;
    [SerializeField] private float _distance;
    
    void Start()
    {
        if (_movementType == MovementType.Automatic)
        {
            _currentTarget = _waypointA;
            StartCoroutine(ConstantMovement());
        }

        if (_movementType == MovementType.Manual)
        {
            _currentTarget = _waypointA;
        }
    }

    private void Update()
    {
        _distance = Vector3.Distance(transform.position, _currentTarget.position); // For some reason does NOT work
    }

    private IEnumerator ConstantMovement()
    {
        //Debug.Log(Vector3.Distance(transform.position, _currentTarget.position));
        _distance = Vector3.Distance(transform.position, _currentTarget.position); // For some reason does NOT work
        _canMove = true;
        while (_canMove)
        {
            //float distance = Vector3.Distance(transform.position, _currentTarget.position);
            if (_distance < 0.1f)
            {
                ChangeTargetPosition();
                yield return _defaultWait;
            }

            transform.position = Vector3.MoveTowards(transform.position, _currentTarget.position, 3 * Time.deltaTime);
            yield return null;
        }
    }
    
    private IEnumerator MoveToTarget(Transform targetPos)
    {
        //TODO: Add option to enable / disable change of direction while in motion
        _canMove = true;
        while (_canMove)
        {
            float distance = Vector3.Distance(transform.position, targetPos.position);
            if (distance  < 0.1f)
            {
                ChangeTargetPosition();
                _canMove = false;
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPos.position, 3 * Time.deltaTime);
            yield return null;
        }
        
        yield return null;
    }

    public void CallMovingPlatform(Transform callToPos)
    {
        
        _currentTarget = callToPos;
        float distance = Vector3.Distance(transform.position, _currentTarget.position);
        if (distance > 0.1f)
        {
            StartCoroutine(MoveToTarget(callToPos));
        }
    }

    public void ButtonOnElevator()
    {
        float distance = Vector3.Distance(transform.position, _currentTarget.position);
        if (distance < 0.1f)
        {
            ChangeTargetPosition();
        }

        StartCoroutine(MoveToTarget(_currentTarget));
    }

    private void ChangeTargetPosition()
    {
        _currentTarget = _currentTarget == _waypointA ? _waypointB : _waypointA;
    }

    public Transform GetTargetPos(bool getPlatformA = true)
    {
        return getPlatformA ? _waypointA : _waypointB;
    }
}
