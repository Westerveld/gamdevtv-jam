using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runner
{
    public class ObjectSpawner : MonoBehaviour
    {
        public GameObject obstacleLow, obstacleHigh;
        public int poolCount = 15;
        private List<CollisionItem> lowObstacles = new List<CollisionItem>(), highObstacles = new List<CollisionItem>(); 
        public Transform lowSpawnPoint, highSpawnPoint;
        public GameObject finalObject;

        private bool canPlay = false;

        private GameObject tmp;

        public bool assignPool;

        private void SetupObjectPools()
        {
            for (int i = 0; i < poolCount; ++i)
            {
                tmp = Instantiate(obstacleLow, transform);
                lowObstacles.Add(tmp.GetComponent<CollisionItem>());
                tmp.SetActive(false);

                tmp = Instantiate(obstacleHigh, transform);
                highObstacles.Add(tmp.GetComponent<CollisionItem>());
                tmp.SetActive(false);
            }
        }

        public void Setup()
        {
            canPlay = true;
            SetupObjectPools();
        }


        public void SpawnNextObstacle(float speed)
        {
            bool high = Random.value > 0.5f;

            if (high)
            {
                for (int i = 0; i < lowObstacles.Count; ++i)
                {
                    if (!lowObstacles[i].gameObject.activeSelf)
                    {
                        lowObstacles[i].transform.position = lowSpawnPoint.position;
                        lowObstacles[i].speed = speed;
                        lowObstacles[i].gameObject.SetActive(true);
                        lowObstacles[i].transform.GetChild(0).gameObject.SetActive(true);
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < highObstacles.Count; ++i)
                {
                    if (!highObstacles[i].gameObject.activeSelf)
                    {
                        highObstacles[i].transform.position = highSpawnPoint.position;
                        highObstacles[i].speed = speed;
                        highObstacles[i].gameObject.SetActive(true);
                        highObstacles[i].transform.GetChild(0).gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }

        public void SpawnFinish(float speed)
        {
            GameObject end = Instantiate(finalObject, transform);
            end.transform.position = lowSpawnPoint.position;
            end.GetComponent<CollisionItem>().speed = speed;
        }
    }
}