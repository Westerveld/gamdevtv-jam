using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBased;

public class TBPlayer : MonoBehaviour
{
    public int m_MaxHealth;
    public int m_Health;
    public int m_Energy;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpPlayer(List<Card> deck, int health, int energy)
    {
        for(int i = 0; i < deck.Count; i++)
        {
            AddCardToDeck(deck[i]);
        }
        ShuffleDeck();
        m_MaxHealth = m_Health = health;
        m_Energy = energy;
        for(int i = 0; i < 5; i++)
        {
            DrawCard();
        }
    }

    public void AddCardToDeck(Card card)
    {
        CardData newCard = new CardData();
        if (card.m_Value == -1)
        {
            newCard.SetupCard(card.m_Title, card.m_Description, card.m_CardType, card.m_EnergyCost);
        }
        else
        {
            newCard.SetupCard(card.m_Title, card.m_Description, card.m_CardType, card.m_EnergyCost, card.m_Value);
        }
        m_PlayerDeck.Add(newCard);
    }

    public void ShuffleDeck()
    {
        m_PlayerDeck.Shuffle();
    }

    public void DrawCard()
    {
        if (m_PlayerDeck.Count > 0)
        {
            m_PlayerHand.Add(m_PlayerDeck[0]);
            m_PlayerDeck.RemoveAt(0);
        }
        else
        {
            m_PlayerDeck = m_UsedCards;
            m_UsedCards.Clear();
            ShuffleDeck();
            m_PlayerHand.Add(m_PlayerDeck[0]);
            m_PlayerDeck.RemoveAt(0);
        }
    }

    public void UseCardFromHand(int CardUsed)
    {
        m_UsedCards.Add(m_PlayerHand[CardUsed]);
        m_PlayerHand.RemoveAt(CardUsed);
    }
}
