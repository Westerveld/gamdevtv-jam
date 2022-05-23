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

    public void AddIngredient(Sprite ingredient)
    {
        int _tmpIndex = -1;
        for(int i = 0; i < m_Ingredients.Length; i++)
        {
            if(!m_Ingredients[i].gameObject.activeSelf)
            {
                _tmpIndex = i;
                m_Ingredients[i].gameObject.SetActive(true);
                break;
            }
        }
    }

    public void StartTimer(float time)
    {
        string sTime = ((int)time).ToString();
        m_TimerText.text = sTime;
        m_MixerReadyObj.SetActive(false);
        m_TimerText.transform.parent.gameObject.SetActive(true);
    }

    public void SetTimer(float time)
    {
        if (time > 0)
        {
            string sTime = ((int)time).ToString();
            m_TimerText.text = sTime;
        }
        else
        {
            MixerReady();
        }
    }

    private void MixerReady()
    {
        m_MixerReadyObj.SetActive(true);
        m_TimerText.transform.parent.gameObject.SetActive(false);
    }

    public void TurnOffUI()
    {
        m_MixerReadyObj.SetActive(false);
        m_TimerText.transform.parent.gameObject.SetActive(false);
    }
}
