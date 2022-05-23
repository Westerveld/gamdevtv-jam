using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public interface IDamagable
    {
        public void TakeDamage(float damage, Vector3 impactPoint);
    }
}