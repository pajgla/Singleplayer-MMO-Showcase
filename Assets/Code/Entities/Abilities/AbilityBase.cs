using System.Collections.Generic;
using UnityEngine;

namespace Entity.Abilities
{
    public enum EAbilityCostType
    {
        Mana,
        Health
    }

    public enum EAbilityType
    {
        BasicAttack,
        Passive,
        Spell,
        Ultimate
    }
    
    public abstract class AbilityBase : ScriptableObject
    {
        [SerializeField] protected string m_AbilityName;
        [SerializeField] protected string m_AbilityDescription;
        [SerializeField] protected string m_AbilityThumbnailPath;

        [SerializeField] protected EAbilityType m_AbilityType;
        [SerializeField] protected EAbilityCostType m_CostType;
        [SerializeField] protected float m_CostAmount;
        [SerializeField] protected float m_Cooldown;
        
        [SerializeField] protected float m_PhysicalDamage = 0.0f;
        [SerializeField] protected float m_MagicDamage = 0.0f;

        protected float m_CurrentCooldown;
        
        public abstract void PrecastAbility(EntityBase abilityOwner);
        public abstract void CastAbility(EntityBase abilityOwner);
        public abstract void CancelAbility(EntityBase abilityOwner);

        public virtual void Update()
        {
            UpdateCooldown();
        }

        public virtual void UpdateCooldown()
        {
            m_CurrentCooldown -= Time.deltaTime;
            if (m_CurrentCooldown <= 0)
                m_CurrentCooldown = 0;
        }
        
        public virtual void CalculateAbilityDamage(out float physicalDamage, out float magicDamage)
        {
            physicalDamage = m_PhysicalDamage;
            magicDamage = m_MagicDamage;
        }

        public virtual void StartAbilityCooldown(EntityBase abilityOwner)
        {
            if (m_CurrentCooldown > 0.0f)
            {
                Debug.LogError("This ability is already on cooldown.");
                return;
            }

            if (abilityOwner == null)
            {
                Debug.LogError("Provided null abilityOwner");
                return;
            }

            CooldownReductionStat cooldownReductionStat = abilityOwner.GetEntityStat<CooldownReductionStat>();
            if (cooldownReductionStat == null)
            {
                Debug.LogError($"Entity {abilityOwner.m_EntityDataset?.m_EntityName} is missing CooldownReductionStat!");
            }
            
            m_CurrentCooldown = cooldownReductionStat.CalculateFinalCooldownValue(m_Cooldown);
        }
        
        public virtual bool CanUseAbility(EntityBase abilityOwner)
        {
            return m_CurrentCooldown <= 0.0f;
        }
    }
}
