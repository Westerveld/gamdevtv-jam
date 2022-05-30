using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class SoulsManager : GameManager
    {

        public SoulsController player;
        public SoulsBoss boss;

        public int m_BuffAmount;
        public float m_DamageBuff = 10f;


        public override void StartGame(float value1 = 0, float value2 = 0)
        {
            base.StartGame();
            if(GameInstance.instance)
            {
               m_BuffAmount = GameInstance.instance.GetCompletedGames();
            }
            boss.Setup(value1, this);
            player.SetupPlayer(100f,100f,0.5f,12.5f, this, m_BuffAmount*m_DamageBuff);
        }

        // Update is called once per frame
        void LateUpdate()
        {
            
        }

        public void PlayerDied()
        {
            if (GameInstance.instance == null)
                return;
            GameInstance.instance.SetPersistantData(gameType, boss.health.currentValue);
            GameInstance.instance.GameEnd();
        }

        public void BossDied()
        {
            if(GameInstance.instance != null)
                GameInstance.instance.SetGameComplete(gameType);
        }

        [ContextMenu("Test")]
        void Test()
        {
            StartGame();
        }
    }
}