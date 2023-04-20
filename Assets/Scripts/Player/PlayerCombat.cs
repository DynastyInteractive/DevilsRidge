using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    /*[SerializeField] string[,] m_attackCombos = { {"light", "light", "light"}, {"heavy", "heavy", "empty"} };
    [SerializeField] string[, 3] m_availableCombos = { { } };
    [SerializeField] string[] m_currentCombo = {"", "", ""};*/

    [SerializeField] List<List<string>> m_attackCombos = new List<List<string>>
    {
        new List<string> {"light", "light", "light"},
        new List<string> {"heavy", "heavy", "empty"}
    };
    [SerializeField] List<List<string>> m_availableCombos = new List<List<string>> { };
    [SerializeField] List<string> m_currentCombo = new List<string> { };

    int m_currentComboNum = 0;
    bool comboAvailable = false;
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

        m_currentCombo.Add(attackType); //adds the attack to the current combo list

        for (int i = 0; i < m_attackCombos.Count + 1; i++)
        {
            Debug.Log(m_attackCombos[i][m_currentComboNum]);


            if (m_attackCombos[i][m_currentComboNum] == attackType)
            {
                comboAvailable = true;
                m_availableCombos.Add(new List<string> { m_attackCombos[i][0], m_attackCombos[i][1], m_attackCombos[i][2] });
            }
                        
            if (comboAvailable)
            {
                i = 0;
                break;
            }
        }
        for (int i = 0; i < m_availableCombos.Count; i++)
        {
            if (i > m_availableCombos.Count) break;

            if (m_availableCombos[i][m_currentComboNum] != attackType)
            {
                comboAvailable = false;
                m_availableCombos.RemoveAt(i);
            }

            for (int x = 0; x < m_availableCombos.Count; x++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Debug.Log(m_availableCombos[x][j]);
                }
            }

            if (!comboAvailable)
            {
                m_currentCombo.RemoveAt(m_currentComboNum);
                m_currentCombo[m_currentComboNum - 1] = attackType;
                m_currentComboNum--;
                break;
            }
        }
        
        m_currentComboNum++;
        
        if (m_currentComboNum == 3 || m_availableCombos[0][m_currentComboNum+1] == "empty")
        {
            Debug.Log("COMBO!");
            m_currentCombo.Clear();
            _damage *= 3;
        }

        
        m_availableCombos.Clear();

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
