using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBased
{
    public class TurnBasedManager : GameManager
    {
        //need player card class
        //deck
        //current hand
        //dead cards to reshuffle on deck empty.

        public void PlayCard(int number)
        {
            Debug.Log($"Playing card {number}");
        }
    }


    public enum CardType
    {
        Attack,
        Defence,
        EffectOnly
    }

    public class CardData
    {
        public string m_Title;
        public string m_Description;

        public CardType m_CardType;

        public int m_EnergyCost;
        public int m_ValueCost;

        public void SetupCard(string title, string description, CardType type, int energy, int value = -1)
        {
            m_Title = title;
            m_Description = description;
            m_CardType = type;
            m_EnergyCost = energy;
            m_ValueCost = value;
        }
    }
}