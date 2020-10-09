﻿using System;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{    
    public enum RequiredCollectibleType { None, Keycard, }
    
    private bool _isPlayer = false;
    [SerializeField] private MovingPlatform _platformToControl = null;

    public enum CallToPos
    {
        WaypointA,
        WaypointB
    }
    [SerializeField] private bool _onTheElevator = default;
    [SerializeField] private CallToPos _callToPos = default;
    private Transform _targetPos = default;
    private bool _accessGranted = false;
    
    [Header("Require item to Operate?")]
    [SerializeField] private bool _itemRequired = false;
    [SerializeField] private RequiredCollectibleType _requiredCollectibleType = RequiredCollectibleType.None;
    [SerializeField] private string _identifierToAccess = "";

    private void Start()
    {
        switch (_callToPos)
        {
            case CallToPos.WaypointA:
                _targetPos = _platformToControl.GetTargetPos(true);
                break;
            case CallToPos.WaypointB:
                _targetPos = _platformToControl.GetTargetPos(false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _isPlayer = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!_isPlayer) return;
        if (!Input.GetKeyDown(KeyCode.E)) return;
        if (!_itemRequired)
        {
            GrantAccess();
            return;
        }
        CheckRequirements(other);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (_isPlayer) _isPlayer = false;
    }

    private void CheckRequirements(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (!player) return;
        _accessGranted = player.CheckCollectible(_requiredCollectibleType, _identifierToAccess);
        if (_accessGranted) GrantAccess();
        else Debug.Log("Access Denied");
    }

    private void GrantAccess()
    {
        //TODO: Add visual to show if access is granted or denied
        Debug.Log("Access granted");
        if (this._onTheElevator) _platformToControl.ButtonOnElevator();
        else _platformToControl.CallMovingPlatform(_targetPos);
    }
}
