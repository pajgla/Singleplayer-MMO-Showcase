using System;
using TypeReferences;
using UnityEngine;
using UnityEngine.AI;

//Entity is anything that we can interact with in the world. If it can be damaged/moved whatever, it should be an entity

namespace Entity
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EntityBase : MonoBehaviour
    {
        [SerializeField] public EntityDataset m_EntityDataset;
        
        //Command components
        [ClassImplements(typeof(Components.IHealthEntityComponent)), SerializeField]
        private ClassTypeReference m_HealthEntityComponentPicker;
        
        private Components.IHealthEntityComponent m_HealthEntityComponent;
        
        //Stats
        //#TODO: Move into struct
        protected HealthStat m_HealthStat = new HealthStat();
        protected ManaStat m_ManaStat = new ManaStat();
        protected MoveSpeedStat m_MoveSpeedStat = new MoveSpeedStat();
        protected ArmorStat m_ArmorStat = new ArmorStat();
        protected MagicResistStat m_MagicResistStat = new MagicResistStat();
        protected ArmorPenetrationStat m_ArmorPenetrationStat = new ArmorPenetrationStat();
        protected MagicResistPenetrationStat m_MagicResistPenetrationStat = new MagicResistPenetrationStat();
        protected LifestealStat m_LifestealStat = new LifestealStat();
        protected SpellVampStat m_SpellVampStat = new SpellVampStat();
        protected AttackRangeStat m_AttackRangeStat = new AttackRangeStat();
        protected PhysicalPowerStat m_PhysicalPowerStat = new PhysicalPowerStat();
        protected AbilityPowerStat m_AbilityPowerStat = new AbilityPowerStat();
        protected AttackSpeedStat m_AttackSpeedStat = new AttackSpeedStat();
        protected CooldownReductionStat m_CooldownReductionStat = new CooldownReductionStat();
        protected CriticalStrikeChanceStat m_CriticalStrikeChanceStat = new CriticalStrikeChanceStat();

        private void Start()
        {
            if (m_EntityDataset == null)
            {
                Debug.LogError("Entity is missing Dataset");
                return;
            }
            
            InitializeStartingStatValues();
            InitializeNavMeshComponent();
            InitializeEntityComponents();
        }

        private void Update()
        {
        }

        private void InitializeEntityComponents()
        {
            if (m_HealthEntityComponentPicker != null)
            {
                m_HealthEntityComponent = Activator.CreateInstance(m_HealthEntityComponentPicker) as Components.IHealthEntityComponent;
            }
        }
        
        void InitializeStartingStatValues()
        {
            m_HealthStat.SetStatValue(m_EntityDataset.m_HealthStat);
            m_ManaStat.SetStatValue(m_EntityDataset.m_ManaStat);
            m_MoveSpeedStat.SetStatValue(m_EntityDataset.m_MoveSpeedStat);
            m_ArmorStat.SetStatValue(m_EntityDataset.m_ArmorStat);
            m_MagicResistStat.SetStatValue(m_EntityDataset.m_MagicResistStat);
            m_ArmorPenetrationStat.SetStatValue(m_EntityDataset.m_ArmorPenetrationStat);
            m_MagicResistPenetrationStat.SetStatValue(m_EntityDataset.m_MagicResistPenetrationStat);
            m_LifestealStat.SetStatValue(m_EntityDataset.m_LifestealStat);
            m_SpellVampStat.SetStatValue(m_EntityDataset.m_SpellVampStat);
            m_AttackRangeStat.SetStatValue(m_EntityDataset.m_AttackRangeStat);
            m_PhysicalPowerStat.SetStatValue(m_EntityDataset.m_PhysicalPowerStat);
            m_AbilityPowerStat.SetStatValue(m_EntityDataset.m_AbilityPowerStat);
            m_AttackSpeedStat.SetStatValue(m_EntityDataset.m_AttackSpeedStat);
            m_CooldownReductionStat.SetStatValue(m_EntityDataset.m_CooldownReductionStat);
            m_CriticalStrikeChanceStat.SetStatValue(m_EntityDataset.m_CriticalStrikeChanceStat);
        }

        void InitializeNavMeshComponent()
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.speed = m_MoveSpeedStat.GetStatValue();
        }

        public void GoToPosition(Vector3 position)
        {
            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(position);
        }

        public void StopMoving()
        {
            GetComponent<NavMeshAgent>().isStopped = true;
        }
        
        //Getters
        public AttackRangeStat GetAttackRangeStat()
        {
            return m_AttackRangeStat;
        }

        public AttackSpeedStat GetAttackSpeedStat()
        {
            return m_AttackSpeedStat;
        }

        public ArmorStat GetArmorStat()
        {
            return m_ArmorStat;
        }

        public MagicResistStat GetMagicResistStat()
        {
            return m_MagicResistStat;
        }

        public ArmorPenetrationStat GetArmorPenetrationStat()
        {
            return m_ArmorPenetrationStat;
        }

        public MagicResistPenetrationStat GetMagicResistPenetrationStat()
        {
            return m_MagicResistPenetrationStat;
        }

        public HealthStat GetHealthStat()
        {
            return m_HealthStat;
        }

        public CooldownReductionStat GetCooldownReductionStat()
        {
            return m_CooldownReductionStat;
        }

        public Components.IHealthEntityComponent GetHealthEntityComponent()
        {
            return m_HealthEntityComponent;
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

