using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwinStick
{
    public class TwinStickManager : GameManager
    {

        public TwinStickController player;
        public UITwinStickManager ui;
        public TwinStickEnemySpawner enemySpawner;
        public int killsReq = 50;
        public float spawnInterval;
        public float spawnTimer;
        private bool canPlay = false;

        public float m_DamageBuff = 6f;
        public int m_BuffAmount = 0;

        // Start is called before the first frame update
        public override void StartGame(float value1 = 0, float value2 = 0)
        {
            base.StartGame(value1, value2);
            if(GameInstance.instance)
            {
                m_BuffAmount = GameInstance.instance.GetCompletedGames();
            }
            player.SetupPlayer(this,m_DamageBuff*m_BuffAmount);
            enemySpawner.Setup(this, player);
            ui.m_KillsNeeded.text = killsReq.ToString();
            spawnTimer = spawnInterval;
            canPlay = true;
        }

        private void FixedUpdate()
        {
            if (!canPlay) return;
            
            spawnTimer -= Time.fixedDeltaTime;
            if (spawnTimer <= 0)
            {
                enemySpawner.SpawnEnemy();
                spawnTimer = spawnInterval;
            }
            
        }


        public void KilledEnemy()
        {
            killsReq--;
            killsReq = Mathf.Max(0, killsReq);
            ui.m_KillsNeeded.text = killsReq.ToString();
        }

        [ContextMenu("Test")]
        public void Test()
        {
            StartGame();
        }

        public void Died()
        {
            if (GameInstance.instance != null)
            {
                GameInstance.instance.SetPersistantData(gameType);
                GameInstance.instance.GameEnd();
            }
        }

        public void GotOut()
        {
            if (GameInstance.instance != null)
            {
                GameInstance.instance.SetGameComplete(gameType);
            }
        }

        public bool NoEnemiesLeftToKill()
        {
            return killsReq > 0;
        }
    }
}