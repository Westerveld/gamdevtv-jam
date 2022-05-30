using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class UIRunnerManager : MonoBehaviour
{
    public RectTransform m_Track;
    public RectTransform m_MovingPointer;

    public float X_MinValue = -820.0f; //used to offset the goal pointer;
    public float X_MaxValue = 820.0f;

    public float m_MaxDistance;

    public GameObject m_ShieldImage;
    public TMP_Text m_ShieldText;

    //TEST CODE 
    /*public float distance = 0;

    private void Start()
    {
        m_MaxDistance = 10000;
    }

    private void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            distance += 130;
            SetPlayerProgress(distance);
        }
    }*/

    public void HideShield()
    {
        m_ShieldImage.SetActive(false);
    }

    public void SetShieldAmount(int amount)
    {
        if(amount == 0)
        {
            HideShield();
        }
        m_ShieldText.text = amount.ToString();
    }

    public void SetUpShield(int amount)
    {
        if (amount > 0)
        {
            m_ShieldImage.SetActive(true);
            SetShieldAmount(amount);
        }
        else
        {
            HideShield();
        }
    }

    public void SetMaxDistance(float maxDistance)
    {
        m_MaxDistance = maxDistance;
    }

    public void SetPlayerProgress(float distance)
    {
        float percentProgress = distance / m_MaxDistance;

        float uiPosition = Mathf.Lerp(X_MinValue,X_MaxValue,percentProgress);

        m_MovingPointer.localPosition = new Vector3 (uiPosition,m_MovingPointer.localPosition.y,0);

    }
}
