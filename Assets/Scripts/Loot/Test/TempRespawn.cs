using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempRespawn : MonoBehaviour
{
    [SerializeField] GameObject _enemyObject;
    [SerializeField] Button _killButton;

    public void Respawn()
    {
        GameObject enemy = Instantiate(_enemyObject,transform);
        _killButton.onClick.AddListener(enemy.GetComponent<TempKill>().Die);
    }
}
