using UnityEngine;
using TMPro;

namespace Maze
{
    public class MazeManager : GameManager
    {
        public GameObject[] doors;
        public MazeController player;

        public float timer;
        public float allowedTime;
        private bool canPlay;

        public TMP_Text m_TimerText;

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
            SetTimerText(timer);
            canPlay = true;
        }

        private void FixedUpdate()
        {
            if (!canPlay) return;
            timer -= Time.fixedDeltaTime;
            SetTimerText(timer);
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

        public void SetTimerText(float val)
        {
            m_TimerText.text = val.ToString("0");
        }
    }
}