using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner
{
    public class ConveyorBelt : MonoBehaviour
    {
        public float speed = 1f;
        public float speedIncrease = 0.1f;
        public float timeBeforeSpeedIncrease = 10f;
        public float timer;

        private bool canPlay = false;
        public void Setup()
        {
            canPlay = true;
            timer = timeBeforeSpeedIncrease;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!canPlay) return;

            transform.position += Vector3.left * speed;

            timer -= Time.fixedDeltaTime;
            if (timer <= 0)
            {
                timer = timeBeforeSpeedIncrease;
                speed += speedIncrease;
            }
        }

        public void Stop()
        {
            canPlay = false;
        }
    }
}