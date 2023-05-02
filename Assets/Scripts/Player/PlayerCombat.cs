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
        //Sets all the variables needed to their defaults at the start of runtime
        _canAttack = true;
        m_isComboActive = false;
        m_comboAttackNum = 0;
        //Gets the new input system and enables it
        Input = new PlayerInput();
    }

    //enables the input system when the code is enabled, ie at the start of runtime
    private void OnEnable()
    {
        Input.Enable();
    }

    //disables the input system when the code is disabled
    private void OnDisable()
    {
        Input.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        //if the combo is currently enabled, reduced the timer to when it ends
        if(m_comboAttackNum > 0 && m_isComboActive)
        {
            m_comboTimer -= Time.deltaTime;
        }

        //disables the combo when the timer runs out
        if(m_comboTimer <= 0 && m_isComboActive)
        {
            Debug.Log("Combo Failed");
            m_isComboActive = false;
            m_comboAttackNum = 0;
        }

        //runs the attack method every frame to check for attacks
        Attack();
    }

    void Attack()
    {
        //if the player can't attack, it returns from the method
        if (!_canAttack) return;

        //if the player attacks, increase the time float that its held for
        if (Input.Player.Attack.IsPressed())
        {
            m_AttackHoldTimer += Time.deltaTime;
        }

        //if the attack control is released OR the max time for the attack held is reached, run this
        if (Input.Player.Attack.WasReleasedThisFrame() || m_AttackHoldTimer >= _heavyAttackMaxTime + 0.5f)
        {
            //adds the attack to the combo
            if(m_comboAttackNum > 0 && m_isComboActive)
            {
                Debug.Log("Combo Attack Number:" + m_comboAttackNum);
                _damage *= 1.2f + (m_comboAttackNum * 0.2f);
            }

            //if the attack doesn't reach the heavy attack threshold, it runs a light attack
            if (m_AttackHoldTimer < _heavyAttackMinTime)
            {
                Debug.Log("Light Attack");
                _canAttack = false;
                StartCoroutine(AttackCooldown(0.5f));
            }
            //if the attack reaches the max attack time, it runs a max heavy attack
            else if (m_AttackHoldTimer >= _heavyAttackMaxTime)
            {
                Debug.Log("Max Heavy Attack");
                _damage += 19f;
                StartCoroutine(AttackCooldown(3f));
            }
            //if neither previous if statements are true, runs a weak heavy attack
            else
            {
                Debug.Log("Weak Heavy Attack");
                _damage += m_AttackHoldTimer * 7.5f;
                StartCoroutine(AttackCooldown(1.5f));
            }

            //outputs damage and resets needed variables that have been changeds
            Debug.Log("The damage hit was " + _damage);
            m_isComboActive = true;
            m_comboTimer = 5f;
            m_comboAttackNum++;
            m_AttackHoldTimer = 0;
            _damage = 1f;
        }
    }

    //IEnumerator allows for the code to be paused for a set time
    private IEnumerator AttackCooldown(float _timeToAttackAgain)
    {
        //stops the player from attacking, waits for the set time, and then allows them again after
        _canAttack = false;
        yield return new WaitForSeconds(_timeToAttackAgain);
        _canAttack = true;
    }
}


