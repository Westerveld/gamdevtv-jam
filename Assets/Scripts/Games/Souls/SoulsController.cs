using System;
using System.Collections;
using System.Collections.Generic;
using Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Souls
{
    public class SoulsController : MonoBehaviour, IDamagable
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
        private int animID_Hit1 = Animator.StringToHash("Hit1");
        private int animID_Hit2 = Animator.StringToHash("Hit2");
        

        private bool queuedAttack = false;
        //private bool queuedDodge = false;
        
        public float moveSpeed = 2.0f;
        public float sprintSpeed = 5.335f;
        public float speedChangeRate = 10.0f;
        public float dodgeSpeed = 5f;
        
        private float speed;
        private float animationBlend;

        public CharacterController controller;
        private Vector3 dodgeDirection = Vector3.zero;

        public SoulsManager soulsManager;
        public Stat health;
        public Stat stamina;

        public float attackStaminaUsage = 20f;
        public float attackDamage = 5f;
        public float dodgeStaminaUsage = 15f;


        public bool canPlay = false;

        public Weapon weapon;

        public UISoulsManager m_UISoulsManager;

        public void SetupPlayer(float maxStamina, float maxHealth, float healthRegenSpeed, float staminaRegenSpeed)
        {
            health = new Stat(maxHealth, healthRegenSpeed);
            stamina = new Stat(maxStamina, staminaRegenSpeed);

            m_UISoulsManager.SetPlayerMaxHealth(maxHealth);
            m_UISoulsManager.SetPlayerMaxStamina(maxStamina);

            canPlay = true;
            weapon.Setup(attackDamage);
        }

        // Update is called once per frame
        void Update()
        {
            if (!canPlay)
            {
                ResetInput();
                return;
            }
            if(!isAttacking && !isDodging) 
                Move();
            Jump();
            Attack();
            Dodge();
        }

        void ResetInput()
        {
            input.attack = input.sprint = input.dodge = input.jump = false;
            input.move = Vector2.zero;
        }
        private void FixedUpdate()
        {
            if (!canPlay) return;
            if (isDodging)
            {
                Vector3 targetDirection = (dodgeDirection.x * transform.right) + (dodgeDirection.z * transform.forward);
                // move the player
                controller.Move(targetDirection * (dodgeSpeed * Time.deltaTime));
            }
            anim.SetBool("Sprint", input.sprint);
            //Regen
            //health.RegenStat(Time.fixedDeltaTime);
            stamina.RegenStat(Time.fixedDeltaTime);
            m_UISoulsManager.SetPlayerStamina(stamina.currentValue);
        }

        private void LateUpdate()
        {
            LookAtBoss();
        }

        void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = input.sprint ? sprintSpeed : moveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (input.move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;


            speed = targetSpeed;

            animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
            if (animationBlend < 0.01f) animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

            if (input.move != Vector2.zero)
            {
                
            }

            Vector3 targetDirection = (inputDirection.x * transform.right) + (inputDirection.z * transform.forward);
            // move the player
            controller.Move(targetDirection * (speed * Time.deltaTime));

            anim.SetFloat(animID_Hor, input.move.x);
            anim.SetFloat(animID_Ver, input.move.y);
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
            if (!isAttacking && queuedAttack)
            {
                if (stamina.currentValue >= attackStaminaUsage)
                {
                    anim.SetBool(animID_Attack, true);
                    isAttacking = true;
                    queuedAttack = false;
                    input.attack = false;
                    return;
                }
            }
            if (input.attack)
            {
                input.attack = false;
                if (!isAttacking)
                {
                    if (stamina.currentValue >= attackStaminaUsage)
                    {
                        anim.SetBool(animID_Attack, true);
                        stamina.RemoveStat(attackStaminaUsage);
                        isAttacking = true;
                    }
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
                if (!isDodging && stamina.currentValue > dodgeStaminaUsage)
                {
                    stamina.RemoveStat(dodgeStaminaUsage);
                    isDodging = true;
                    if (input.move.y > deadzone)
                    {
                        anim.SetTrigger(animID_Dodge);
                        dodgeDirection = Vector3.forward;
                    }
                    else if (input.move.x < 0)
                    {
                        anim.SetTrigger(animID_DodgeLeft);
                        dodgeDirection = Vector3.left;
                    }
                    else if (input.move.x > 0)
                    {
                        anim.SetTrigger(animID_DodgeRight);
                        dodgeDirection = Vector3.right;
                    }
                    else
                    {
                        anim.SetTrigger(animID_DodgeBack);
                        dodgeDirection = Vector3.back;
                    }
                }
            }
        }

        void AttackOn()
        {
            //ToDo: Turn on weapon collision
            isDodging = false;
            weapon.AttackOn();
            stamina.RemoveStat(attackStaminaUsage);
        }

        void AttackOff()
        {
            weapon.AttackOff();
            /*if (input.attack || queuedAttack)
            {
                anim.SetBool(animID_Attack, true);
                input.attack = false;
                queuedAttack = false;
            }
            else
            {
                anim.SetBool(animID_Attack, false);
                isAttacking = false;
            }*/
            anim.SetBool(animID_Attack, false);
            isAttacking = false;
            Attack();
        }

        void DodgeOff()
        {
            isDodging = false;
        }

        void LookAtBoss()
        {
            Vector3 direction = boss.transform.position - transform.position;
            Vector3 rotateVector = Quaternion.LookRotation(direction).eulerAngles;
            rotateVector.x = rotateVector.z = 0;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(rotateVector), 1);
        }

        public void TakeDamage(float damage, Vector3 hitPoint)
        {
            if (isDodging)
            {
                //ToDo: vfx for dodging?
                return;
            }
            
            anim.SetTrigger(Random.value > 0.5f ? animID_Hit1 : animID_Hit2);
            health.RemoveStat(damage, 0.5f);
            m_UISoulsManager.SetPlayerHealth(health.currentValue);
            if (health.currentValue <= 0)
            {
                soulsManager.PlayerDied();
            }
        }
    }
}