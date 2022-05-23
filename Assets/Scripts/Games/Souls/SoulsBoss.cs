using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Souls
{
    public class SoulsBoss : MonoBehaviour, IDamagable
    {
        public SoulStat health;
        public float maxHealth = 300f;

        public Animator anim;
        public CharacterController controller;

        private int animID_Roar = Animator.StringToHash("Roar");
        private int animID_Walk = Animator.StringToHash("Walk");
        private int animID_Run = Animator.StringToHash("Run");
        private int animID_HeavyAttack_Left = Animator.StringToHash("HeavyAttack_Left");
        private int animID_HeavyAttack_Right = Animator.StringToHash("HeavyAttack_Right");
        private int animID_Jump = Animator.StringToHash("Jump");
        private int animID_JumpAttack = Animator.StringToHash("JumpAttack");
        private int animID_Combo = Animator.StringToHash("Combo");
        private int animID_Dead = Animator.StringToHash("Dead");

        private int animID_Hit1 = Animator.StringToHash("Hit1");
        private int animID_Hit2 = Animator.StringToHash("Hit2");

        private int currentCombo = 0;
        private float stateTime = 0f;

        public BossPhase[] phases;
        public BossPhase currentPhase;
        private int phasePart = 0;

        public Weapon[] hands;

        private bool canPlay = false;
        public void Setup(float healthValue)
        {
            health = new SoulStat(maxHealth, 0f, healthValue);
            PickNextPhase();
            canPlay = true;
        }
        
        public void TakeDamage(float damage, Vector3 normal)
        {
            normal.y = 0f;
            transform.position += normal;
            health.RemoveStat(damage);
        }

        void Update()
        {
            if (!canPlay) return;
            ProgressPhase();
        }

        void ProgressPhase()
        {
            if (currentPhase.mechanics[phasePart].completed)
            {
                stateTime += Time.fixedDeltaTime;
                if (stateTime > currentPhase.mechanics[phasePart].length)
                {
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
        }

        void TriggerPhase()
        {
            switch (currentPhase.mechanics[phasePart].state)
            {
                case BossState.Idle:
                    currentPhase.mechanics[phasePart].completed = true;
                    break;
                case BossState.Roar:
                    anim.SetTrigger(animID_Roar);
                    currentPhase.mechanics[phasePart].completed = true;
                    break;
                case BossState.Jump:
                    anim.SetTrigger(animID_Jump);
                    currentPhase.mechanics[phasePart].completed = true;
                    break;
                case BossState.JumpAttack:
                    anim.SetTrigger(animID_JumpAttack);
                    currentPhase.mechanics[phasePart].completed = true;
                    break;
                case BossState.SwipeLeft:
                    anim.SetTrigger(animID_HeavyAttack_Left);
                    currentPhase.mechanics[phasePart].completed = true;
                    break;
                case BossState.SwipeRight:
                    anim.SetTrigger(animID_HeavyAttack_Right);
                    currentPhase.mechanics[phasePart].completed = true;
                    break;
                case BossState.SwipeCombo:
                    anim.SetBool(animID_Combo, true);
                    currentPhase.mechanics[phasePart].completed = true;
                    break;
                case BossState.Run:
                    anim.SetBool(animID_Run, true);
                    currentPhase.mechanics[phasePart].completed = true;
                    break;
                case BossState.Walk:
                    anim.SetBool(animID_Walk, true);
                    currentPhase.mechanics[phasePart].completed = true;
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
            currentPhase = phases[Random.Range(0, phases.Length)];
            if (currentPhase.healthTrigger > health.currentValue)
            {
                PickNextPhase();
                return;
            }

            phasePart = 0;
            stateTime = 0;
            TriggerPhase();
        }

        void AttackOn()
        {
            for (int i = 0; i < hands.Length; ++i)
            {
                hands[i].AttackOn();
            }
        }

        void AttackOff()
        {
            anim.SetBool(animID_Combo, currentCombo < currentPhase.comboAmount);
            if (!anim.GetBool(animID_Combo))
            {
                for (int i = 0; i < hands.Length; ++i)
                {
                    hands[i].AttackOff();
                }
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