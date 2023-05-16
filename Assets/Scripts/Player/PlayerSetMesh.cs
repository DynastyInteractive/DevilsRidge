using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSetMesh : NetworkBehaviour
{
    [SerializeField] GameObject _fullMesh;
    [SerializeField] GameObject _armFPSMesh;

    // Start is called before the first frame update
    void Start()
    {
        if (IsOwner)
        {
            _fullMesh.SetActive(false);
            _armFPSMesh.SetActive(true);
        }
        if (!IsOwner)
        {
            _fullMesh.SetActive(true);
            _armFPSMesh.SetActive(false);
        }
    }
}
