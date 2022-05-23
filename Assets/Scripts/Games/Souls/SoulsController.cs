using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

        private bool queuedAttack = false;
        private bool queuedDodge = false;
        
        public float moveSpeed = 2.0f;
        public float sprintSpeed = 5.335f;
        public float speedChangeRate = 10.0f;
        public float dodgeSpeed = 5f;
        
        private float speed;
        private float animationBlend;

        public CharacterController controller;
        private Vector3 dodgeDirection = Vector3.zero;

        public SoulsManager soulsManager;
        public SoulStat health;
        public SoulStat stamina;

        public float attackStaminaUsage = 10f;
        public float attackDamage = 5f;


        public bool canPlay = false;

        public Weapon weapon;

        public void SetupPlayer(float maxStamina, float maxHealth, float healthRegenSpeed, float staminaRegenSpeed)
        {
            health = new SoulStat(maxHealth, healthRegenSpeed);
            stamina = new SoulStat(maxStamina, staminaRegenSpeed);

            canPlay = true;
            weapon.Setup(attackDamage);
        }

        // Update is called once per frame
        void Update()
        {
            if (!canPlay) return;
            if(!isAttacking && !isDodging) 
                Move();
            Jump();
            Attack();
            Dodge();
            if (isDodging)
            {
                Vector3 targetDirection = (dodgeDirection.x * transform.right) + (dodgeDirection.z * transform.forward);
                // move the player
                controller.Move(targetDirection * (dodgeSpeed * Time.deltaTime));
            }
            anim.SetBool("Sprint", input.sprint);
        }

        private void FixedUpdate()
        {
            
            //Regen
            health.RegenStat(Time.fixedDeltaTime);
            stamina.RegenStat(Time.fixedDeltaTime);
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

            float speedOffset = 0.1f;
            float inputMagnitude = input.move.magnitude;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * speedChangeRate);

                // round speed to 3 decimal places
                speed = Mathf.Round(speed * 1000f) / 1000f;
            }
            else
            {
                speed = targetSpeed;
            }

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
            if (input.attack)
            {
                input.attack = false;
                if (!isAttacking)
                {
                    anim.SetBool(animID_Attack, true);
                    stamina.RemoveStat(15f);
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
                else
                {
                    queuedDodge = true;
                }
            }
        }

        void AttackOn()
        {
            //ToDo: Turn on weapon collision
            weapon.AttackOn();
        }

        void AttackOff()
        {
            weapon.AttackOff();
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
                if (input.move.y > deadzone)
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

        public void TakeDamage(float damage, Vector3 hitPoint)
        {
            health.RemoveStat(damage, 0.25f);
            if (health.currentValue <= 0)
            {
                soulsManager.PlayerDied();
            }
        }
    }
}