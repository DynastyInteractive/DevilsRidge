using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    #region Attack Variables
    [SerializeField] float _heavyAttackMinTime = 0.5f;
    [SerializeField] float _heavyAttackMaxTime = 2f;
    [SerializeField] bool _canAttack;
    [SerializeField] float _damage = 1f;

    PlayerInput Input;
    float m_AttackHoldTimer;
    #endregion

    #region Combo Variables
    [SerializeField] string[,] m_attackCombos = { {"light", "light", "light"}, {"heavy", "heavy", "empty"} };
    [SerializeField] string[] m_currentCombo = {"", "", ""};

    int m_currentComboNum = 0;
    #endregion

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
                ComboChecker("light");
                _canAttack = false;
                StartCoroutine(AttackCooldown(0.5f));
            }
            else if (m_AttackHoldTimer >= _heavyAttackMaxTime)
            {
                Debug.Log("Max Heavy Attack");
                ComboChecker("heavy");
                _damage += 19f;
                StartCoroutine(AttackCooldown(3f));
            }
            else
            {
                Debug.Log("Weak Heavy Attack");
                ComboChecker("heavy");
                _damage += m_AttackHoldTimer * 7.5f;
                StartCoroutine(AttackCooldown(1.5f));
            }

            Debug.Log("The damage hit was " + Mathf.FloorToInt(_damage));

            m_AttackHoldTimer = 0;
            _damage = 1f;
        }
    }

    void ComboChecker(string attackType)
    {
        int attackCombo_num1 = 0;
        int attackCombo_num2 = 0;

        m_currentCombo[m_currentComboNum] = attackType;
        m_currentComboNum++;



        /*for (int i = 0; i < m_currentCombo.Length; i++)
        {
            Debug.Log(i);
            if (m_currentCombo[i] == m_attackCombos[attackCombo_num1, attackCombo_num2] || m_attackCombos[attackCombo_num1, attackCombo_num2] == "empty")
            {
                attackCombo_num2++;
            }
            else
            {
                if (attackCombo_num1 == (m_attackCombos.Length / 3)) return;

                i = 0;
                attackCombo_num1++;
                attackCombo_num2 = 0;
            }

            if (attackCombo_num2 == 3 || m_attackCombos[attackCombo_num1, attackCombo_num2++] == "empty")
            {
                Debug.Log("COMBO!");
                _damage *= 3;
                i = m_currentCombo.Length;
            }
        }

        for(int j = 0; j < m_currentCombo.Length; j++)
        {
            m_currentCombo[j] = "";
        }*/
    }

    private IEnumerator AttackCooldown(float _timeToAttackAgain)
    {
        _canAttack = false;
        yield return new WaitForSeconds(_timeToAttackAgain);
        _canAttack = true;
    }
}
