using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dating
{
    public class DatingManager : GameManager
    {
        public float currentCharm = 0f;
        public float charmAddition = 15f;


        public int displayedOptions = 4;

        public TMP_Text questionText;
        public List<TMP_Text> answerText;
        public List<GameObject> buttons;

        public Question[] questions;

        private Question[] currentQuestions;
        private Question selectedQuestion;

        private int correctOption = 0;
        private int currentQ = 0;

        private System.Random rand = new System.Random();

        public DatingController player;

        public AudioClip gameWon;

        public UIDatingManager m_UIDatingManager;

        public float m_CharmBuff = 5f;
        public int m_BuffAmount = 0;

        public override void StartGame(float value1 = 0f, float value2 = 0f)
        {
            base.StartGame(value1, value2);
            currentCharm = value1;
            if(GameInstance.instance)
            {
                m_BuffAmount = GameInstance.instance.GetCompletedGames();
            }
            currentQuestions = questions.OrderBy(x => rand.Next()).ToArray();
            m_UIDatingManager.SetUpUI(value1/100f);
            StartTurn();
        }

        void StartTurn()
        {
            selectedQuestion = currentQuestions[currentQ];
            correctOption = Random.Range(0, displayedOptions);
            questionText.text = selectedQuestion.questionText;
            answerText[correctOption].text = selectedQuestion.correctAnswer;
            int fakeText = 0;
            List<string> randomAnswers = selectedQuestion.fakeAnswers.OrderBy(x => rand.Next()).ToList();
            for (int i = 0; i < answerText.Count; ++i)
            {
                if (i == correctOption)
                    answerText[i].text = selectedQuestion.correctAnswer;
                else
                {
                    answerText[i].text = randomAnswers[fakeText];
                    fakeText++;
                }

            }
        }

        public void UI_Option1()
        {
            ShowResult(correctOption == 0);
        }

        public void UI_Option2()
        {
            ShowResult(correctOption == 1);
        }

        public void UI_Option3()
        {
            ShowResult(correctOption == 2);
        }

        public void UI_Option4()
        {
            ShowResult(correctOption == 3);
        }

        void ShowResult(bool correct)
        {
            player.Attempt();
            if (correct)
            {
                StartCoroutine(ResultThenNextScreen());
            }
            else
            {
                StartCoroutine(ResultThenDeduction());
            }
        }

        IEnumerator ResultThenNextScreen()
        {
            player.Success();
            currentCharm += charmAddition + (m_BuffAmount*m_CharmBuff);
            m_UIDatingManager.StartFillAmount(currentCharm / 100f);
            currentQ++;
            yield return new WaitForSeconds(6f);

            if (currentCharm >= 100)
            {
                AudioManager.instance?.PlaySFX(gameWon);
                GameInstance.instance.SetGameComplete(gameType);
            }
            else
            {
                if (currentQ >= currentQuestions.Length)
                {
                    currentQuestions = questions.OrderBy(x => rand.Next()).ToArray();
                    currentQ = 0;
                }

                StartTurn();
            }
        }

        IEnumerator ResultThenDeduction()
        {
            player.Failure();
            GameInstance.instance.SetPersistantData(gameType, currentCharm);
            yield return new WaitForSeconds(6f);
            GameInstance.instance.GameEnd();
        }
    }

    [Serializable]
    public class Question
    {
        public string questionText;
        public string correctAnswer;
        public string[] fakeAnswers;
    }
}