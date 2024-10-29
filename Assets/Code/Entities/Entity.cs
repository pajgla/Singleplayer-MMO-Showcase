using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//An entity is anything that we can interact in the world. If it can be damaged/moved whatever, it should be an entity

namespace Entity
{
    public class Entity : MonoBehaviour
    {
        [SerializeField] private EntityDataset m_EntityDataset;
        
        protected HealthStat m_HealthStat = new HealthStat();
        protected ManaStat m_ManaStat = new ManaStat();
        protected MoveSpeedStat m_MoveSpeedStat = new MoveSpeedStat();
        protected ArmorStat m_ArmorStat = new ArmorStat();
        protected MagicResistStat m_MagicResistStat = new MagicResistStat();
        protected LifestealStat m_LifestealStat = new LifestealStat();
        protected SpellVampStat m_SpellVampStat = new SpellVampStat();
        protected AttackRangeStat m_AttackRangeStat = new AttackRangeStat();
        protected AttackDamageStat m_AttackDamageStat = new AttackDamageStat();
        protected AbilityPowerStat m_AbilityPowerStat = new AbilityPowerStat();
        protected AttackSpeedStat m_AttackSpeedStat = new AttackSpeedStat();
        protected SpellCooldownStat m_SpellCooldownStat = new SpellCooldownStat();
        protected CriticalStrikeChanceStat m_CriticalStrikeChanceStat = new CriticalStrikeChanceStat();

        private void Start()
        {
            if (m_EntityDataset == null)
            {
                Debug.LogError("Entity is missing Dataset");
                return;
            }
            
            InitializeStartingStatValues();
        }

        void InitializeStartingStatValues()
        {
            m_HealthStat.SetStatValue(m_EntityDataset.m_HealthStat);
            m_ManaStat.SetStatValue(m_EntityDataset.m_ManaStat);
            m_MoveSpeedStat.SetStatValue(m_EntityDataset.m_MoveSpeedStat);
            m_ArmorStat.SetStatValue(m_EntityDataset.m_ArmorStat);
            m_MagicResistStat.SetStatValue(m_EntityDataset.m_MagicResistStat);
            m_LifestealStat.SetStatValue(m_EntityDataset.m_LifestealStat);
            m_SpellVampStat.SetStatValue(m_EntityDataset.m_SpellVampStat);
            m_AttackRangeStat.SetStatValue(m_EntityDataset.m_AttackRangeStat);
            m_AttackDamageStat.SetStatValue(m_EntityDataset.m_AttackDamageStat);
            m_AbilityPowerStat.SetStatValue(m_EntityDataset.m_AbilityPowerStat);
            m_AttackSpeedStat.SetStatValue(m_EntityDataset.m_AttackSpeedStat);
            m_SpellCooldownStat.SetStatValue(m_EntityDataset.m_SpellCooldownStat);
            m_CriticalStrikeChanceStat.SetStatValue(m_EntityDataset.m_CriticalStrikeChanceStat);
        }
    }
}

