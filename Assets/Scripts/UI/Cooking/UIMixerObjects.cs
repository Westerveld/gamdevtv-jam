using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMixerObjects : MonoBehaviour
{
    public Image[] m_Ingredients;
    public TMP_Text m_TimerText;
    public GameObject m_MixerReadyObj;
    public Sprite[] m_Sprites;

    public void AddIngredient(int ingredient)
    {
        int _tmpIndex = -1;
        for(int i = 0; i < m_Ingredients.Length; i++)
        {
            if(!m_Ingredients[i].gameObject.activeSelf)
            {
                _tmpIndex = i;
                m_Ingredients[i].sprite = m_Sprites[ingredient];
                m_Ingredients[i].gameObject.SetActive(true);
                break;
            }
        }
    }

    public void StartTimer()
    {
        m_MixerReadyObj.SetActive(false);
        m_TimerText.transform.parent.gameObject.SetActive(true);
    }

    public void SetTimer(float time)
    {
            string sTime = time.ToString("0");
            m_TimerText.text = sTime;
    }

    public void MixerReady()
    {
        m_MixerReadyObj.SetActive(true);
        m_TimerText.transform.parent.gameObject.SetActive(false);
    }



    public void TurnOffUI()
    {
        for(int i = 0; i < m_Ingredients.Length; i++)
        {
            m_Ingredients[i].gameObject.SetActive(false);
        }
        m_MixerReadyObj.SetActive(false);
        m_TimerText.transform.parent.gameObject.SetActive(false);
    }
}
