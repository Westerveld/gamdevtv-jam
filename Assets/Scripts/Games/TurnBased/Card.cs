using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBased;

[CreateAssetMenu(fileName = "Card",menuName = "TurnBased/Card", order = 0)]
public class Card : ScriptableObject
{
    public CardType m_CardType;
    public string m_Title;
    public string m_Description;
    public int m_EnergyCost;
    public int m_Value;
    public Sprite m_Sprite;

}
