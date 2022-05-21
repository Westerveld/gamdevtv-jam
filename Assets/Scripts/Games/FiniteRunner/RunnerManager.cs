using System.Collections;
using System.Collections.Generic;
using Runner;
using UnityEngine;

namespace Runner
{
    public class RunnerManager : GameManager
    {
        public RunController player;
        public ConveyorBelt belt;

        public static RunnerManager instance;

        void Awake()
        {
            instance = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            player.Setup();
            belt.Setup();
        }

        public void WonGame()
        {
            GameInstance.instance.SetGameComplete(gameType);
        }

        public void LostGame()
        {
            GameInstance.instance.GameEnd();
        }
    }
}