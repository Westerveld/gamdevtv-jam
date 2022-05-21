using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISoulsManager : MonoBehaviour
{
    #region Boss
    public int m_BossMaxHealth;
    public int m_BossHealth;

    public Image m_BossHealthImage;
    public Image m_BossMovingHealthImage;

    #endregion

    #region Player
    public int m_PlayerMaxHealth;
    public int m_PlayerHealth;


    public Image m_PlayerHealthImage;
    public Image m_PlayerMovingHealthImage;

    public int m_PlayerMaxStamina;
    public int m_PlayerStamina;

    public Image m_PlayerStaminaImage;
    public Image m_PlayerMovingStaminaImage;
    #endregion


    public void SetBossMaxHealth(int health)
    {
        m_BossMaxHealth = health;
    }

    public void SetBossHealth(int health)
    {
        m_BossHealth = health;
        SetBossUIHealth(health);
    }

    private void SetBossUIHealth(int health)
    {
        m_BossHealthImage.fillAmount = (float)m_BossHealth / (float)m_BossMaxHealth;
    }

    public void SetPlayerMaxHealth(int health)
    {
        m_PlayerMaxHealth = health;
    }

    public void SetPlayerHealth(int health)
    {
        m_PlayerHealth = health;
        SetPlayerUIHealth(health);
    }

    private void SetPlayerUIHealth(int health)
    {
        m_PlayerHealthImage.fillAmount = (float)m_PlayerHealth / (float)m_PlayerMaxHealth;
    }

    public void SetPlayerMaxStamina(int stamina)
    {
        m_PlayerMaxStamina = stamina;
    }

    public void SetPlayerStamina(int stamina)
    {
        m_PlayerStamina = stamina;
        SetPlayerUIStamina(stamina);
    }

    private void SetPlayerUIStamina(int stamina)
    {
        m_PlayerStaminaImage.fillAmount = (float)m_PlayerStamina / (float)m_PlayerMaxStamina;
    }
}
