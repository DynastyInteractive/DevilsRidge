using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    #region Attack Variables
    [Space(10)]
    [Header("Attack Variables")]
    [Tooltip("Minimum time to make a heavy attack")][SerializeField] float _heavyAttackMinTime = 0.5f;
    [Tooltip("Maximum time for an attack, which is when it will auto attack")][SerializeField] float _heavyAttackMaxTime = 2f;
    [Tooltip("A check to see if the player can attack")][SerializeField] bool _canAttack;
    [Tooltip("The damage amount")][SerializeField] float _damage = 1f;

    PlayerInput Input;
    float m_AttackHoldTimer;
    #endregion

    #region Combo Variables
    [Space(10)]
    [Header("Combo Variables")]
    [Tooltip("The number of attacks the player has done in the chain")] int m_comboAttackNum;
    [Tooltip("A check to see if the combo is active")] bool m_isComboActive;
    [Tooltip("The time for the player to attack again before combo fails")] float m_comboTimer = 10f;
    #endregion

    void Start()
    {
        _canAttack = true;
        m_isComboActive = false;
        m_comboAttackNum = 0;
        Input = new PlayerInput();
        Input.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_comboAttackNum > 0 && m_isComboActive)
        {
            m_comboTimer -= Time.deltaTime;
        }

        if(m_comboTimer <= 0 && m_isComboActive)
        {
            Debug.Log("Combo Failed");
            m_isComboActive = false;
            m_comboAttackNum = 0;
        }

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
            if(m_comboAttackNum > 0 && m_isComboActive)
            {
                Debug.Log("Combo Attack Number:" + m_comboAttackNum);
                _damage *= 1.2f + (m_comboAttackNum * 0.2f);
            }

            if (m_AttackHoldTimer < _heavyAttackMinTime)
            {
                Debug.Log("Light Attack");
                _canAttack = false;
                StartCoroutine(AttackCooldown(0.5f));
            }
            else if (m_AttackHoldTimer >= _heavyAttackMaxTime)
            {
                Debug.Log("Max Heavy Attack");
                _damage += 19f;
                StartCoroutine(AttackCooldown(3f));
            }
            else
            {
                Debug.Log("Weak Heavy Attack");
                _damage += m_AttackHoldTimer * 7.5f;
                StartCoroutine(AttackCooldown(1.5f));
            }

            Debug.Log("The damage hit was " + _damage);

            m_isComboActive = true;
            m_comboTimer = 5f;
            m_comboAttackNum++;
            m_AttackHoldTimer = 0;
            _damage = 1f;
        }
    }

    private IEnumerator AttackCooldown(float _timeToAttackAgain)
    {
        _canAttack = false;
        yield return new WaitForSeconds(_timeToAttackAgain);
        _canAttack = true;
    }
}


