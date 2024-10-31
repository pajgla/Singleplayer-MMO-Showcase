using System;
using System.Collections;
using System.Collections.Generic;
using TypeReferences;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

//Entity is anything that we can interact with in the world. If it can be damaged/moved whatever, it should be an entity

namespace Entity
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EntityBase : MonoBehaviour
    {
        [SerializeField] public EntityDataset m_EntityDataset;
        [SerializeField] protected Transform m_BasicAttackProjectileSpawnLocation;
        
        //Command components
        [ClassImplements(typeof(IBasicAttackCommandComponent)), SerializeField]
        private ClassTypeReference m_BasicAttackCommandComponentPicker;

        private IBasicAttackCommandComponent m_BasicAttackCommandComponent;
        
        
        //Basic info
        protected string m_EntityName;
        protected string m_EntityDescription;
        protected string m_EntityAvatarPath;
        
        //Stats
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
        
        //Other info
        protected float m_CurrentXP;
        protected float m_BasicAttackCooldown = 0.0f;

        //If we attack a target that's outside attack range, we need to come to the target first
        private EntityBase m_Target;

        private void Start()
        {
            if (m_EntityDataset == null)
            {
                Debug.LogError("Entity is missing Dataset");
                return;
            }
            
            InitializeStartingStatValues();
            InitializeBasicInfo();
            InitializeNavMeshComponent();
            InitializeCommandComponents();
        }

        private void Update()
        {
            if (m_Target)
            {
                FollowTarget();
            }
            
            //Update command components
            m_BasicAttackCommandComponent?.Update();
        }

        private void FollowTarget()
        {
            float distance = Vector3.Distance(transform.position, m_Target.transform.position);
            if (distance <= m_AttackRangeStat.GetStatValue())
            {
                StopMoving();
                m_BasicAttackCommandComponent.AttackTarget(this, m_Target);
            }
            else
            {
                GoToPosition(m_Target.transform.position, false);
            }
        }

        public void SetTarget(EntityBase target)
        {
            if (target == null)
            {
                Debug.LogError("Target is null");
                return;
            }
            
            m_Target = target;
        }

        private void InitializeCommandComponents()
        {
            if (m_BasicAttackCommandComponentPicker != null)
            {
                m_BasicAttackCommandComponent = Activator.CreateInstance(m_BasicAttackCommandComponentPicker) as IBasicAttackCommandComponent;
            }
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

        void InitializeBasicInfo()
        {
            m_EntityName = m_EntityDataset.m_EntityName;
            m_EntityDescription = m_EntityDataset.m_EntityDescription;
            m_EntityAvatarPath = m_EntityDataset.m_EntityAvatarPath;
        }

        void InitializeNavMeshComponent()
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.speed = m_MoveSpeedStat.GetStatValue();
        }

        public void GoToPosition(Vector3 position, bool removeTarget = true)
        {
            if (removeTarget)
                m_Target = null;
            
            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(position);
        }

        public void StopMoving()
        {
            GetComponent<NavMeshAgent>().isStopped = true;
        }

        public void BasicAttackTarget(EntityBase target)
        {
            if (target == null)
            {
                Debug.LogError("Target is null");
                return;
            }
            
            if (m_BasicAttackCommandComponent == null)
            {
                Debug.LogError("Basic attack command component is not selected for this entity");
                return;
            }
            
            m_BasicAttackCommandComponent.AttackTarget(this, target);
        }
        
        //Getters
        public AttackRangeStat GetAttackRangeStat()
        {
            return m_AttackRangeStat;
        }

        public Transform GetBasicAttackProjectileSpawnPosition()
        {
            return m_BasicAttackProjectileSpawnLocation;
        }

        public AttackSpeedStat GetAttackSpeedStat()
        {
            return m_AttackSpeedStat;
        }
        
#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_AttackRangeStat.GetStatValue());
        }
#endif
    }
}

