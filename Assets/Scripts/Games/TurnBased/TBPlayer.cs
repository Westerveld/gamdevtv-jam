using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBased;

public class TBPlayer : MonoBehaviour
{
    public TurnBasedManager m_TurnBasedManager;
    public UITurnBasedManager m_UITurnBasedManager;

    public int m_MaxHealth;
    public int m_Health;
    public int m_Energy;
    public int m_Armour;
    public List<CardData> m_PlayerDeck;
    public List<CardData> m_PlayerHand;
    public List<CardData> m_UsedCards;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerDeck = new List<CardData>();
        m_PlayerHand = new List<CardData>();
        m_UsedCards = new List<CardData>();
    }

    public void SetUpPlayer(List<Card> deck,int health, int energy)
    {
        for(int i = 0; i < deck.Count; i++)
        {
            AddCardToDeck(deck[i]);
        }
        ShuffleDeck();
        m_MaxHealth = health;
        m_Health = m_MaxHealth;
        SetEnergy(energy);
        for(int i = 0; i < 5; i++)
        {
            DrawCard();
        }
    }

    public void TakeDamage(int damage)
    {
        m_Health -= damage;
        if(m_Health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        //save monster persistent health
        m_TurnBasedManager.SavePersistentData();
        //move onto next game
        //put die animation here
    }

    public void AddCardToDeck(Card card)
    {
        CardData newCard = new CardData();
        if (card.m_Value == -1)
        {
            newCard.SetupCard(card.m_Title, card.m_Description, card.m_CardType, card.m_EnergyCost, card.m_Sprite);
        }
        else
        {
            newCard.SetupCard(card.m_Title, card.m_Description, card.m_CardType, card.m_EnergyCost, card.m_Sprite, card.m_Value);
        }
        m_PlayerDeck.Add(newCard);
    }

    public void ShuffleDeck()
    {
        m_PlayerDeck.Shuffle();
    }

    public void DrawCard(int energyCost = 0)
    {
        if (energyCost <= m_Energy && m_PlayerHand.Count < 5)
        {
            RemoveEnergy(energyCost);
            if (m_PlayerDeck.Count > 0)
            {
                m_PlayerHand.Add(m_PlayerDeck[0]);
                m_UITurnBasedManager.CreateDrawnCard(m_PlayerDeck[0]);
                m_PlayerDeck.RemoveAt(0);
            }
            else
            {
                m_PlayerDeck = m_UsedCards;
                m_UsedCards.Clear();
                ShuffleDeck();
                m_PlayerHand.Add(m_PlayerDeck[0]);
                m_UITurnBasedManager.CreateDrawnCard(m_PlayerDeck[0]);
                m_PlayerDeck.RemoveAt(0);
            }
        }
    }

    public void UseCardFromHand(int CardUsed)
    {
        RemoveEnergy(m_PlayerHand[CardUsed].m_EnergyCost);
        UseCard(m_PlayerHand[CardUsed]);
        m_UsedCards.Add(m_PlayerHand[CardUsed]);
        m_PlayerHand.RemoveAt(CardUsed);
    }

    public void SetEnergy(int value)
    {
        m_Energy = value;
        m_UITurnBasedManager.SetEnergyAmount(value.ToString());
    }

    public void RemoveEnergy(int value)
    {
        SetEnergy(m_Energy - value);
    }

    public void IncreaseEnergy(int value)
    {
        SetEnergy(m_Energy + value);
    }

    public void ResetArmour()
    {
        m_Armour = 0;
    }
    public void IncreaseArmour(int value)
    {
        m_Armour += value;
    }

    public void UseCard(CardData card)
    {
        switch (card.m_CardType)
        {
            case CardType.Attack:
                m_TurnBasedManager.m_Monster.TakeDamage(card.m_ValueCost);
                break;
            case CardType.Defence:
                IncreaseArmour(card.m_ValueCost);
                break;
            case CardType.EffectOnly:
                break;
            default:
                break;
        }
    }
}
