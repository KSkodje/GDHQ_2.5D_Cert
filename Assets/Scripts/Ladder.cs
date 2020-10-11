using System;
using UnityEngine;

[RequireComponent(typeof(Ledge))]
public class Ladder : MonoBehaviour
{
    private bool _isPlayer = false;
    [SerializeField] private Transform _topOfLadder = null;

    [Header("Solid = Can't pass through")]
    [SerializeField] private bool _isSolid = true;
    [SerializeField] private GameObject _solid = null;
    private Ledge _ledge;

    private void Start()
    {
        if (_solid) _solid.gameObject.SetActive(_isSolid);
        _ledge = GetComponent<Ledge>();
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
        if (player) player.StartLadderClimb(transform, _topOfLadder.position, _ledge);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (_isPlayer) _isPlayer = false;
    }
}
