using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class Weapon : MonoBehaviour
    {
        public TrailRenderer trail;
        public Collider collider;
        private float damage;
        public string enemyLayer = "Boss";

        public void Setup(float d)
        {
            damage = d;
        }
        
        public void AttackOn()
        {
            collider.enabled = true;
            if(trail != null)
                trail.emitting = true;
        }

        public void AttackOff()
        {
            collider.enabled = false;
            if(trail != null)
                trail.emitting = false;
        }

        void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer(enemyLayer))
            {
                IDamagable opp = col.gameObject.GetComponent<IDamagable>();
                opp.TakeDamage(damage, col.contacts[0].normal);
            }
        }
    }
}
