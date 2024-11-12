using System;
using UnityEngine;

namespace Entity
{
    public class EntityStatBase
    {
        [SerializeField]
        protected float m_StatValue;
        public virtual float GetStatValue() { return m_StatValue; }
        public virtual void SetStatValue(float value) { m_StatValue = value; }
        public virtual void SetStatValue(EntityStatBase stat) { m_StatValue = stat.GetStatValue(); }
        
        //Operators
        public static EntityStatBase operator +(EntityStatBase statA, EntityStatBase statB)
        {
            EntityStatBase newStat = new EntityStatBase();
            newStat.SetStatValue(statA.GetStatValue() + statB.GetStatValue());
            return newStat;
        }
        
        public static EntityStatBase operator -(EntityStatBase statA, EntityStatBase statB)
        {
            EntityStatBase newStat = new EntityStatBase();
            newStat.SetStatValue(statA.GetStatValue() - statB.GetStatValue());
            return newStat;
        }
        
        public static EntityStatBase operator *(EntityStatBase statA, EntityStatBase statB)
        {
            EntityStatBase newStat = new EntityStatBase();
            newStat.SetStatValue(statA.GetStatValue() * statB.GetStatValue());
            return newStat;
        }
        
        public static EntityStatBase operator /(EntityStatBase statA, EntityStatBase statB)
        {
            EntityStatBase newStat = new EntityStatBase();
            newStat.SetStatValue(statA.GetStatValue() / statB.GetStatValue());
            return newStat;
        }
    }

    public class PercentageStat : EntityStatBase
    {
        public PercentageStat() { m_StatValue = 0.0f; }
        
        public PercentageStat(float value)
        {
            if (value < 0 || value > 1)
            {
                Debug.LogError("PercentageStat: Value must be between 0 and 1.");
                return;
            }
            
            m_StatValue = value;
        }
        
        public override void SetStatValue(float value)
        {
            base.SetStatValue(value);
            ClampValue();
        }

        public override void SetStatValue(EntityStatBase stat)
        {
            base.SetStatValue(stat);
            ClampValue();
        }

        public int GetPercentageAsInt()
        {
            return Mathf.RoundToInt(GetStatValue() * 100);
        }

        protected void ClampValue()
        {
            m_StatValue = Mathf.Clamp(m_StatValue, 0f, 1f);
        }
    }
    
    [Serializable]
    public class HealthStat : EntityStatBase
    {
        //#TODO change once we have a data structure for damages
        public void TakeDamage(float physicalDamage, ArmorStat ownerArmorStat, ArmorPenetrationStat attackerArmorPenetrationStat, out float finalDamageTaken)
        {
            float finalOwnerArmor = ownerArmorStat.CalculateFinalArmorValue(attackerArmorPenetrationStat);
            float finalPhysicalDamage = physicalDamage * (1.0f - finalOwnerArmor);
            if (finalPhysicalDamage < 0.0f)
            {
                finalPhysicalDamage = 0.0f;
            }

            finalDamageTaken = finalPhysicalDamage;
            
            float currentHealth = GetStatValue();
            currentHealth -= finalPhysicalDamage;
            if (currentHealth <= 0.0f)
            {
                //#TODO Notify that we lost all health
            }
            
            SetStatValue(currentHealth);
        }
        
        public void TakeDamage(float magicDamage, MagicResistStat ownerMagicResistStat, MagicResistPenetrationStat attackerMagicPenetrationStat, out float finalDamageTaken)
        {
            float finalOwnerMagicResist = ownerMagicResistStat.CalculateFinalMagicResistValue(attackerMagicPenetrationStat);
            float finalMagicDamage = magicDamage * (1.0f - finalOwnerMagicResist);
            if (finalMagicDamage < 0.0f)
            {
                finalMagicDamage = 0.0f;
            }

            finalDamageTaken = finalMagicDamage;
            
            float currentHealth = GetStatValue();
            currentHealth -= finalMagicDamage;
            if (currentHealth <= 0.0f)
            {
                //#TODO Notify that we lost all health
            }
            
            SetStatValue(currentHealth);
        }
    }

    [Serializable]
    public class ManaStat : EntityStatBase
    {
        
    }

    [Serializable]
    public class MoveSpeedStat : EntityStatBase
    {
        
    }

    [Serializable]
    public class ArmorStat : PercentageStat
    {
        public ArmorStat() : base() {}
        public ArmorStat(float value) : base(value) {}
        
        public float CalculateFinalArmorValue(ArmorPenetrationStat armorPenetrationStat)
        {
            float finalArmorValue = GetStatValue() - armorPenetrationStat.GetStatValue();
            if (finalArmorValue < 0.0f)
                return 0.0f;

            return finalArmorValue;
        }
    }

    [Serializable]
    public class MagicResistStat : PercentageStat
    {
        public MagicResistStat() : base() {}
        public MagicResistStat(float value) : base(value) {}
        
        public float CalculateFinalMagicResistValue(MagicResistPenetrationStat magicResistStat)
        {
            float finalMagicResistValue = GetStatValue() - magicResistStat.GetStatValue();
            if (finalMagicResistValue < 0.0f)
                return 0.0f;

            return finalMagicResistValue;
        }
    }

    [Serializable]
    public class LifestealStat : PercentageStat
    {
        public LifestealStat() : base() {}
        public LifestealStat(float value) : base(value) {}
    }

    [Serializable]
    public class SpellVampStat : PercentageStat
    {
        public SpellVampStat() : base() {}
        public SpellVampStat(float value) : base(value) {}
    }

    [Serializable]
    public class AttackRangeStat : EntityStatBase
    {
        
    }

    [Serializable]
    public class PhysicalPowerStat : EntityStatBase
    {
        
    }

    [Serializable]
    public class AbilityPowerStat : EntityStatBase
    {
        
    }

    [Serializable]
    public class AttackSpeedStat : EntityStatBase
    {
        public float CalculateCooldownBetweenAttacks()
        {
            return 1.0f / GetStatValue();
        }
    }

    [Serializable]
    public class ArmorPenetrationStat : PercentageStat
    {

    }

    [Serializable]
    public class MagicResistPenetrationStat : PercentageStat
    {

    }

    [Serializable]
    public class CooldownReductionStat : PercentageStat
    {
        public CooldownReductionStat() : base() {}
        public CooldownReductionStat(float value) : base(value) {}

        public float CalculateFinalCooldownValue(float baseCooldownValue)
        {
            return baseCooldownValue * (1.0f - GetStatValue());
        }
    }

    [Serializable]
    public class CriticalStrikeChanceStat : PercentageStat
    {
        public CriticalStrikeChanceStat() : base() {}
        public CriticalStrikeChanceStat(float value) : base(value) {}
    }
}
