using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    [Serializable]
    public class SoulStat
    {
        public float currentValue;
        public readonly float maxValue;
        public readonly float regenSpeed;
        public float regenTimer;
        public SoulStat(float mValue, float regenSpeed, float cValue = 0f)
        {
            currentValue = maxValue = mValue;
            if (cValue > 0)
            {
                currentValue = cValue;
            }
            this.regenSpeed = regenSpeed;
        }

        public void RegenStat(float deltaTime)
        {
            regenTimer -= deltaTime;
            if (regenTimer <= 0f)
            {
                currentValue = Mathf.Max(currentValue + (regenSpeed * deltaTime), maxValue);
            }
        }

        public void RemoveStat(float value, float waitBeforeRegen = 0.1f)
        {
            currentValue = Mathf.Max(currentValue - value, 0f);
            regenTimer = waitBeforeRegen;
        }
    }
}
