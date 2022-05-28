using System;
using System.Collections;
using System.Collections.Generic;
using Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Souls
{
    public class SoulsBoss : MonoBehaviour, IDamagable
    {
        public Stat health;
        public float maxHealth = 300f;

        public Animator anim;
        public CharacterController controller;

        public Transform player;

        private readonly int animID_Roar = Animator.StringToHash("Roar");
        private readonly int animID_Walk = Animator.StringToHash("Walk");
        private readonly int animID_Run = Animator.StringToHash("Run");
        private readonly int animID_HeavyAttack_Left = Animator.StringToHash("HeavyAttack_Left");
        private readonly int animID_HeavyAttack_Right = Animator.StringToHash("HeavyAttack_Right");
        private readonly int animID_Jump = Animator.StringToHash("Jump");
        private readonly int animID_JumpAttack = Animator.StringToHash("JumpAttack");
        private readonly int animID_Combo = Animator.StringToHash("Combo");
        private readonly int animID_Dead = Animator.StringToHash("Dead");
        private readonly int animID_MotionSpeed = Animator.StringToHash("MotionSpeed");

        private readonly int animID_Hit1 = Animator.StringToHash("Hit1");
        private readonly int animID_Hit2 = Animator.StringToHash("Hit2");
        private readonly int animID_SmallLeft = Animator.StringToHash("SmallLeft");
        private readonly int animID_SmallRight = Animator.StringToHash("SmallRight");
        private readonly int animID_LargeRight = Animator.StringToHash("LargeRight");
        private readonly int animID_LargeLeft = Animator.StringToHash("LargeLeft");

        public GameObject smashAttackParticles;
        public GameObject jumpAttackParticles;

        private int currentCombo = 0;
        private float stateTime = 0f;
        private float rotationTimer = 5f;

        public BossPhase[] phases;
        public BossPhase currentPhase;
        private int phasePart = 0;

        public Weapon[] hands;

        private bool canPlay = false;

        public float timeBetweenPhases = 5f;
        private float phaseTimer;
        private bool waitingBetweenPhases;

        private BossState state;

        private SoulsManager manager;

        public float turnSpeed = 0.25f;

        public float walkSpeed = 2f;
        public float runSpeed = 5.5f;
        public float attackMoveDistance = 0.25f;
        public float speedChangeRate = 5.0f;
        private float speed;
        public int damage = 15;

        public UISoulsManager m_UISoulsManager;

        public Weapon jumpAttackWeapon;

        private Coroutine walk;

        private void Awake()
        {
            anim.GetBehaviour<IdleState>().boss = this;
            var rotates = anim.GetBehaviours<RotateBoss>();
            foreach (var rotate in rotates)
                rotate.boss = this;
        }

        public void Setup(float healthValue, SoulsManager m)
        {
            manager = m;
            health = new Stat(maxHealth, 0f, healthValue);
            m_UISoulsManager.SetBossMaxHealth(maxHealth);
            m_UISoulsManager.SetBossHealth(health.currentValue);
            anim.GetBehaviour<IdleState>().boss = this;
            player = m.player.transform;
            PickNextPhase();
            canPlay = true;
            for (int i = 0; i < hands.Length; ++i)
            {
                hands[i].Setup(damage);
            }
        }
        
        public void TakeDamage(float damage, Vector3 normal)
        {
            normal.y = 0f;
            transform.position += normal;
            health.RemoveStat(damage);
            m_UISoulsManager.SetBossHealth(health.currentValue);
            if (health.currentValue > 0)
            {
                anim.SetTrigger(Random.value > 0.5f ? animID_Hit1 : animID_Hit2);
            }
            else
            {
                canPlay = false;
                anim.SetTrigger(animID_Dead);
                StartCoroutine(WaitThenFinish());
            }
        }

        IEnumerator WaitThenFinish()
        {
            yield return new WaitForSeconds(3f);
            
            manager.BossDied();
        }

        void FixedUpdate()
        {
            if (!canPlay) return;
            if (waitingBetweenPhases)
            {
                phaseTimer -= Time.fixedDeltaTime;
                if (phaseTimer <= 0)
                {
                    waitingBetweenPhases = false;
                    TriggerPhase();
                }
            }
            else
            {
                ProgressPhase();
            }
            StateAdjustment();
        }

        void ProgressPhase()
        {
            stateTime += Time.fixedDeltaTime;
            if (currentPhase.mechanics[phasePart].length > 0)
            {
                if (stateTime > currentPhase.mechanics[phasePart].length)
                {
                    currentPhase.mechanics[phasePart].completed = true;
                    EndPhase();
                    phasePart++;
                    if (phasePart >= currentPhase.mechanics.Count)
                    {
                        PickNextPhase();
                    }
                    else
                    {
                        TriggerPhase();
                    }
                }
            }
            else if (state == BossState.Idle)
            {
                currentPhase.mechanics[phasePart].completed = true;
                EndPhase();
                phasePart++;
                if (phasePart >= currentPhase.mechanics.Count)
                {
                    PickNextPhase();
                }
                else
                {
                    TriggerPhase();
                }
            }
        }

        void StateAdjustment()
        {
            switch (state)
            {
                case BossState.Idle:
                    break;
                case BossState.Walk:
                    Move(walkSpeed);
                    break;
                case BossState.Run:
                    if(anim.GetBool(animID_Run))
                        Move(runSpeed);
                    break;
            }
            RotateToPlayer();
        }

        void RotateToPlayer()
        {
            rotationTimer -= Time.fixedDeltaTime;
            if (rotationTimer <= 0)
            {
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                Vector3 direction = player.transform.position - transform.position;
                Vector3 rotateVector = Quaternion.LookRotation(direction).eulerAngles;
                rotateVector.x = rotateVector.z = 0f;
                Vector3 diff = rotateVector - transform.rotation.eulerAngles;
                float dot = Vector3.Dot(direction, transform.forward);
               
                if (dot < 0.25f)
                {
                    anim.applyRootMotion = true;
                    //Keep the value between -180 to 180
                    if (diff.y > 180f)
                        diff.y -= 360f;
                    else if (diff.y < -180f)
                        diff.y += 360f;
                    
                    anim.SetTrigger(diff.y > 0f ? animID_LargeRight : animID_LargeLeft);
                }

                rotationTimer = 5f;
            }
        }

        void Move(float targetSpeed)
        {
            speed = targetSpeed;
            Vector3 targetDirection = transform.forward;
            controller.Move(targetDirection * (speed * Time.deltaTime));
            anim.SetFloat(animID_MotionSpeed, speed);
            if (Vector3.Distance(transform.position, player.position) < 2.0f)
            {
                currentPhase.mechanics[phasePart].completed = true;
                stateTime += currentPhase.mechanics[phasePart].length;
            }

        }
        #region State/phase change
        void TriggerPhase()
        {
            ChangeState(currentPhase.mechanics[phasePart].state);
        }

        public void ChangeState(BossState newState)
        {
            state = newState;
            switch (state)
            {
                case BossState.Idle:
                    
                    break;
                case BossState.Roar:
                    anim.SetTrigger(animID_Roar);
                    break;
                case BossState.Jump:
                    anim.SetTrigger(animID_Jump);
                    break;
                case BossState.JumpAttack:
                    anim.SetTrigger(animID_JumpAttack);
                    break;
                case BossState.SwipeLeft:
                    anim.SetTrigger(animID_HeavyAttack_Left);
                    break;
                case BossState.SwipeRight:
                    anim.SetTrigger(animID_HeavyAttack_Right);
                    break;
                case BossState.SwipeCombo:
                    currentCombo = 0;
                    anim.SetTrigger(Random.value > 0.5f ? animID_HeavyAttack_Right : animID_HeavyAttack_Left);
                    anim.SetBool(animID_Combo, true);
                    break;
                case BossState.Run:
                    anim.SetBool(animID_Run, true);
                    break;
                case BossState.Walk:
                    anim.SetBool(animID_Walk, true);
                    break;
            }
            stateTime = 0;
        }
        
        void EndPhase()
        {
            switch (currentPhase.mechanics[phasePart].state)
            {
                case BossState.SwipeCombo:
                    anim.SetBool(animID_Combo,  false);
                    break;
                case BossState.Run:
                    anim.SetBool(animID_Run, false);
                    break;
                case BossState.Walk:
                    anim.SetBool(animID_Walk, false);
                    break;
            }
        }
        

        void PickNextPhase()
        {
            phaseTimer = timeBetweenPhases;
            waitingBetweenPhases = true;
            
            currentPhase = phases[Random.Range(0, phases.Length)];
            if (currentPhase.healthTrigger > health.currentValue)
            {
                PickNextPhase();
                return;
            }

            phasePart = 0;
            stateTime = 0;
        }

        #endregion
        
        #region Animation events
        void AttackOn()
        {
            currentCombo++;
            if(walk != null)
                StopCoroutine(walk);
            walk = StartCoroutine(MoveForwards());
            for (int i = 0; i < hands.Length; ++i)
            {
                hands[i].AttackOn();
            }
        }

        void AttackOff()
        {
            anim.SetBool(animID_Combo, currentCombo < currentPhase.comboAmount);
            StopCoroutine(walk);
            walk = null;
            if (!anim.GetBool(animID_Combo))
            {
                for (int i = 0; i < hands.Length; ++i)
                {
                    hands[i].AttackOff();
                }
            }
        }

        void SmashImpact()
        {
            smashAttackParticles.SetActive(true);
            jumpAttackWeapon.Setup(damage * 1.5f);
            jumpAttackWeapon.AttackOn();
            StartCoroutine(LerpScale(2.5f));
            StartCoroutine(TurnOffObject(smashAttackParticles, 5f));
        }

        void JumpImpact()
        {
            jumpAttackParticles.SetActive(true);
            jumpAttackWeapon.Setup(damage * 1.2f);
            jumpAttackWeapon.AttackOn();
            StartCoroutine(LerpScale(1.5f));
            StartCoroutine(TurnOffObject(jumpAttackParticles, 3.5f));
        }

        #endregion

        IEnumerator TurnOffObject(GameObject go, float time)
        {
            yield return new WaitForSeconds(time);
            go.SetActive(false);
        }

        IEnumerator LerpScale(float time)
        {
            float timer = time;
            while (timer > 0)
            {
                timer -= Time.fixedDeltaTime;
                jumpAttackWeapon.transform.localScale = jumpAttackWeapon.transform.localScale * 1.01f;
                yield return null;
            }

            jumpAttackWeapon.transform.localScale = Vector3.one;
            jumpAttackWeapon.AttackOff();
        }

        IEnumerator MoveForwards()
        {
            while (true)
            {
                controller.Move(transform.forward.normalized * (attackMoveDistance * Time.fixedDeltaTime));
                yield return null;
                
            }
        }
    }

    [Serializable]
    public class BossPhase
    {
        public List<BossMechanic> mechanics;
        public float healthTrigger;
        public int comboAmount;

        public void Copy(BossPhase phase)
        {
            mechanics = new List<BossMechanic>();
            for (int i = 0; i < phase.mechanics.Count; ++i)
            {
                mechanics.Add(phase.mechanics[i]);
            }

            healthTrigger = phase.healthTrigger;
            comboAmount = phase.comboAmount;
        }
    }

    [Serializable]
    public class BossMechanic
    {
        public BossState state;
        public float length;
        public bool completed;
    }
    public enum BossState
    {
        Idle,
        Roar,
        Jump,
        JumpAttack,
        SwipeLeft,
        SwipeRight,
        SwipeCombo,
        Run,
        Walk
    }
}