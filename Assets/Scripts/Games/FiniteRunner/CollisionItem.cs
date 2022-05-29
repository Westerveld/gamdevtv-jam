using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Runner
{
    public class CollisionItem : MonoBehaviour
    {
        public bool goal = false;

        public float speed;
        public AudioClip hit;

        private void FixedUpdate()
        {
            transform.position += Vector3.left * speed;
            if (transform.position.x < -20f)
            {
                gameObject.SetActive(false);
            }
        }

        void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (goal)
                {
                    //Tell the instance
                    RunnerManager.instance.WonGame();
                }
                else
                {
                    AudioManager.instance?.PlaySFX(hit);
                    //Game Over
                    RunnerManager.instance.LostGame();
                }
                gameObject.SetActive(false);
            }

            if (col.gameObject.layer == LayerMask.NameToLayer("Boss"))
            {
                gameObject.SetActive(false);
            }
        }
    }
}