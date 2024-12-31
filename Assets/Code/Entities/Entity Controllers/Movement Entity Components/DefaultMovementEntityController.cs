using UnityEngine;
using UnityEngine.AI;

namespace Entity.Controllers
{
    public class DefaultMovementEntityController : IMovementEntityController
    {
        public EntityBase p_Owner { get; set; }
        private NavMeshAgent p_NavMeshAgentComponent { get; set; }

        public void Initialize(EntityBase ownerEntity)
        {
            p_Owner = ownerEntity;

            InitializeNavMeshComponent();
        }
        
        public void MoveTo(Vector3 worldPosition)
        {
            p_NavMeshAgentComponent.isStopped = false;
            p_NavMeshAgentComponent.SetDestination(worldPosition);
        }

        public void StopMoving()
        {
            p_NavMeshAgentComponent.isStopped = true;
        }

        public void Dash(Vector3 direction, float speed, float distance)
        {
            
        }

        public void Update()
        {
            
        }
        
        void InitializeNavMeshComponent()
        {
            MoveSpeedStat moveSpeedStat = p_Owner.GetEntityStat<MoveSpeedStat>();
            if (moveSpeedStat == null)
            {
                Debug.LogError("Entity is missing MoveSpeedStat");
                return;
            }

            NavMeshAgent agent = p_Owner.GetComponent<NavMeshAgent>();
            agent.speed = moveSpeedStat.GetStatValue();

            p_NavMeshAgentComponent = agent;
        }
    }
}
