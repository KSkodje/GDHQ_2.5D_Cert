using System;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{    
    public enum RequiredCollectibleType { Keycard, }
    [SerializeField] private RequiredCollectibleType _requiredCollectibleType;
    [SerializeField] private string _identifierToAccess;
    [SerializeField] private bool _access = false;
    private bool _isPlayer = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _isPlayer = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!_isPlayer) return;
        if (!Input.GetKeyDown(KeyCode.E)) return;

        var player = other.GetComponent<Player>();
        if (!player) return;
        _access = player.CheckCollectible(_requiredCollectibleType, _identifierToAccess);
        if (_access) AccessGranted();
        else Debug.Log("Access Denied");
    }

    private void AccessGranted()
    {
        //TODO: Add elevator functionality
        Debug.Log("Access Granted");
    }
}
