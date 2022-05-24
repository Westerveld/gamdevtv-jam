using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIOvenObject : MonoBehaviour
{
    public GameObject m_TimerObj;
    public TMP_Text m_CookingTime;
    public GameObject m_TimerCompleteObj;
  
    public void StartTimer()
    {
        m_TimerCompleteObj.SetActive(false);
        m_TimerObj.SetActive(true);
    }

    public void SetTimer(float time)
    {
        string sTime = time.ToString("0");
        m_CookingTime.text = sTime;
    }

    public void OvenReady()
    {
        m_TimerCompleteObj.SetActive(true);
        m_TimerObj.SetActive(false);
    }

    public void TurnOffUI()
    {
        m_TimerCompleteObj.SetActive(false);
        m_TimerObj.SetActive(false);
    }
}
