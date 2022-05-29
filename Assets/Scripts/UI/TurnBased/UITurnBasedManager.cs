using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TurnBased;
using TMPro;

public class UITurnBasedManager : MonoBehaviour
{
    //public Transform[] m_CardPositions;
    public Transform m_CardHolder;
    public UICardObject[] m_CurrentCards;

    public TMP_Text m_EnergyAmount;


    public void CreateDrawnCard(CardData card)
    {
        int cardPosition = GetCurrentCardIndex();
        for(int i = 0; i < m_CurrentCards.Length; i++)
        {
            if(!m_CurrentCards[i].m_Active)
            {
                if (card.m_ValueCost == -1)
                {
                    m_CurrentCards[i].SetUpCard(card.m_CardType, cardPosition, card.m_Title, card.m_Description, card.m_EnergyCost.ToString(), card.m_Sprite);
                    return;
                }
                else
                {
                    m_CurrentCards[i].SetUpCard(card.m_CardType, cardPosition, card.m_Title, card.m_Description, card.m_EnergyCost.ToString(), card.m_Sprite, card.m_ValueCost.ToString());
                    return;
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
            if (m_CurrentCards[i].m_CardPosition > removedCardPosition && m_CurrentCards[i].gameObject.activeSelf)
            {
                m_CurrentCards[i - 1].SetUpCard(m_CurrentCards[i]);
                m_CurrentCards[i - 1].m_CardPosition--;
            }
        }
        for(int i = m_CurrentCards.Length-1; i > -1; i--)
        {
            if(m_CurrentCards[i].m_Active)
            {
                m_CurrentCards[i].m_Active = false;
                m_CurrentCards[i].gameObject.SetActive(false);
                return;
            }
        }
    }

    public void OnHighlightCard(Transform cardTrans)
    {
        cardTrans.SetAsLastSibling();
        cardTrans.localScale = Vector3.one * 1.25f;
    }

    public void OnUnhighlightCard(UICardObject cardObj)
    {
        cardObj.transform.SetSiblingIndex(cardObj.m_CardPosition);
        cardObj.transform.localScale = Vector3.one;
    }

    public void SetEnergyAmount(string Amount)
    {
        m_EnergyAmount.text = Amount;
    }
}
