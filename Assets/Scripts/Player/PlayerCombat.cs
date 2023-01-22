using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    private PlayerInput Input;

    private float attackHoldTimer;
    [SerializeField] private float heavyAttackMinTime = 0.5f;
    [SerializeField] private float heavyAttackMaxTime = 2f;
    [SerializeField] private bool canAttack;

    [SerializeField] private float damage = 1f;

    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        canAttack = true;
        slider.minValue = heavyAttackMinTime;
        slider.value = 0;
        Input = new PlayerInput();
        Input.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        slider.value = attackHoldTimer / heavyAttackMaxTime;

        if(attackHoldTimer >= heavyAttackMinTime)
        {
            slider.gameObject.SetActive(true);
        }
        else
        {
            slider.gameObject.SetActive(false);
        }
    }

    private void Attack()
    {
        if (canAttack)
        {
            if (Input.Player.Attack.IsPressed())
            {
                attackHoldTimer += Time.deltaTime;
            }

            if (Input.Player.Attack.WasReleasedThisFrame() || attackHoldTimer >= heavyAttackMaxTime + 0.5f)
            {
                if (attackHoldTimer < heavyAttackMinTime)
                {
                    Debug.Log("Light Attack");
                }
                else if (attackHoldTimer >= heavyAttackMaxTime)
                {
                    Debug.Log("Max Heavy Attack");
                    damage += 19f;
                }
                else
                {
                    Debug.Log("Weak Heavy Attack");
                    damage += attackHoldTimer * 7.5f;
                }

                Debug.Log("The damage hit was " + Mathf.FloorToInt(damage));

                canAttack = false;
                attackHoldTimer = 0;
                damage = 1f;
            }
        }

        if (Input.Player.Attack.WasReleasedThisFrame())
        {
            canAttack = true;
        }
    }
}
