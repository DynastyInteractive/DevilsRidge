using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempRespawn : MonoBehaviour
{
    [SerializeField] GameObject _enemyObject;
    [SerializeField] GameObject _killButton;

    public void Respawn()
    {
        GameObject enemy = Instantiate(_enemyObject,transform);
        Button button = _killButton.GetComponent<Button>();
        button.onClick.AddListener(enemy.GetComponent<TempKill>().Die);
    }
}
