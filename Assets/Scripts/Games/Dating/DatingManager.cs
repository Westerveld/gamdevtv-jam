using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DatingManager : GameManager
{
    public float currentCharm = 0f;
    public float charmDeduction = 10f;
    public float charmAddition = 15f;

    public int displayedOptions = 4;

    public TMP_Text questionText;
    public List<TMP_Text> answerText;
    public List<GameObject> buttons;

    public Question[] questions;

    private Question[] currentQuestions;
    private Question selectedQuestion;
    
    private int correctOption = 0;

    private System.Random rand = new System.Random();

    public DatingController player;
    
    public override void StartGame(float value1 = 0f, float value2 = 0f)
    {
        currentCharm = value1;
        StartTurn();
    }

    void StartTurn()
    {
        selectedQuestion = currentQuestions[Random.Range(0, currentQuestions.Length)];
        correctOption = Random.Range(1, displayedOptions);
        questionText.text = selectedQuestion.questionText;
        answerText[correctOption].text = selectedQuestion.correctAnswer;
        int fakeText = 0;
        List<string> randomAnswers = selectedQuestion.fakeAnswers.OrderBy(x => rand.Next()).ToList();
        for (int i = 0; i < answerText.Count; ++i)
        {
            if(i == correctOption)
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
        ShowResult(correctOption == 1);
    }

    public void UI_Option2()
    {
        ShowResult(correctOption == 2);
    }

    public void UI_Option3()
    {
        ShowResult(correctOption == 3);
    }

    public void UI_Option4()
    {
        ShowResult(correctOption == 4);
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
        currentCharm += charmAddition;
        yield return new WaitForSeconds(2f);
        
    }

    IEnumerator ResultThenDeduction()
    {
        player.Failure();
        GameInstance.instance.SetPersistantData(gameType, currentCharm);
        yield return new WaitForSeconds(2f);
        
    }
}

[Serializable]
public class Question
{
    public string questionText;
    public string correctAnswer;
    public string[] fakeAnswers;
}