using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameType gameType;

    public TMP_Text timerText;
    public GameObject tutorial;
    public virtual void StartGame(float value1 = 0f, float value2 = 0f)
    {
        UIMasterGameManager.instance.SetIcon(gameType);
        AudioManager.instance?.StartGameAudio(gameType);
    }
    
    [ContextMenu("Test")]
    void Test()
    {
        StartGame();
    }

    public virtual void EndGame()
    {
        
    }

    public void ShowTutorial(float value1 = 0f, float value2 = 0f)
    {
        StartCoroutine(Tutorial(value1, value2));
    }

    private IEnumerator Tutorial(float value1 = 0f, float value2 = 0f)
    {
        timerText.text = "5";
        yield return new WaitForSeconds(1f);
        timerText.text = "4";
        yield return new WaitForSeconds(1f);
        timerText.text = "3";
        yield return new WaitForSeconds(1f);
        timerText.text = "2";
        yield return new WaitForSeconds(1f);
        timerText.text = "1";
        yield return new WaitForSeconds(1f);
        timerText.text = "GO!";
        
        tutorial.SetActive(false);
        StartGame(value1,value2);
    }
}

public enum GameType
{
    Cooking,
    Dating,
    Runner,
    Maze,
    Souls,
    TurnBased,
    TwinStick
}
