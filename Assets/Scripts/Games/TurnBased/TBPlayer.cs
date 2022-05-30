using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBased;

public class TBPlayer : MonoBehaviour
{
    public TurnBasedManager m_TurnBasedManager;
    public UITurnBasedManager m_UITurnBasedManager;

    public int m_MaxHealth;
    public int m_Health;
    public int m_Energy;
    public int m_Armour;
    public List<CardData> m_PlayerDeck;
    public List<CardData> m_PlayerHand;
    public List<CardData> m_UsedCards;

    public Animator anim;
    private bool dying = false;
    private static readonly int animID_Die = Animator.StringToHash("Die");
    private static readonly int animID_Block = Animator.StringToHash("Block");
    private static readonly int animID_Attack = Animator.StringToHash("Attack");
    private static readonly int animID_Effect = Animator.StringToHash("Effect");
    private static readonly int animID_Hit = Animator.StringToHash("Hit");

    public AudioClip hitSFX, armorHitSFX, drawCard, useCard, attackNoise, armourGainSFX, dieSFX;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerDeck = new List<CardData>();
        m_PlayerHand = new List<CardData>();
        m_UsedCards = new List<CardData>();
        anim = GetComponent<Animator>();
    }

    public void SetUpPlayer(List<Card> deck,int health, int energy, int buffAmount)
    {
        for(int i = 0; i < deck.Count; i++)
        {
            AddCardToDeck(deck[i], buffAmount);
        }
        ShuffleDeck();
        m_MaxHealth = health;
        m_Health = m_MaxHealth;
        m_UITurnBasedManager.SetPlayerHealthAmount(m_Health, m_MaxHealth);
        SetEnergy(energy);
        for(int i = 0; i < 5; i++)
        {
            DrawCard();
        }
    }

    public void TakeDamage(int damage)
    {
        if(!anim.GetBool(animID_Hit))
            anim.SetTrigger(animID_Hit);
        
        if (m_Armour > 0)
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
        m_UITurnBasedManager.SetPlayerHealthAmount(m_Health, m_MaxHealth);
        if (m_Health <= 0 && !dying)
        {
            dying = true;
            anim.SetTrigger(animID_Die);
            StartCoroutine(WaitAndDie());
            AudioManager.instance?.PlaySFX(dieSFX);
            return;
        }
        AudioManager.instance?.PlaySFX(hitSFX);
    }

    IEnumerator WaitAndDie()
    {
        //Wait for animation
        yield return new WaitForSeconds(2f);
        //save monster persistent health
        m_TurnBasedManager.Died();
        //move onto next game
        //put die animation here
    }

    public void AddCardToDeck(Card card, int buffAmount)
    {
        CardData newCard = new CardData();
        if (card.m_Value == -1)
        {
            newCard.SetupCard(card.m_Title, card.m_Description, card.m_CardType, card.m_EnergyCost, card.m_Sprite);
        }
        else
        {
            newCard.SetupCard(card.m_Title, card.m_Description, card.m_CardType, card.m_EnergyCost, card.m_Sprite, card.m_Value, buffAmount);
        }
        m_PlayerDeck.Add(newCard);
    }

    public void ShuffleDeck()
    {
        m_PlayerDeck.Shuffle();
    }

    public void DrawCard(int energyCost = 0)
    {
        if (energyCost <= m_Energy && m_PlayerHand.Count < 5)
        {
            AudioManager.instance?.PlaySFX(drawCard);
            RemoveEnergy(energyCost);
            if (m_PlayerDeck.Count > 0)
            {
                m_PlayerHand.Add(m_PlayerDeck[0]);
                m_UITurnBasedManager.CreateDrawnCard(m_PlayerDeck[0]);
                m_PlayerDeck.RemoveAt(0);
            }
            else
            {
                for(int i = 0; i < m_UsedCards.Count; i++)
                {
                    m_PlayerDeck.Add(m_UsedCards[i]);

                }
                m_UsedCards.Clear();
                ShuffleDeck();
                m_PlayerHand.Add(m_PlayerDeck[0]);
                m_UITurnBasedManager.CreateDrawnCard(m_PlayerDeck[0]);
                m_PlayerDeck.RemoveAt(0);
            }
        }
    }

    public void NewTurn(int energyVal)
    {
        int len = 5 - m_PlayerHand.Count;
        for(int i = 0; i < len; i++)
        {
            DrawCard();
        }
        IncreaseEnergy(energyVal);
        ResetArmour();
    }

    public void UseCardFromHand(int CardUsed)
    {
        AudioManager.instance?.PlaySFX(useCard);
        RemoveEnergy(m_PlayerHand[CardUsed].m_EnergyCost);
        UseCard(m_PlayerHand[CardUsed]);
        m_UsedCards.Add(m_PlayerHand[CardUsed]);
        m_PlayerHand.RemoveAt(CardUsed);
    }

    public void SetEnergy(int value)
    {
        m_Energy = value;
        m_UITurnBasedManager.SetEnergyAmount(value.ToString());
    }

    public void RemoveEnergy(int value)
    {
        SetEnergy(m_Energy - value);
    }

    public void IncreaseEnergy(int value)
    {
        SetEnergy(m_Energy + value);
    }

    public void ResetArmour()
    {
        m_Armour = 0;
        m_UITurnBasedManager.RemovePlayerArmour();
    }

    public void DecreaseArmour(int value)
    {
        m_Armour -= value;
        m_UITurnBasedManager.SetPlayerArmour(m_Armour);
        AudioManager.instance?.PlaySFX(armourGainSFX);
        if (m_Armour <= 0)
        {
            ResetArmour();
        }
    }

    public void IncreaseArmour(int value)
    {
        m_Armour += value;
        m_UITurnBasedManager.SetPlayerArmour(m_Armour);
    }

    public void UseCard(CardData card)
    {
        switch (card.m_CardType)
        {
            case CardType.Attack:
                if(!anim.GetBool(animID_Attack))
                    anim.SetTrigger(animID_Attack);
                AudioManager.instance?.PlaySFX(attackNoise);
                m_TurnBasedManager.m_Monster.TakeDamage(card.m_ValueCost);
                break;
            case CardType.Defence:
                if(!anim.GetBool(animID_Block))
                    anim.SetTrigger(animID_Block);
                IncreaseArmour(card.m_ValueCost);
                break;
            case CardType.EffectOnly:
                anim.SetTrigger(animID_Effect);
                break;
            default:
                break;
        }
    }
}
