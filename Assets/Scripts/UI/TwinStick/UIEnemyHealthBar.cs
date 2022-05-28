using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemyHealthBar : MonoBehaviour
{
    public float Y_Offset = 250.0f;
    public float Z_Offset = 30.0f;

    public Transform m_MonsterTrans;

    public float m_MonsterMaxHealth;
    public float m_MonsterHealth;

    public Image m_MonsterHealthImage;

    [Range(0,1.5f)]
    public float m_LifeTime = 0.5f;

    // Update is called once per frame
    void Update()
    {
        if (m_MonsterTrans)
        {
            transform.localPosition = new Vector3(m_MonsterTrans.position.x*100, Y_Offset, (m_MonsterTrans.position.z*100) + Z_Offset);
        }
    }   

    public void SetMonsterMaxHealth(float health)
    {
        m_MonsterMaxHealth = health;
        SetMonsterHealth(health);
    }

    public void SetMonsterHealth(float health)
    {
        m_MonsterHealth = health;
        SetMonsterHealthBar();
    }

    public void SetMonsterTransform(Transform trans)
    {
        m_MonsterTrans = trans;
    }

    private void SetMonsterHealthBar()
    {
        m_MonsterHealthImage.fillAmount = m_MonsterHealth / m_MonsterMaxHealth;
    }

    public void KillMonster()
    {
        m_MonsterTrans = null;
        StartCoroutine(DisableHealthBar());
    }

    private IEnumerator DisableHealthBar()
    {
        yield return new WaitForSeconds(m_LifeTime);
        gameObject.SetActive(false);
    }
}
