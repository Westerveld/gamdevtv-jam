using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMasterGameManager : MonoBehaviour
{
    public static UIMasterGameManager instance;
    public List<Image> icons;

    private readonly Color offColor = new Color(1, 1, 1, 0.098f);
    private readonly Color completeColor = new Color(0.9f, 0.2f, 0.9f, 1f);

    private void Awake()
    {
        if(instance == null)
            instance = this;
        
    }

    public void SetIcon(GameType type)
    {
        if (instance == null) instance = this;
        for (int i = 0; i < icons.Count; ++i)
        {
            if(icons[i].color != completeColor)
                icons[i].color = offColor;
        }
        icons[(int)type].color = Color.white;
        
    }

    public void SetIconComplete(GameType type)
    {
        icons[(int)type].color = completeColor;
    }
}