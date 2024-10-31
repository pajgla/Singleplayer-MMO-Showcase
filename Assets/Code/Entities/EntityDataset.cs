using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    [CreateAssetMenu(fileName = "Entity Dataset", menuName = "Entity/New Entity Dataset", order = 1)]
    public class EntityDataset : ScriptableObject
    {
        //Basic champion information
        public string m_EntityName;
        public string m_EntityDescription;
        public string m_EntityAvatarPath;
        
        //Stats
        //Note that these values are starting stats, and do not represent current stat values for the champion
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
        
        //Other info
        public BasicAttackProjectile m_ProjectilePrefab;
        public float m_ProjectileSpeed;
    }
}