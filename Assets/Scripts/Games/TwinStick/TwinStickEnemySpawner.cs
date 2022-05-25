using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwinStick
{
    public class TwinStickEnemySpawner : MonoBehaviour
    {
        public Transform[] spawnPoints;
        public GameObject enemyPrefab;

        public List<GameObject> enemyPool;
        public int poolCount = 30;
        public TwinStickController player;

        public EffectsPool deathVFX, hitVFX;
        public TwinStickBulletPool bullets;
        public UITwinStickManager ui;
        public TwinStickManager manager;

        public float minDistToPlayer = 10f;
        public MinMax damage;
        public MinMax moveSpeed;
        public MinMax bulletSpeed;
        public MinMax health;
        public MinMax shootingRange;
        public MinMax bulletLifetime;
        void Start()
        {
            enemyPool = new List<GameObject>();
        }
        public void Setup(TwinStickManager m, TwinStickController p)
        {
            player = p;
            GameObject tmp;
            manager = m;
            for (int i = 0; i < poolCount; ++i)
            {
                tmp = Instantiate(enemyPrefab, transform);
                tmp.transform.localPosition = Vector3.zero;
                tmp.GetComponent<TwinStickEnemy>().SetReferences(player.transform,hitVFX,deathVFX, m, bullets);
                enemyPool.Add(tmp);
                tmp.SetActive(false);
            }
        }
        
        public void SpawnEnemy()
        {
            int spawnPoint = Random.Range(0, spawnPoints.Length);
            if ((spawnPoints[spawnPoint].position - player.transform.position).sqrMagnitude < minDistToPlayer)
            {
                spawnPoint = Random.Range(0, spawnPoints.Length);
            }
            float bDamage = Random.Range(damage.min, damage.max);
            float bSpeed = Random.Range(bulletSpeed.min, bulletSpeed.max);
            float h = Random.Range(health.min, health.max);
            float s = Random.Range(moveSpeed.min, moveSpeed.max);
            float sRange = Random.Range(shootingRange.min, shootingRange.max);
            float bRange = Random.Range(bulletLifetime.min, bulletLifetime.max);
            float scale = (bDamage + bSpeed + h + s + sRange + bRange) / (bulletLifetime.max + shootingRange.max + damage.max + bulletSpeed.max + health.max + moveSpeed.max); 
            for (int i = 0; i < enemyPool.Count; ++i)
            {
                if (!enemyPool[i].activeSelf)
                {
                    enemyPool[i].transform.position = spawnPoints[spawnPoint].position;
                    TwinStickEnemy enemyScript = enemyPool[i].GetComponent<TwinStickEnemy>();
                    ui.AssignEnemyHealthBar(enemyScript.transform);
                    enemyScript.bulletDamage = bDamage;
                    enemyScript.bulletSpeed = bSpeed;
                    enemyScript.shootingRange = sRange;
                    enemyScript.bulletRange = bRange;
                    enemyScript.Setup( h, s, scale);
                    enemyPool[i].SetActive(true);
                    break;
                }
            }
        }
        
    }
}