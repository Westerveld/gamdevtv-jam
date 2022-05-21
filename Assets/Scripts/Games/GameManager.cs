using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameType gameType;

    public virtual void StartGame(float value1 = 0f, float value2 = 0f)
    {
        
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
