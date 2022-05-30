using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlatformSpawner : MonoBehaviour
{
        public GameObject platformPrefab;
        public int poolCount = 15;
        public List<PlatformMover> platform = new List<PlatformMover>();
        public Transform spawnPoint;

        private bool canPlay = false;

        private GameObject tmp;

        private void Awake()
        {
            SetupObjectPools();
        }

        private void SetupObjectPools()
        {
            for (int i = 0; i < poolCount; ++i)
            {
                tmp = Instantiate(platformPrefab, transform);
                tmp.GetComponent<PlatformMover>().pool = this;
                platform.Add(tmp.GetComponent<PlatformMover>());
                tmp.SetActive(false);

            }
        }


        public void SpawnNextObstacle(float speed)
        {
            bool high = Random.value > 0.5f;

            for (int i = 0; i < platform.Count; ++i)
            {
                if (!platform[i].gameObject.activeSelf)
                {
                    platform[i].transform.position = spawnPoint.position;
                    platform[i].speed = speed;
                    platform[i].gameObject.SetActive(true);
                    break;
                }
            }
        }
}
