using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TurnBased;
using TMPro;

public class UICardObject : MonoBehaviour
{
    public CardType m_CardType;

    public int m_CardPosition;

    public TMP_Text m_CardTitle;
    public TMP_Text m_CardDescription;
    public Image m_CardImage;

    public TMP_Text m_CardEnergy;
    public TMP_Text m_CardValue;
    public Image m_ValueImage;

    public bool m_Active;
    public Sprite m_AttackIcon;
    public Sprite m_DefenceIcon;

    public void SetUpCard(CardType type, int position, string title, string description, string energy, Sprite cardSprite, string value = null)
    {
        m_CardType = type;
        m_CardPosition = position;
        m_CardTitle.text = title;
        m_CardDescription.text = description;
        m_CardEnergy.text = energy;
        m_CardImage.sprite = cardSprite;
        if (m_CardValue != null && value != null)
        {
            m_ValueImage.gameObject.SetActive(true);
            m_CardValue.gameObject.SetActive(true);
            m_CardValue.text = value;
        }
        else
        {
            m_ValueImage.gameObject.SetActive(false);
            m_CardValue.gameObject.SetActive(false);
            m_CardValue.text = "";
        }

        switch (m_CardType)
        {
            case CardType.Attack:
                m_ValueImage.sprite = m_AttackIcon;
                break;
            case CardType.Defence:
                m_ValueImage.sprite = m_DefenceIcon;
                break;
        }

        m_Active = true;
        gameObject.SetActive(true);
    }

    public void SetUpCard(UICardObject card)
    {
        m_CardType = card.m_CardType;
        m_CardPosition = card.m_CardPosition;
        m_CardTitle.text = card.m_CardTitle.text;
        m_CardDescription.text = card.m_CardDescription.text;
        m_CardEnergy.text = card.m_CardEnergy.text;
        m_CardImage.sprite = card.m_CardImage.sprite;
        if (!string.IsNullOrEmpty(card.m_CardValue.text))
        {
            m_ValueImage.gameObject.SetActive(true);
            m_CardValue.gameObject.SetActive(true);
            m_CardValue.text = card.m_CardValue.text;
        }
        else
        {
            m_ValueImage.gameObject.SetActive(false);
            m_CardValue.gameObject.SetActive(false);
            m_CardValue.text = "";
        }

        switch (m_CardType)
        {
            case CardType.Attack:
                m_ValueImage.sprite = m_AttackIcon;
                break;
            case CardType.Defence:
                m_ValueImage.sprite = m_DefenceIcon;
                break;
        }

        m_Active = true;
    }
}
