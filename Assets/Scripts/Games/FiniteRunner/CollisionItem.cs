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

        private void FixedUpdate()
        {
            transform.position += Vector3.left * speed;
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
                    //Game Over
                    RunnerManager.instance.LostGame();
                }
                gameObject.SetActive(false);
            }
        }
    }
}