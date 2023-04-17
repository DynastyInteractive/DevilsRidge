using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] float _heavyAttackMinTime = 0.5f;
    [SerializeField] float _heavyAttackMaxTime = 2f;
    [SerializeField] bool _canAttack;
    [SerializeField] float _damage = 1f;

    PlayerInput Input;

    float m_AttackHoldTimer;


    void Start()
    {
        _canAttack = true;
        Input = new PlayerInput();
        Input.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    void Attack()
    {
        if (!_canAttack) return;

        if (Input.Player.Attack.IsPressed())
        {
            m_AttackHoldTimer += Time.deltaTime;
        }

        if (Input.Player.Attack.WasReleasedThisFrame() || m_AttackHoldTimer >= _heavyAttackMaxTime + 0.5f)
        {
            if (m_AttackHoldTimer < _heavyAttackMinTime)
            {
                Debug.Log("Light Attack");
            }
            else if (m_AttackHoldTimer >= _heavyAttackMaxTime)
            {
                Debug.Log("Max Heavy Attack");
                _damage += 19f;
            }
            else
            {
                Debug.Log("Weak Heavy Attack");
                _damage += m_AttackHoldTimer * 7.5f;
            }

            Debug.Log("The damage hit was " + Mathf.FloorToInt(_damage));

            _canAttack = false;
            m_AttackHoldTimer = 0;
            _damage = 1f;
        }

        if (Input.Player.Attack.WasReleasedThisFrame())
        {
            _canAttack = true;
        }

    }
}
