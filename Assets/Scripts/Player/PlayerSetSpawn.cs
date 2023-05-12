using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetSpawn : MonoBehaviour
{
    [SerializeField] Vector3 _spawnCoordinates = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(SetPos), 0.2f);
    }

    void SetPos()
    {
        transform.position = _spawnCoordinates;
    }
}
