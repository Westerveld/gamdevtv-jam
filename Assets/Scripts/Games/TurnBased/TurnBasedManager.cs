using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

namespace TurnBased
{
    public class TurnBasedManager : GameManager
    {
        //need player card class
        //deck
        //current hand
        //dead cards to reshuffle on deck empty.

        public UITurnBasedManager m_UITurnedBasedManager;
        public TBPlayer m_Player;
        public TBMonster m_Monster;
        public CurrentTurn m_CurrentTurn;

        public List<Card> m_PlayerDeck;

        public void PlayCard(int number)
        {
            Debug.Log($"Playing card {number}");
        }

        private void Start()
        {
            m_Player.SetUpPlayer(m_PlayerDeck, 10, 5);
        }
    }

    public enum CurrentTurn
    {
        PlayerTurn,
        MonsterTurn
    }

    public enum CardType
    {
        Attack,
        Defence,
        EffectOnly
    }

    [System.Serializable]
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

    static class MyExtensions
    {
        private static System.Random rng = new System.Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            if (list == null)
                return;
            int n = list.Count;
            if (n < 1)
                return;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}