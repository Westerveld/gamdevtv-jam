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

        public int m_PlayerMaxHealth;
        public int m_PlayerEnergy;

        public List<Card> m_PlayerDeck;

        public int m_CardBuff = 1;
        public int m_BuffAmount = 0;

        private bool ending = false;

        public void PlayCard(int number)
        {
            Debug.Log($"Playing card {number}");
        }

        public override void StartGame(float value1 = 0, float value2 = 0)
        {
            base.StartGame(value1, value2);

            if(GameInstance.instance != null)
            {
                m_BuffAmount = GameInstance.instance.GetCompletedGames();
            }

            m_Player.SetUpPlayer(m_PlayerDeck, m_PlayerMaxHealth, m_PlayerEnergy,m_BuffAmount*m_CardBuff);

            //need to use persistent data to set monster health
            m_Monster.SetUpMonster((int)value1);
            m_Monster.GetMonsterDecision();
            m_CurrentTurn = CurrentTurn.PlayerTurn;
        }

        public void UseCard(int CardIndex)
        {
            if (m_Player.m_Energy >= m_Player.m_PlayerHand[CardIndex].m_EnergyCost && m_CurrentTurn == CurrentTurn.PlayerTurn)
            {
                m_UITurnedBasedManager.UpdateOnRemovedCard(CardIndex);
                m_Player.UseCardFromHand(CardIndex);
            }
        }

        public void Died()
        {
            
            if (ending) return;
            ending = true;
            GameInstance.instance.SetPersistantData(gameType, m_Monster.m_Health);
            GameInstance.instance.GameEnd();
        }

        //called by unity event
        public void EndTurn()
        {
            m_CurrentTurn = CurrentTurn.MonsterTurn;
            RunEnemyTurn();
        }

        public void RunEnemyTurn()
        {
            m_Monster.ActTurn();
        }

        public void EndEnemyTurn()
        {
            m_Player.NewTurn(5);
            m_CurrentTurn = CurrentTurn.PlayerTurn;
            m_Monster.GetMonsterDecision();
        }

        public void CompleteGame()
        {
            if (ending) return;
            ending = true;
            GameInstance.instance.SetGameComplete(gameType);
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

        public Sprite m_Sprite;

        public void SetupCard(string title, string description, CardType type, int energy, Sprite sprite, int value = -1, int buffAmount = 0)
        {
            m_Title = title;
            if(buffAmount != 0)
            {
                if (description.Contains(value.ToString()))
                {
                    description = description.Replace(value.ToString(), (value + buffAmount).ToString());
                }
            }
            m_Description = description;
            m_CardType = type;
            m_EnergyCost = energy;
            m_Sprite = sprite;
            m_ValueCost = value + buffAmount;
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