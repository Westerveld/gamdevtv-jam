using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMasterGameManager : MonoBehaviour
{
    public static UIMasterGameManager instance;
    public List<Image> icons;

    private Color offColor = new Color(1, 1, 1, 0.098f);

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
            icons[i].color = offColor;
        }
        icons[(int)type].color = Color.white;
        
    }
}