using System;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private bool _isPlayer = false;
    [Header("Solid = Can't pass through")]
    [SerializeField] private bool _isSolid = true;
    [SerializeField] private GameObject _solid = null;

    private void Start()
    {
        if (_solid) _solid.gameObject.SetActive(_isSolid);
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
        var player = other.GetComponent<Player>();
        player.LadderClimb();

    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (_isPlayer) _isPlayer = false;
    }
}
