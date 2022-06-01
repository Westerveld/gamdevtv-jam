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

        public float m_SpeedBuff = 0.5f;
        public float m_ReloadSpeedBuff = 0.5f;
        public int m_AmmoBuff = 10;
        

        private bool ending = false;

        public TwinStickDoor[] m_Doors;
        public bool m_CanSpawn;

        // Start is called before the first frame update
        public override void StartGame(float value1 = 0, float value2 = 0)
        {
            base.StartGame(value1, value2);
            if(GameInstance.instance)
            {
                m_BuffAmount = GameInstance.instance.GetCompletedGames();
            }
            float speed = 1 + (m_SpeedBuff * m_BuffAmount);
            float reloadSpeed = 1 + (m_ReloadSpeedBuff * m_BuffAmount);
            int ammoCount = 50 + (m_AmmoBuff * m_BuffAmount);
            int maxHealth = 100 + (m_AmmoBuff * m_BuffAmount);
            player.SetupPlayer(this,m_DamageBuff*m_BuffAmount,speed, ammoCount, maxHealth, reloadSpeed);
            enemySpawner.Setup(this, player);
            if ((int)value1 != 0)
                killsReq = (int)value1;
            ui.m_KillsNeeded.text = killsReq.ToString();
            spawnTimer = spawnInterval;
            canPlay = true;
            m_CanSpawn = true;
        }

        private void FixedUpdate()
        {
            if (!canPlay) return;
            
            spawnTimer -= Time.fixedDeltaTime;
            if (spawnTimer <= 0 && m_CanSpawn)
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
            if(killsReq == 0)
            {
                m_CanSpawn = false;
                OpenDoors();
            }
        }

        [ContextMenu("Test")]
        public void Test()
        {
            StartGame();
        }

        public void Died()
        {
            if (ending) return;
            ending = true;
            if (GameInstance.instance != null)
            {
                GameInstance.instance.SetPersistantData(gameType,killsReq);
                GameInstance.instance.GameEnd();
            }
        }

        public void OpenDoors()
        {
            for(int i = 0; i < m_Doors.Length; i++)
            {
                m_Doors[i].OpenDoors();
            }
        }
        
        public void GotOut()
        {
            if (ending) return;
            ending = true;
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