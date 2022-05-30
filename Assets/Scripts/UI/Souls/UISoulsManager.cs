using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISoulsManager : MonoBehaviour
{
    #region Boss
    public float m_BossMaxHealth;
    public float m_BossHealth;

    public Image m_BossHealthImage;
    public Image m_BossMovingHealthImage;

    #endregion

    #region Player
    public float m_PlayerMaxHealth;
    public float m_PlayerHealth;


    public Image m_PlayerHealthImage;
    public Image m_PlayerMovingHealthImage;

    public float m_PlayerMaxStamina;
    public float m_PlayerStamina;

    public Image m_PlayerStaminaImage;
    public Image m_PlayerMovingStaminaImage;

    #endregion


    public void SetBossMaxHealth(float health)
    {
        m_BossMaxHealth = health;
        SetBossHealth(health);
    }

    public void SetBossHealth(float health)
    {
        m_BossHealth = health;
        SetBossUIHealth(health);
    }

    private void SetBossUIHealth(float health)
    {
        m_BossHealthImage.fillAmount = m_BossHealth / m_BossMaxHealth;
    }

    public void SetPlayerMaxHealth(float health)
    {
        m_PlayerMaxHealth = health;
        SetPlayerHealth(health);
    }

    public void SetPlayerHealth(float health)
    {
        m_PlayerHealth = health;
        SetPlayerUIHealth(health);
    }

    private void SetPlayerUIHealth(float health)
    {
        m_PlayerHealthImage.fillAmount = m_PlayerHealth / m_PlayerMaxHealth;
    }

    public void SetPlayerMaxStamina(float stamina)
    {
        m_PlayerMaxStamina = stamina;
        SetPlayerStamina(stamina);
    }

    public void SetPlayerStamina(float stamina)
    {
        m_PlayerStamina = stamina;
        SetPlayerUIStamina(stamina);
    }

    private void SetPlayerUIStamina(float stamina)
    {
        m_PlayerStaminaImage.fillAmount = m_PlayerStamina / m_PlayerMaxStamina;
    }
}
