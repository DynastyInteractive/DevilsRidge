using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetSpawn : MonoBehaviour
{
    [SerializeField] Vector3 _spawnCoordinates = Vector3.zero;
    [SerializeField] Collider _playerCollider;

    void Awake()
    {
        _playerCollider.enabled = false;
    }


    void Start()
    {
        Invoke(nameof(SetPos), 0.2f);
    }

    void SetPos()
    {
        transform.position = _spawnCoordinates;
        _playerCollider.enabled = true;
    }
}
