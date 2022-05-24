using System.Collections;
using System.Collections.Generic;
using Generic;
using Souls;
using UnityEngine;
using UnityEngine.AI;

namespace TwinStick
{
    public class TwinStickEnemy : MonoBehaviour, IDamagable
    {
        public Transform player;
        public NavMeshAgent agent;
        public bool canPlay = false;

        public Stat health;

        public void Setup(float maxHealth)
        {
            health = new Stat(maxHealth, 0f);
        }
        void Update()
        {
            if (canPlay)
            {
                agent.SetDestination(player.position);
            }
        }

        public void TakeDamage(float amount, Vector3 impactPoint)
        {
            health.RemoveStat(amount);
            transform.position += impactPoint;
            if (health.currentValue <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            //ToDo: DeathAnimation
        }
    }
}