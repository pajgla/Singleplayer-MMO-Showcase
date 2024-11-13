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
        
        //Entity components
        [ClassImplements(typeof(Components.IHealthEntityComponent)), SerializeField]
        private ClassTypeReference m_HealthEntityComponentPicker;
        private Components.IHealthEntityComponent m_HealthEntityComponent;
        
        //Stats
        [SerializeField]
        StatsHolder m_StatsHolder = new StatsHolder();

        private void Start()
        {
            if (m_EntityDataset == null)
            {
                Debug.LogError("Entity is missing Dataset");
                return;
            }
            m_StatsHolder.InitializeStatsFromDataset(m_EntityDataset);
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

        void InitializeNavMeshComponent()
        {
            MoveSpeedStat moveSpeedStat = m_StatsHolder.GetStat<MoveSpeedStat>();
            if (moveSpeedStat == null)
            {
                Debug.LogError("Entity is missing MoveSpeedStat");
                return;
            }
            
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.speed = moveSpeedStat.GetStatValue();
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
        public Components.IHealthEntityComponent GetHealthEntityComponent()
        {
            return m_HealthEntityComponent;
        }

        public T GetEntityStat<T>() where T : EntityStatBase
        {
            if (m_StatsHolder == null)
            {
                Debug.LogError("Entity is missing StatsHolder");
                return null;
            }
            
            return m_StatsHolder.GetStat<T>();
        }
    }
}

