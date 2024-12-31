using System;
using TypeReferences;
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
        
        //#TODO Encapsulate controllers
        [ClassImplements(typeof(Controllers.IHealthEntityController)), SerializeField]
        private ClassTypeReference m_HealthEntityControllerPicker;
        private Controllers.IHealthEntityController m_HealthEntityController;
        
        [ClassImplements(typeof(Controllers.IMovementEntityController)), SerializeField]
        private ClassTypeReference m_MovementEntityControllerPicker;
        private Controllers.IMovementEntityController m_MovementEntityController;
        
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
            InitializeEntityControllers();
        }

        private void InitializeEntityControllers()
        {
            if (m_HealthEntityControllerPicker.Type != null)
            {
                m_HealthEntityController = Activator.CreateInstance(m_HealthEntityControllerPicker) as Controllers.IHealthEntityController;
                m_HealthEntityController.Initialize(this);
            }
            
            if (m_MovementEntityControllerPicker.Type != null)
            {
                m_MovementEntityController = Activator.CreateInstance(m_MovementEntityControllerPicker) as Controllers.IMovementEntityController;
                m_MovementEntityController.Initialize(this);
            }
        }

        void Update()
        {
            m_HealthEntityController?.Update();
            m_MovementEntityController?.Update();
        }

        //Getters
        public Controllers.IHealthEntityController GetHealthEntityController()
        {
            return m_HealthEntityController;
        }

        public Controllers.IMovementEntityController GetMovementEntityController()
        {
            return m_MovementEntityController;
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

