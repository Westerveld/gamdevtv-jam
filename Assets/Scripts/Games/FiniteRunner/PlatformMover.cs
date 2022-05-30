using System.Collections;
using System.Collections.Generic;
using Runner;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    public float speed;
    public PlatformSpawner pool;
    private void FixedUpdate()
    {
        if(RunnerManager.instance.canPlay)
            transform.position += Vector3.left * RunnerManager.instance.currSpeed;
        if (transform.position.x < -20f)
        {
            pool.SpawnNextObstacle(RunnerManager.instance.currSpeed);
            gameObject.SetActive(false);
            
        }
    }
}
