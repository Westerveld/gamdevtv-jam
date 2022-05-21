using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoulsController : MonoBehaviour
{
    public SoulsInput input;
    private Vector2 moveInput;

    public bool isAttacking;
    public bool isDodging;

    public Transform boss;

    public Animator anim;
    public float animLerpSpeed = 2f;
    public float deadzone = 0.1f;

    private int animID_Hor = Animator.StringToHash("Horizontal");
    private int animID_Ver = Animator.StringToHash("Vertical");
    private int animID_Attack = Animator.StringToHash("Attack");
    private int animID_Jump = Animator.StringToHash("Jump");
    private int animID_Dodge = Animator.StringToHash("Dodge");
    private int animID_DodgeBack = Animator.StringToHash("DodgeBack");
    private int animID_DodgeLeft = Animator.StringToHash("DodgeLeft");
    private int animID_DodgeRight = Animator.StringToHash("DodgeRight");

    private bool queuedAttack = false;
    private bool queuedDodge = false;
    
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        Attack();
        Dodge();
        anim.SetBool("Sprint", input.sprint);
    }

    private void LateUpdate()
    {
        LookAtBoss();
    }

    void Move()
    {
        anim.SetFloat(animID_Hor, input.move.normalized.x);
        anim.SetFloat(animID_Ver, input.move.normalized.y);
    }

    void Jump()
    {
        if (input.jump)
        {
            input.jump = false;
            anim.SetTrigger(animID_Jump);
        }
    }

    void Attack()
    {
        if (input.attack)
        {
            input.attack = false;
            if (!isAttacking)
            {
                anim.SetBool(animID_Attack, true);
                isAttacking = true;
            }
            else
            {
                queuedAttack = true;
            }
        }
    }

    void Dodge()
    {
        if (input.dodge)
        {
            input.dodge = false;
            if (!isDodging)
            {
                isDodging = true;
                if(input.move.y > deadzone)
                {
                    anim.SetTrigger(animID_Dodge);
                }
                else if (input.move.x < 0)
                {
                    anim.SetTrigger(animID_DodgeLeft);
                }
                else if (input.move.x > 0)
                {
                    anim.SetTrigger(animID_DodgeRight);
                }
                else
                {
                    anim.SetTrigger(animID_DodgeBack);
                }
            }
            else
            {
                queuedDodge = true;
            }
        }
    }

    void AttackOn()
    {
        //ToDo: Turn on weapon collision
    }

    void AttackOff()
    {
        if (input.attack || queuedAttack)
        {
            anim.SetBool(animID_Attack, true);
            input.attack = false;
            queuedAttack = false;
        }
        else
        {
            anim.SetBool(animID_Attack, false);
            isAttacking = false;
        }
    }

    void DodgeOff()
    {
        if (input.dodge || queuedDodge)
        {
            if(input.move.y > deadzone)
            {
                anim.SetTrigger(animID_Dodge);
            }
            else if (input.move.x < 0)
            {
                anim.SetTrigger(animID_DodgeLeft);
            }
            else if (input.move.x > 0)
            {
                anim.SetTrigger(animID_DodgeRight);
            }
            else
            {
                anim.SetTrigger(animID_DodgeBack);
            }
            input.dodge = false;
            queuedDodge = false;
        }
        else
        {
            isDodging = false;
        }
    }

    void LookAtBoss()
    {
        Vector3 direction = boss.transform.position - transform.position;
        Vector3 rotateVector = Quaternion.LookRotation(direction).eulerAngles;
        rotateVector.x = rotateVector.z = 0;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(rotateVector), 1);
    }
}


