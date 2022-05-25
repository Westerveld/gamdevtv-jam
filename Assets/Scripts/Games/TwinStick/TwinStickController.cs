using System;
using System.Collections;
using System.Collections.Generic;
using Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TwinStick
{
    [RequireComponent(typeof(TwinStickInput))]
    public class TwinStickController : MonoBehaviour, IDamagable
    {
        public TwinStickInput input;
        public PlayerInput playerInput;
        public Camera cam;

        public TwinStickBulletPool bulletPool;
        public float shotInterval = 0.25f;
        public float shotTimer;

        public float baseDamage = 5f;
        public float baseSpeed = 2.5f;

        public float bulletSpeed;
        public float bulletDamage;
        public float playerSpeed = 3f;
        public float rotationSpeed = 4f;

        private Vector3 direction;
        public Transform bulletSpawnPoint;

        private Rigidbody rigid;

        private bool canPlay = false;

        private int ammo, maxAmmo;
        public float recoilAmount = 0.25f;

        public UITwinStickManager ui;

        public float reloadSpeed = 0.1f;
        public float healthRegenSpeed = 0.1f;

        public Stat health;

        public TwinStickManager tManager;

        public Animator gun;
        private static readonly int pew = Animator.StringToHash("Pew");

        private void Start()
        {
            rigid = GetComponent<Rigidbody>();
        }

        public void SetupPlayer(TwinStickManager t, float damageModifier = 1f, float speedModifier = 1f, int ammoAmount = 50, int maxHealth = 100)
        {
            tManager = t;
            //Are we having regen in twin stick?
            health = new Stat(maxHealth, healthRegenSpeed);
            ui.SetPlayerMaxHealth(maxHealth);
            ui.SetPlayerHealth(maxHealth);
            bulletDamage = baseDamage * damageModifier;
            bulletSpeed = baseSpeed * speedModifier;
            canPlay = true;
            ammo = maxAmmo = ammoAmount;
        }

        void Update()
        {
            if (!canPlay) return;
            shotTimer -= Time.fixedDeltaTime;
            Move();
            Rotate();
            Shoot();
        }

        private void FixedUpdate()
        {
            health.RegenStat(Time.fixedDeltaTime);
            ui.SetPlayerHealth((int)health.currentValue);
        }

        void Move()
        {
            rigid.velocity = new Vector3(input.move.normalized.x * (playerSpeed * Time.fixedDeltaTime), 0f,
                input.move.normalized.y * (playerSpeed * Time.fixedDeltaTime));
        }

        void Rotate()
        {
            if (input.rotate == Vector2.zero)
                return;
            if (playerInput.currentControlScheme == "KeyboardMouse")
            {
                MouseRotate();
                return;
            }

            Vector3 rotVector = Quaternion.LookRotation(new Vector3(input.rotate.x, 0f, -input.rotate.y)).eulerAngles;
            rotVector.x = rotVector.z = 0f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(rotVector),
                rotationSpeed * Time.fixedDeltaTime);
        }

        void MouseRotate()
        {
            Vector3 mouseWorldPosition =
                cam.ScreenToWorldPoint(new Vector3(input.rotate.x, input.rotate.y, cam.transform.position.y));
            //mouseWorldPosition.y = transform.position.y;
            Vector3 rotationVector = Quaternion.LookRotation(mouseWorldPosition - transform.position).eulerAngles;
            rotationVector.x = rotationVector.z = 0;

            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, Quaternion.Euler(rotationVector), 1f);

        }

        void Shoot()
        {
             if (input.shoot)
             {
                 //input.shoot = false;
                if (shotTimer <= 0 && ammo > 0)
                {
                    shotTimer = shotInterval;
                    bulletPool.FireBullet(bulletSpawnPoint.transform.forward, bulletSpawnPoint.position,
                        bulletSpawnPoint.rotation, bulletSpeed, bulletDamage);
                    transform.position -= (bulletSpawnPoint.transform.forward * recoilAmount);
                    gun.SetTrigger(pew);
                    ammo--;
                    ui.SetAmmoCount(ammo, maxAmmo);
                    if (ammo <= 0 )
                    {
                        StartCoroutine(Reloading());
                    }
                }
             }
        }
        
        IEnumerator Reloading()
        {
            float timer = 1f;
            while (timer > 0f)
            {
                ui.SetReloadTimerState(1-timer);
                timer -= Time.fixedDeltaTime * reloadSpeed;
                yield return null;
            }
            ammo = maxAmmo;
            ui.SetAmmoCount(ammo, maxAmmo);
        }

        public void TakeDamage(float damage, Vector3 impactPoint)
        {
            health.RemoveStat(damage);

            if (health.currentValue <= 0)
            {
                tManager.Died();
            }
        }
    }
}
