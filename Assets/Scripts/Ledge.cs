using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Ledge : MonoBehaviour
{
    [SerializeField] private Transform _grabPos = default;
    [SerializeField] private Transform _standPos = default;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ledge_Grab_Checker"))
        {
            var player = GameObject.FindWithTag("Player").GetComponent<Player>();
            if (player) player.GrabLedge(_grabPos.position, this);
        }
    }

    public Vector3 GetStandPos()
    {
        return _standPos.position;
    }
}
