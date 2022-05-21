using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoulsController : MonoBehaviour
{
    public SoulsInput input;

    public bool isAttacking;
    public bool isDodging;

    public Transform boss;

    public Animator anim;

    private int animID_Hor = Animator.StringToHash("Horizontal");
    private int animID_Ver = Animator.StringToHash("Vertical");
    private int animID_Attack = Animator.StringToHash("Attack");
    private int animID_Dodge = Animator.StringToHash("Dodge");
    private int animID_Jump = Animator.StringToHash("Jump");

    private bool queuedAttack = false;
    
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttacking && !isDodging)
        {
            Move();
            Jump();
        }
        
        Attack();
        Dodge();
        
    }

    private void LateUpdate()
    {
        LookAtBoss();
    }

    void Move()
    {
        anim.SetFloat(animID_Hor, input.move.x);
        anim.SetFloat(animID_Ver, input.move.y);
    }

    void Jump()
    {
        
    }

    void Attack()
    {
        if (input.attack)
        {
            input.attack = false;
            if (!isAttacking)
            {
                anim.SetTrigger(animID_Attack);
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
            anim.SetTrigger(animID_Dodge);
        }
    }

    void AttackOn()
    {
        
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

    void LookAtBoss()
    {
        Vector3 direction = boss.transform.position - transform.position;
        Vector3 rotateVector = Quaternion.LookRotation(direction).eulerAngles;
        rotateVector.x = rotateVector.z = 0;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(rotateVector), 1);
    }
}


