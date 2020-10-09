using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum CollectibleType { Keycard, }
    [SerializeField] private CollectibleType _collectibleType;
    [SerializeField] private string _identifier;

    [SerializeField] private float _rotationSpeed = 10f;
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 1, 0) * _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player)
            {
                player.PickupCollectible(_collectibleType, _identifier);
            }
            Destroy(gameObject);
        }
    }
}
