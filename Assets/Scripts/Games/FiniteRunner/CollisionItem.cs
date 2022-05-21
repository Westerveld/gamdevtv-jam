using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Runner
{
    public class CollisionItem : MonoBehaviour
    {
        public bool goal = false;


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