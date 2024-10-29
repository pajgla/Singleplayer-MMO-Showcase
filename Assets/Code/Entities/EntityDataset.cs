using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    [CreateAssetMenu(fileName = "Entity Dataset", menuName = "Entity/New Entity Dataset", order = 1)]
    public class EntityDataset : ScriptableObject
    {
        public HealthStat m_HealthStat;
        public ManaStat m_ManaStat;
        public MoveSpeedStat m_MoveSpeedStat;
        public ArmorStat m_ArmorStat;
        public MagicResistStat m_MagicResistStat;
        public LifestealStat m_LifestealStat;
        public SpellVampStat m_SpellVampStat;
        public AttackRangeStat m_AttackRangeStat;
        public AttackDamageStat m_AttackDamageStat;
        public AbilityPowerStat m_AbilityPowerStat;
        public AttackSpeedStat m_AttackSpeedStat;
        public SpellCooldownStat m_SpellCooldownStat;
        public CriticalStrikeChanceStat m_CriticalStrikeChanceStat;
    }
}