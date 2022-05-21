using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemyHealthBar : MonoBehaviour
{
    public float Y_Offset = 250.0f;
    public float Z_Offset = 30.0f;

    public Transform m_MonsterTrans;

    public int m_MonsterMaxHealth;
    public int m_MonsterHealth;

    public Image m_MonsterHealthImage;

    [Range(0,1.5f)]
    public float m_LifeTime = 0.5f;

    // Update is called once per frame
    void Update()
    {
        if (m_MonsterTrans)
        {
            transform.position = new Vector3(m_MonsterTrans.position.x, Y_Offset, m_MonsterTrans.position.z + Z_Offset);
        }
    }   

    public void SetMonsterMaxHealth(int health)
    {
        m_MonsterMaxHealth = health;
    }

    public void SetMonsterHealth(int health)
    {
        m_MonsterHealth = health;
    }

    public void SetMonsterTransform(Transform trans)
    {
        m_MonsterTrans = trans;
    }

    private void SetMonsterHealthBar()
    {
        m_MonsterHealthImage.fillAmount = (float)m_MonsterHealth / (float)m_MonsterMaxHealth;
        if(m_MonsterHealth == 0)
        {
            StartCoroutine(DisableHealthBar());
        }
    }

    private IEnumerator DisableHealthBar()
    {
        yield return new WaitForSeconds(m_LifeTime);
        m_MonsterTrans = null;
        gameObject.SetActive(false);
    }
}
