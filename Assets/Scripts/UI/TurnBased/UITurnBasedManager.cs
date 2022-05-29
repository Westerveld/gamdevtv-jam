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

    public Image m_PlayerHealthBar;
    public TMP_Text m_PlayerHealthText;
    public GameObject m_PlayerArmourImage;
    public TMP_Text m_PlayerArmorCount;

    public Image m_MonsterHealthBar;
    public TMP_Text m_MonsterHealthText;
    public GameObject m_MonsterArmourImage;
    public TMP_Text m_MonsterArmorCount;

    public GameObject m_MonsterDecisionObject;
    public Image m_MonsterDecisionImage;
    public Sprite[] m_MonsterDecisionSprites;
    public TMP_Text m_MonsterDecisionValue;

    public TMP_Text m_MonsterBuff1;
    public TMP_Text m_MonsterBuff2;

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
                m_CurrentCards[i].transform.SetSiblingIndex(m_CurrentCards[i].m_CardPosition); //bug fix when using end card then drawing more then card pos
                m_CurrentCards[i].transform.localScale = Vector3.one; //bug fix when using end card then drawing
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

    public void SetPlayerHealthAmount(int value, int maxVal)
    {
        m_PlayerHealthBar.fillAmount = (float)value / (float)maxVal;
        m_PlayerHealthText.text = value.ToString() + "/" + maxVal.ToString();
    }

    public void SetPlayerArmour(int val)
    {
        m_PlayerArmourImage.SetActive(true);
        m_PlayerArmorCount.text = val.ToString();
    }

    public void RemovePlayerArmour()
    {
        m_PlayerArmourImage.SetActive(false);
    }

    public void SetMonsterHealthAmount(int value, int maxVal)
    {
        m_MonsterHealthBar.fillAmount = (float)value / (float)maxVal;
        m_MonsterHealthText.text = value.ToString() + "/" + maxVal.ToString();
    }

    public void SetMonsterArmour(int val)
    {
        m_MonsterArmourImage.SetActive(true);
        m_MonsterArmorCount.text = val.ToString();
    }

    public void RemoveMonsterArmour()
    {
        m_MonsterArmourImage.SetActive(false);
    }

    public void ShowMonsterDecision(int decision, int value)
    {
        m_MonsterDecisionImage.sprite = m_MonsterDecisionSprites[decision];
        if(decision != 2)
        {
            m_MonsterDecisionValue.text = value.ToString();
        }
        else
        {
            m_MonsterDecisionValue.text = "";
        }
        m_MonsterDecisionObject.SetActive(true);
    }

    public void HideMonsterDecision()
    {
        m_MonsterDecisionObject.SetActive(false);
    }

    public void SetMonsterBuff(int val)
    {
        m_MonsterBuff1.transform.parent.gameObject.SetActive(true);
        m_MonsterBuff1.text = "+" + val.ToString();
        m_MonsterBuff2.transform.parent.gameObject.SetActive(true);
        m_MonsterBuff2.text = "+" + val.ToString();
    }
}
