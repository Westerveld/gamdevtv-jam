using System;
using System.Collections;
using System.Collections.Generic;
using Runner;
using UnityEngine;

namespace Runner
{
    public class RunnerManager : GameManager
    {
        public RunController player;
        public ObjectSpawner belt;

        public static RunnerManager instance;
        public float distanceToCompleteGame;
        private float currentDistance;
        public bool canPlay;
        public float currSpeed = 1f;
        public float speedIncrease = 0.1f;
        public MinMax speedLimit;
        private float objectTimer = 5f;
        public float spawnInterval = 2.5f;
        private bool finishedGame;

        public UIRunnerManager ui;
        void Awake()
        {
            instance = this;
        }
        // Start is called before the first frame update
        public override void StartGame(float value1 = 0, float value2 = 0)
        {
            base.StartGame();
            currentDistance = value1;
            ui.SetMaxDistance(distanceToCompleteGame);
            if (value2 != 0)
            {
                currSpeed = value2;
            }
            else
            {
                currSpeed = speedLimit.min;
            }
            ui.SetPlayerProgress(currentDistance);

            finishedGame = false;
            canPlay = true;
            player.Setup();
            belt.Setup();
        }

        private void FixedUpdate()
        {
            if (!canPlay) return;
            if (finishedGame) return;
            currentDistance += Time.deltaTime * currSpeed;
            objectTimer -= Time.deltaTime;
            if (objectTimer <= 0)
            {
                if (currentDistance > distanceToCompleteGame)
                {
                    belt.SpawnFinish(currSpeed);
                    finishedGame = true;
                }
                else
                {
                    belt.SpawnNextObstacle(currSpeed);
                    objectTimer = spawnInterval;
                        
                }
            }
            if (currentDistance % 5 == 0)
            {
                currSpeed = Mathf.Min(currSpeed + speedIncrease, speedLimit.max);
            }
            ui.SetPlayerProgress(currentDistance);
        }

        public void WonGame()
        {
            GameInstance.instance.SetGameComplete(gameType);
        }

        public void LostGame()
        {
            GameInstance.instance.SetPersistantData(gameType, currentDistance, currSpeed);
            GameInstance.instance.GameEnd();
        }

        [ContextMenu("Test")]
        public void Test()
        {
            StartGame();
        }
    }
}