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

        public int m_BuffAmount;
        public float m_TimerBuff = 15;

        private bool canPlay;

        public TMP_Text m_TimerText;

        public Transform endDoor;
        private float dist;
        private float distReq = 1f;

        public override void StartGame(float value1 = 0, float value2 = 0)
        {
            base.StartGame(value1, value2);
            if (GameInstance.instance != null)
            {
                int completedGames = GameInstance.instance.GetCompletedGames();

                for (int i = 0; i < completedGames; i++)
                {
                    if (doors[i] != null)
                    {
                        doors[i].SetActive(false);
                    }
                }
                m_BuffAmount = GameInstance.instance.GetCompletedGames();
                allowedTime += (m_TimerBuff * m_BuffAmount);
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

            dist = Vector3.Distance(endDoor.position, player.transform.position);
            if (dist < distReq)
            {
                Exit();
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