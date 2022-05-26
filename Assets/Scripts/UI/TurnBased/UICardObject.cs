using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBased;
using TMPro;

public class UICardObject : MonoBehaviour
{
    public CardType m_CardType;

    public int m_CardPosition;

    public TMP_Text m_CardTitle;
    public TMP_Text m_CardDescription;

    public TMP_Text m_CardEnergy;
    public TMP_Text m_CardValue;

    public bool m_Active;


    public void SetUpCard(CardType type, int position, string title, string description, string energy, string value = null)
    {
        m_CardType = type;
        m_CardPosition = position;
        m_CardTitle.text = title;
        m_CardDescription.text = description;
        m_CardEnergy.text = energy;
        if(m_CardValue != null && value != null)
        {
            m_CardValue.text = value;
        }

        m_Active = true;
    }
}
