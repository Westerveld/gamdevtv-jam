using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDatingManager : MonoBehaviour
{
    public float m_CurrentFillAmount;
    public float m_DesiredFillAmount;
    public Image m_UIFillBar;

    public float m_FillTime;
    public float m_FillSpeed;

    public Sprite[] m_FaceSprites;
    public Image m_FaceImage;

    public float m_SmileyYMin;
    public float m_SmileyYMax;

    public void SetUpUI(float fillAmount)
    {
        m_CurrentFillAmount = fillAmount;
        SetFillAmount(fillAmount);
    }

    public void StartFillAmount(float fillAmount)
    {
        m_DesiredFillAmount = fillAmount;
        StartCoroutine(FillInTime());
    }

    private IEnumerator FillInTime()
    {
        m_FillTime = 0;
        while(m_FillTime < 1)
        {
            m_FillTime += m_FillSpeed * Time.deltaTime;
            SetFillAmount(Mathf.Lerp(m_CurrentFillAmount, m_DesiredFillAmount, m_FillTime));
            yield return null;
        }
        m_CurrentFillAmount = m_DesiredFillAmount;
    }

    public void SetFillAmount(float fillAmount)
    {
        m_UIFillBar.fillAmount = fillAmount;
        if(fillAmount < 0.5f)
        {
            m_FaceImage.sprite = m_FaceSprites[0];
        }
        else if(fillAmount < 0.99f)
        {
            m_FaceImage.sprite = m_FaceSprites[1];
        }
        else
        {
            m_FaceImage.sprite = m_FaceSprites[2];
        }
        m_FaceImage.transform.localPosition = new Vector3(60, Mathf.Lerp(m_SmileyYMin, m_SmileyYMax, fillAmount), 0);
    }

}
