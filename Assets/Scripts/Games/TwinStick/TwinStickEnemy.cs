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
        public EffectsPool hitFX, deathFX;
        private TwinStickManager manager;

        public TwinStickBulletPool bullets;
        public float shotInterval = 4f;
        public float bulletSpeed = 5f;
        private float shotTimer;
        public float bulletDamage = 6f;
        public float shootingRange = 50f;
        public float bulletRange = 0f;

        private float distToPlayer;
        public float maxDistToPlayer = 5f;

        public UIEnemyHealthBar m_HealthBar;

        public float timeTilMoving = 5f;
        public float moveTimer = 0f;
        public void SetReferences(Transform p, EffectsPool hFX, EffectsPool dFX, TwinStickManager m,  TwinStickBulletPool b)
        {
            deathFX = dFX;
            hitFX = hFX;
            player = p;
            manager = m;
            bullets = b;

        }
        
        public void Setup(float maxHealth, float speed, float scale)
        {
            health = new Stat(maxHealth, 0f);
            m_HealthBar.SetMonsterMaxHealth(maxHealth);
            canPlay = true;
            shotTimer = shotInterval;
            agent.speed = speed;
            Vector3 s = Vector3.one;
            s *= 1.5f - scale;
            transform.localScale = s;

        }
        
        void FixedUpdate()
        {
            if (canPlay)
            {
                distToPlayer = Vector3.Distance(transform.position, player.position);
                if (distToPlayer > 5f)
                {
                    moveTimer -= Time.fixedDeltaTime;
                    if (moveTimer <= 0)
                    {
                        agent.SetDestination(player.position);
                        agent.isStopped = false;
                    }
                }
                else
                {
                    agent.isStopped = true;
                    transform.LookAt(player, Vector3.up);
                    Vector3 rot = transform.rotation.eulerAngles;
                    rot.x = rot.z = 0f;
                    transform.rotation = Quaternion.Euler(rot);
                    moveTimer = timeTilMoving;
                }
                
                shotTimer -= Time.fixedDeltaTime;
                if (shotTimer <= 0 &&  distToPlayer < shootingRange)
                {
                    bullets.FireBullet(transform.forward.normalized, transform.position + transform.forward.normalized, transform.rotation, bulletSpeed, bulletDamage, bulletRange);
                    shotTimer = shotInterval;
                }
            }
        }

        public void TakeDamage(float amount, Vector3 impactPoint)
        {
            health.RemoveStat(amount);
            m_HealthBar.SetMonsterHealth(health.currentValue);
            hitFX.SpawnObject(transform.position - impactPoint, transform.rotation );
            transform.position += impactPoint;
            if (health.currentValue <= 0)
            {
                Die(impactPoint);
            }
        }

        void Die(Vector3 impactPoint)
        {
            //ToDo: DeathAnimation
            m_HealthBar.KillMonster();
            deathFX.SpawnObject(transform.position - impactPoint, transform.rotation);
            gameObject.SetActive(false);
            manager.KilledEnemy();
            //StartCoroutine(WaitThenDisable());
        }

        IEnumerator WaitThenDisable()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            gameObject.SetActive(false);
        }
    }
}