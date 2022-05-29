using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBMonster : MonoBehaviour
{
    public int m_MaxHealth;
    public int m_Health;
    public int m_Armour;

    public MonsterDecision m_Decision;

    [System.Serializable]
    public class MonsterDecision
    {
        public enum TurnDecision
        {
            Attack,
            Defend,
            Buff
        }

        public TurnDecision m_TurnDecision;
        public int m_BuffAmount = 0;
        public int m_BaseValue = 0;

        public void PickRandomDecision()
        {
           int choice = Random.Range(0, 9);
            m_TurnDecision = (TurnDecision)choice;
        }
    }

    public void SetUpMonster(int health)
    {
        m_Health = (health != 0) ? health : m_MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        m_Health -= damage;
        if(m_Health <= 0)
        {
            Die();    
        }
    }
    
    public void GetMonsterDecision()
    {
        m_Decision.PickRandomDecision();
    }

    public void Die()
    {
        //put die animation here
        //put end game here
    }
}
