using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICustomerObject : MonoBehaviour
{

    public Image m_ProductImage;
    public GameObject m_TimerObject;
    public TMP_Text m_TimerCount;

    public void SetCustomerAsk(Sprite product, string time)
    {
        m_TimerCount.text = time;
        m_ProductImage.sprite = product;
        m_ProductImage.transform.parent.gameObject.SetActive(true);
        m_TimerObject.SetActive(true);
    }

    public void CompleteCustomerOrder()
    {
        m_ProductImage.transform.parent.gameObject.SetActive(false);
        m_TimerObject.SetActive(false);
    }

    public void SetTimer(string time)
    {
        m_TimerCount.text = time;
    }
}
