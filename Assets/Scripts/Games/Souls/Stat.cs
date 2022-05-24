using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generic
{
    [Serializable]
    public class Stat
    {
        public float currentValue;
        public readonly float maxValue;
        public readonly float regenSpeed;
        public float regenTimer;
        public Stat(float mValue, float regenSpeed, float cValue = 0f)
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
                currentValue = Mathf.Min(currentValue + (regenSpeed * deltaTime), maxValue);
            }
        }

        public void RemoveStat(float value, float waitBeforeRegen = 0.1f)
        {
            currentValue = Mathf.Max(currentValue - value, 0f);
            regenTimer = waitBeforeRegen;
        }
    }
}
