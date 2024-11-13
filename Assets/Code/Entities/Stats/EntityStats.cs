using System;
using UnityEngine;

namespace Entity
{
    [Serializable]
    public class EntityStatBase
    {
        [SerializeField]
        protected float m_StatValue;
        public virtual float GetStatValue() { return m_StatValue; }
        public virtual void SetStatValue(float value) { m_StatValue = value; }
        public virtual void SetStatValue(EntityStatBase stat) { m_StatValue = stat.GetStatValue(); }
        
        //Constructors
        public EntityStatBase(EntityStatBase other)
        {
            SetStatValue(other.GetStatValue());
        }

        public EntityStatBase()
        {
            m_StatValue = 0;
        }
        
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
            if (value < 0.0f || value > 1.0f)
            {
                Debug.LogError("PercentageStat: Value must be between 0 and 1.");
                return;
            }
            
            m_StatValue = value;
        }
        
        public PercentageStat(PercentageStat other) : base(other) {}
        
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
        public HealthStat() : base() {}
        public HealthStat(HealthStat other) : base(other) {}
        
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
        public ManaStat() : base() {}
        public ManaStat(ManaStat other) : base(other) {}
    }

    [Serializable]
    public class MoveSpeedStat : EntityStatBase
    {
        public MoveSpeedStat() : base() {}
        public MoveSpeedStat(MoveSpeedStat other) : base(other) {}
    }

    [Serializable]
    public class ArmorStat : PercentageStat
    {
        public ArmorStat() : base() {}
        public ArmorStat(ArmorStat other) : base(other) {}
        
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
        public MagicResistStat(MagicResistStat other) : base(other) {}
        
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
        public LifestealStat(LifestealStat other) : base(other) {}
    }

    [Serializable]
    public class SpellVampStat : PercentageStat
    {
        public SpellVampStat() : base() {}
        public SpellVampStat(SpellVampStat other) : base(other) {}
    }

    [Serializable]
    public class AttackRangeStat : EntityStatBase
    {
        public AttackRangeStat() : base() {}
        public AttackRangeStat(AttackRangeStat other) : base(other) {}
    }

    [Serializable]
    public class PhysicalPowerStat : EntityStatBase
    {
        public PhysicalPowerStat() : base() {}
        public PhysicalPowerStat(PhysicalPowerStat other) : base(other) {}
    }

    [Serializable]
    public class AbilityPowerStat : EntityStatBase
    {
        public AbilityPowerStat() : base() {}
        public AbilityPowerStat(AbilityPowerStat other) : base(other) {}
    }

    [Serializable]
    public class AttackSpeedStat : EntityStatBase
    {
        public AttackSpeedStat() : base() {}
        public AttackSpeedStat(AttackSpeedStat other) : base(other) {}
        
        public float CalculateCooldownBetweenAttacks()
        {
            return 1.0f / GetStatValue();
        }
    }

    [Serializable]
    public class ArmorPenetrationStat : PercentageStat
    {
        public ArmorPenetrationStat() : base() {}
        public ArmorPenetrationStat(ArmorPenetrationStat other) : base(other) {}
    }

    [Serializable]
    public class MagicResistPenetrationStat : PercentageStat
    {
        public MagicResistPenetrationStat() : base() {}
        public MagicResistPenetrationStat(MagicResistPenetrationStat other) : base(other) {}
    }

    [Serializable]
    public class CooldownReductionStat : PercentageStat
    {
        public CooldownReductionStat() : base() {}
        public CooldownReductionStat(CooldownReductionStat other) : base(other) {}

        public float CalculateFinalCooldownValue(float baseCooldownValue)
        {
            return baseCooldownValue * (1.0f - GetStatValue());
        }
    }

    [Serializable]
    public class CriticalStrikeChanceStat : PercentageStat
    {
        public CriticalStrikeChanceStat() : base() {}
        public CriticalStrikeChanceStat(CriticalStrikeChanceStat other) : base(other) {}
    }
}
