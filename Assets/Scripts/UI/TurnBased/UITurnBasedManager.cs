using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TurnBased;

public class UITurnBasedManager : MonoBehaviour
{
    public GameObject m_AttackCardPrefab;
    public GameObject m_DefenceCardPrefab;
    public GameObject m_EventCardPrefab;

    public Transform[] m_CardPositions;
    public UICardObject[] m_CurrentCards;

    public void CreateDrawnCard(CardData card)
    {
        int cardPosition = GetCurrentCardIndex();
        for(int i = 0; i < m_CurrentCards.Length; i++)
        {
            if(!m_CurrentCards[i].m_Active)
            {
                if (card.m_ValueCost == -1)
                {
                    m_CurrentCards[i].SetUpCard(card.m_CardType, cardPosition, card.m_Title, card.m_Description, card.m_EnergyCost.ToString());
                }
                else
                {
                    m_CurrentCards[i].SetUpCard(card.m_CardType, cardPosition, card.m_Title, card.m_Description, card.m_EnergyCost.ToString(),card.m_ValueCost.ToString());
                }
            }
        }
    }

    public int GetCurrentCardIndex()
    {
        int pos = -1;
        for(int i = 0; i < m_CurrentCards.Length; i++)
        {
            if(m_CurrentCards[i].m_Active)
            {
                if(m_CurrentCards[i].m_CardPosition > pos)
                {
                    pos = m_CurrentCards[i].m_CardPosition;
                }
            }
        }
        return pos + 1;
    }

    public void UpdateOnRemovedCard(int removedCardPosition)
    {
        for(int i = 0; i < m_CurrentCards.Length; i++)
        {
            if (m_CurrentCards[i].m_CardPosition > removedCardPosition)
            {
                m_CurrentCards[i].m_CardPosition--;
            }
        }
    }
}
