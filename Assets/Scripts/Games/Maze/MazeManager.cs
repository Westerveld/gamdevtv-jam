using UnityEngine;

namespace Maze
{
    public class MazeManager : GameManager
    {
        public GameObject[] doors;
        public MazeController player;

        public float timer;
        public float allowedTime;
        private bool canPlay;
        
        public override void StartGame(float value1 = 0, float value2 = 0)
        {
            base.StartGame(value1, value2);
            if (GameInstance.instance != null)
            {
                int completedGames = GameInstance.instance.GetCompletedGames();

                for (int i = 0; i < completedGames; i++)
                {
                    doors[i].SetActive(false);
                }
            }
            player.Setup(this);
            timer = allowedTime;
            canPlay = true;
        }

        private void FixedUpdate()
        {
            if (!canPlay) return;
            timer -= Time.fixedDeltaTime;
            if (timer <= 0)
            {
                if(GameInstance.instance !=null)
                    GameInstance.instance.GameEnd();
            }
        }

        public void Exit()
        {
            if (GameInstance.instance != null)
            {
                GameInstance.instance.SetGameComplete(gameType);
            }
        }

        [ContextMenu("Test")]
        public void Test()
        {
            StartGame();
        }
    }
}