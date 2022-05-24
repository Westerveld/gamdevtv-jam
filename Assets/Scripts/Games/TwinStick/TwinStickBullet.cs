using System.Collections;
using System.Collections.Generic;
using Generic;
using UnityEngine;

namespace TwinStick
{
    public class TwinStickBullet : MonoBehaviour
    {
        public float damage;
        public float speed;
        public Vector3 normalisedDirection;

        private Rigidbody rigid;

        void Awake()
        {
            rigid = GetComponent<Rigidbody>();
        }

        void OnEnable()
        {
            StartCoroutine(DisableAfterTime());
        }

        void OnDisable()
        {
            StopAllCoroutines();
        }

        void Update()
        {
            rigid.velocity = normalisedDirection * (speed * Time.fixedDeltaTime);
        }

        IEnumerator DisableAfterTime()
        {
            yield return new WaitForSeconds(10f);
            gameObject.SetActive(false);
        }


        void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Boss"))
            {
                IDamagable damagable = col.gameObject.GetComponent<IDamagable>();
                damagable.TakeDamage(damage, (-col.contacts[0].normal * 0.05f));
                gameObject.SetActive(false);
            }
        }
    }
}
