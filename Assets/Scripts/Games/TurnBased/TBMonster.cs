using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBased;

public class TBMonster : MonoBehaviour
{
    public TurnBasedManager m_TurnBasedManager;
    public UITurnBasedManager m_UITurnBasedManager;

    public int m_MaxHealth;
    public int m_Health;
    public int m_Armour;

    public MonsterDecision m_Decision;

    public Animator anim;

    private bool dying = false;
    private static readonly int attack = Animator.StringToHash("Attack");
    private static readonly int block = Animator.StringToHash("Block");
    private static readonly int effect = Animator.StringToHash("Effect");
    private static readonly int hit = Animator.StringToHash("Hit");
    private static readonly int die = Animator.StringToHash("Die");

    public AudioClip hitSFX, dieSFX, armorHitSFX, attackSFX;

    [System.Serializable]
    public class MonsterDecision
    {
        public enum TurnDecision
        {
            Attack = 0,
            Defend,
            Buff
        }

        public TurnDecision m_TurnDecision;
        public int m_BuffAmount = 0;
        public int m_BaseValue = 0;

        public void PickRandomDecision()
        {
           int choice = Random.Range(0, 3);
           m_TurnDecision = (TurnDecision)choice;

            switch (m_TurnDecision)
            {
                case TurnDecision.Attack:
                    m_BaseValue = Random.Range(1, 4);
                    break;
                case TurnDecision.Defend:
                    m_BaseValue = Random.Range(2, 5);
                    break;
                case TurnDecision.Buff:
                    break;
                default:
                    break;
            }
        }
    }

    public void SetUpMonster(int health)
    {
        m_Health = (health != 0) ? health : m_MaxHealth;
        anim = GetComponent<Animator>();
        m_UITurnBasedManager.SetMonsterHealthAmount(m_Health, m_MaxHealth);
    }

    public void ActTurn()
    {
        ResetArmour();
        m_UITurnBasedManager.HideMonsterDecision();
        switch (m_Decision.m_TurnDecision)
        {
            case MonsterDecision.TurnDecision.Attack:
                anim.SetTrigger(attack);
                AudioManager.instance?.PlaySFX(attackSFX);
                m_TurnBasedManager.m_Player.TakeDamage(m_Decision.m_BaseValue + m_Decision.m_BuffAmount);
                break;
            case MonsterDecision.TurnDecision.Defend:
                anim.SetTrigger(block);
                IncreaseArmour(m_Decision.m_BaseValue + m_Decision.m_BuffAmount);
                break;
            case MonsterDecision.TurnDecision.Buff:
                anim.SetTrigger(effect);
                IncreaseMonsterBuff();
                break;
            default:
                break;
        }

        //to be after animation
        EndMonsterTurn();
    }

    private void EndMonsterTurn()
    {
        m_TurnBasedManager.EndEnemyTurn();
    }

    public void TakeDamage(int damage)
    {
        anim.SetTrigger(hit);
        if(m_Armour > 0)
        {
            int tmpDmg = damage;
            damage -= m_Armour;
            DecreaseArmour(tmpDmg);
            if (damage <= 0)
            {
                AudioManager.instance?.PlaySFX(armorHitSFX);
                return;
            }
        }
        m_Health -= damage;
        m_UITurnBasedManager.SetMonsterHealthAmount(m_Health, m_MaxHealth);
        if (m_Health <= 0 && !dying)
        {
            dying = true;
            anim.SetTrigger(die);
            AudioManager.instance?.PlaySFX(dieSFX);
            StartCoroutine(WaitThenDie());
            return;
        }
        AudioManager.instance?.PlaySFX(hitSFX);
    }

    private void IncreaseMonsterBuff()
    {
        m_Decision.m_BuffAmount++;
        m_UITurnBasedManager.SetMonsterBuff(m_Decision.m_BuffAmount);
    }

    private void ResetArmour()
    {
        m_Armour = 0;
        m_UITurnBasedManager.RemoveMonsterArmour();
    }

    private void DecreaseArmour(int value)
    {
        m_Armour -= value;
        m_UITurnBasedManager.SetMonsterArmour(m_Armour);
        if (m_Armour <= 0)
        {
            ResetArmour();
        }
    }

    private void IncreaseArmour(int value)
    {
        m_Armour += value;
        m_UITurnBasedManager.SetMonsterArmour(m_Armour);
    }

    public void GetMonsterDecision()
    {
        m_Decision.PickRandomDecision();
        m_UITurnBasedManager.ShowMonsterDecision((int)m_Decision.m_TurnDecision, m_Decision.m_BaseValue + m_Decision.m_BuffAmount);
    }

    private IEnumerator WaitThenDie()
    {
        yield return new WaitForSeconds(2.2f);
        //put die animation here
        //put end game here
        m_TurnBasedManager.CompleteGame();
    }

    private void SmashImpact()
    {
        
    }
}
