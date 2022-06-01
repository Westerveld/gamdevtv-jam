using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITwinStickManager : MonoBehaviour
{
    #region Health
    public int m_PlayerHealth;
    public int m_PlayerMaxHealth;
    public Image m_PlayerHealthImage;

    #endregion

    #region Ammo
    public TMP_Text m_AmmoCount;
    public TMP_Text m_AmmoAmount;
    public Image m_AmmoImage;
    public Image m_ReloadImage;
    [Range(1,10)]
    public float m_FadeSpeed = 1.0f;
    #endregion
    
    #region Kills
    public TMP_Text m_KillsNeeded;
    #endregion

    #region WorldSpaceUI
    public GameObject m_EnemyHealthPrefab;
    public Transform m_EnemyHealthBarPool;

    #endregion

    public void SetPlayerMaxHealth(int health)
    {
        m_PlayerMaxHealth = health;
    }

    public void SetPlayerHealth(int health)
    {
        m_PlayerHealth = health;
        SetPlayerHealthUI();
    }

    public void SetAmmoCount(int amount, int max = 50)
    {
        m_AmmoCount.text = amount.ToString();
        m_AmmoAmount.text = "/" + max.ToString();
        m_AmmoImage.fillAmount = amount > 0 ? (float)amount / max : 0f;
    }

    private void SetPlayerHealthUI()
    {
        m_PlayerHealthImage.fillAmount = (float)m_PlayerHealth / (float)m_PlayerMaxHealth;
    }

    public void SetReloadTimerState(float timeTillReload)
    {
        m_ReloadImage.fillAmount = timeTillReload;
        if(timeTillReload >= 1.0f)
        {
            StartCoroutine(FadeReloadImage());
        }
    }

    private IEnumerator FadeReloadImage()
    {
        while(m_ReloadImage.color.a > 0)
        {
            Color _tmpColor = new Color(1, 1, 1, m_ReloadImage.color.a - Time.deltaTime*m_FadeSpeed);
            yield return null;
        }
        m_ReloadImage.fillAmount = 0;
        m_ReloadImage.color = Color.white;
        yield return null;
    }

    public UIEnemyHealthBar AssignEnemyHealthBar(Transform Enemy)
    {
        UIEnemyHealthBar uihp = null;
        for (int i = 0; i < m_EnemyHealthBarPool.childCount; i++)
        {
            if(!m_EnemyHealthBarPool.GetChild(i).gameObject.activeSelf)
            {
                //put health stuff here? or set health bar to enemy to handle health
                uihp = m_EnemyHealthBarPool.GetChild(i).GetComponent<UIEnemyHealthBar>();
                uihp.SetMonsterTransform(Enemy);
                m_EnemyHealthBarPool.GetChild(i).gameObject.SetActive(true);
                return uihp;
            }
        }
        GameObject go = GameObject.Instantiate(m_EnemyHealthPrefab, m_EnemyHealthBarPool);
        uihp = go.GetComponent<UIEnemyHealthBar>();
        uihp.SetMonsterTransform(Enemy);
        return uihp;
    }
    
}
