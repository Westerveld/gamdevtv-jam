using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameType gameType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
