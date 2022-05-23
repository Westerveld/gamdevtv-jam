using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class SoulsManager : GameManager
    {

        public SoulsController player;
        public SoulsBoss boss;


        public override void StartGame(float value1 = 0, float value2 = 0)
        {
            boss.Setup(value1);
            player.SetupPlayer(100f,100f,0.5f,0.5f);
        }

        // Update is called once per frame
        void LateUpdate()
        {
            
        }

        public void PlayerDied()
        {
            GameInstance.instance.SetPersistantData(gameType, boss.health.currentValue);
            GameInstance.instance.GameEnd();
        }

        [ContextMenu("Test")]
        void Test()
        {
            StartGame();
        }
    }
}