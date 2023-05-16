using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : NetworkBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] float _heavyAttackMinTime = 0.5f;
    [SerializeField] float _heavyAttackMaxTime = 2f;
    [SerializeField] bool _canAttack;

    PlayerInput Input;

    float _damage = 1f;
    float m_AttackHoldTimer;

    void Start()
    {
        if (!IsOwner) return;
        _canAttack = true;
        Input = new PlayerInput();
        Input.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        Attack();
    }

    void Attack()
    {
        if (Input.Player.Attack.WasPressedThisFrame())
        {
            _damage = _player.Strength;
        }

        if (Input.Player.Attack.IsPressed())
        {
            m_AttackHoldTimer += Time.deltaTime;
        }

        if ((Input.Player.Attack.WasReleasedThisFrame() || m_AttackHoldTimer >= _heavyAttackMaxTime + 0.5f) && _canAttack)
        {
            if (m_AttackHoldTimer < _heavyAttackMinTime)
            {
                Debug.Log("Light Attack");
            }
            else if (m_AttackHoldTimer >= _heavyAttackMaxTime)
            {
                Debug.Log("Max Heavy Attack");
                _damage *= 2;
            }
            else
            {
                Debug.Log("Weak Heavy Attack");
                _damage *= 1 + (m_AttackHoldTimer * 0.5f);
            }

            Debug.Log("The damage hit was " + Mathf.FloorToInt(_damage));

            _canAttack = false;
            m_AttackHoldTimer = 0;
            _damage = 1f;
        }

        if (Input.Player.Attack.WasReleasedThisFrame()) _canAttack = true;
    }
}
