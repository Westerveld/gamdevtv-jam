using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class SoulsBoss : MonoBehaviour
    {
        public SoulStat health;
        public float maxHealth = 300f;

        public void Setup(float healthValue)
        {
            health = new SoulStat(maxHealth, 0f, healthValue);
        }
    }
}