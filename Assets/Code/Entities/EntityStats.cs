using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using Unity.VisualScripting;
using UnityEngine;

namespace Entity
{
    
// #region Stat Value
//     //We have to have wrappers for values because we cannot use arithmetic operations on generic T types. Once Unity
//     //upgrades to .Net 8, use generic type constraint (where T : INumber<T>) instead.
//     
//     public abstract class StatValueBase<T>
//     {
//         protected T m_Value;
//
//         public abstract void Add(StatValueBase<T> other);
//         public abstract void Subtract(StatValueBase<T> other);
//         public abstract void Multiply(StatValueBase<T> other);
//         public abstract void Divide(StatValueBase<T> other);
//         
//         public virtual T GetValue() { return m_Value; }
//     }
//
//     public class FloatStatValue : StatValueBase<float>
//     {
//         public FloatStatValue(float value)
//         {
//             m_Value = value;
//         }
//         
//         public override void Add(StatValueBase<float> other)
//         {
//             m_Value += other.GetValue();
//         }
//
//         public override void Subtract(StatValueBase<float> other)
//         {
//             m_Value -= other.GetValue();
//         }
//
//         public override void Multiply(StatValueBase<float> other)
//         {
//             m_Value *= other.GetValue();
//         }
//
//         public override void Divide(StatValueBase<float> other)
//         {
//             m_Value /= other.GetValue();
//         }
//     }
//
//     public class FloatPercentageStatValue : StatValueBase<float>
//     {
//         public FloatPercentageStatValue(float value)
//         {
//             if (value < 0.0f || value > 1.0f)
//             {
//                 throw new ArgumentOutOfRangeException(nameof(value), value, "Value must be between 0.0 and 1.0");
//                 return;
//             }
//             
//             m_Value = value;
//         }
//         
//         public override void Add(StatValueBase<float> other)
//         {
//             m_Value += other.GetValue();
//             ClampValue();
//         }
//
//         public override void Subtract(StatValueBase<float> other)
//         {
//             m_Value -= other.GetValue();
//             ClampValue();
//         }
//
//         public override void Multiply(StatValueBase<float> other)
//         {
//             m_Value *= other.GetValue();
//             ClampValue();
//         }
//
//         public override void Divide(StatValueBase<float> other)
//         {
//             m_Value += other.GetValue();
//             ClampValue();
//         }
//
//         protected void ClampValue()
//         {
//             m_Value = Mathf.Clamp(m_Value, 0.0f, 100.0f);
//         }
//     }
//
//     public class IntStatValue : StatValueBase<int>
//     {
//         public IntStatValue(int value)
//         {
//             m_Value = value;
//         }
//         
//         public override void Add(StatValueBase<int> other)
//         {
//             m_Value += other.GetValue();
//         }
//
//         public override void Subtract(StatValueBase<int> other)
//         {
//             m_Value -= other.GetValue();
//         }
//
//         public override void Multiply(StatValueBase<int> other)
//         {
//             m_Value *= other.GetValue();
//         }
//
//         public override void Divide(StatValueBase<int> other)
//         {
//             m_Value /= other.GetValue();
//         }
//     }
//
// #endregion

    
    
    
    public class EntityStatBase<T>
    {
        [SerializeField]
        protected T m_StatValue;
        public virtual T GetStatValue() { return m_StatValue; }
        public virtual void SetStatValue(T value) { m_StatValue = value; }
        public virtual void SetStatValue(EntityStatBase<T> stat) { m_StatValue = stat.GetStatValue(); }
        
        //Operators
        public static EntityStatBase<T> operator +(EntityStatBase<T> statA, EntityStatBase<T> statB)
        {
            EntityStatBase<T> newStat = new EntityStatBase<T>();
            dynamic statAValue = statA.GetStatValue();
            dynamic statBValue = statB.GetStatValue();

            newStat.SetStatValue(statAValue + statBValue);
            return newStat;
        }
        
        public static EntityStatBase<T> operator -(EntityStatBase<T> statA, EntityStatBase<T> statB)
        {
            EntityStatBase<T> newStat = new EntityStatBase<T>();
            dynamic statAValue = statA.GetStatValue();
            dynamic statBValue = statB.GetStatValue();

            newStat.SetStatValue(statAValue - statBValue);
            return newStat;
        }
        
        public static EntityStatBase<T> operator *(EntityStatBase<T> statA, EntityStatBase<T> statB)
        {
            EntityStatBase<T> newStat = new EntityStatBase<T>();
            dynamic statAValue = statA.GetStatValue();
            dynamic statBValue = statB.GetStatValue();

            newStat.SetStatValue(statAValue * statBValue);
            return newStat;
        }
        
        public static EntityStatBase<T> operator /(EntityStatBase<T> statA, EntityStatBase<T> statB)
        {
            EntityStatBase<T> newStat = new EntityStatBase<T>();
            dynamic statAValue = statA.GetStatValue();
            dynamic statBValue = statB.GetStatValue();

            newStat.SetStatValue(statAValue / statBValue);
            return newStat;
        }
    }

    public class PercentageStat : EntityStatBase<float>
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

        public override void SetStatValue(EntityStatBase<float> stat)
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
    public class HealthStat : EntityStatBase<float>
    {
        
    }

    [Serializable]
    public class ManaStat : EntityStatBase<float>
    {
        
    }

    [Serializable]
    public class MoveSpeedStat : EntityStatBase<float>
    {
        
    }

    [Serializable]
    public class ArmorStat : PercentageStat
    {
        public ArmorStat() : base() {}
        public ArmorStat(float value) : base(value) {}

        public override void SetStatValue(float value)
        {
            base.SetStatValue(value);
        }
    }

    [Serializable]
    public class MagicResistStat : PercentageStat
    {
        public MagicResistStat() : base() {}
        public MagicResistStat(float value) : base(value) {}
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
    public class AttackRangeStat : EntityStatBase<float>
    {
        
    }

    [Serializable]
    public class AttackDamageStat : EntityStatBase<float>
    {
        
    }

    [Serializable]
    public class AbilityPowerStat : EntityStatBase<float>
    {
        
    }

    [Serializable]
    public class AttackSpeedStat : EntityStatBase<float>
    {
        
    }

    [Serializable]
    public class SpellCooldownStat : PercentageStat
    {
        public SpellCooldownStat() : base() {}
        public SpellCooldownStat(float value) : base(value) {}
    }

    [Serializable]
    public class CriticalStrikeChanceStat : PercentageStat
    {
        public CriticalStrikeChanceStat() : base() {}
        public CriticalStrikeChanceStat(float value) : base(value) {}
    }
}
