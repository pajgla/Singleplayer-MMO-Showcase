using System.Collections;
using System.Collections.Generic;
using Entity.Abilities;
using Entity.Controllers;
using UnityEngine;

namespace Entity.Abilities
{
    [CreateAssetMenu(fileName = "New Basic Attack Ability", menuName = "Entity/Abilities/New Basic Attack Ability")]
    public class BasicAttackAbility : AbilityBase
    {
        //#TODO create a pool of projectiles
        [SerializeField] protected BasicAttackProjectile m_ProjectilePrefab;
        
        public override void PrecastAbility()
        {
            EntityBase target = TryGetTarget();
            if (target == null)
            {
                return;
            }

            AttackRangeStat attackRangeStat = m_OwnerEntity.GetEntityStat<AttackRangeStat>();
            if (attackRangeStat == null)
            {
                Debug.LogError($"Entity {target.m_EntityDataset?.m_EntityName} is missing AttackRangeStat");
                return;
            }

            IMovementEntityController movementEntityController = m_OwnerEntity.GetMovementEntityController();
            
            float distanceBetweenOwnerAndTarget = Vector3.Distance(m_OwnerEntity.transform.position, target.transform.position);
            if (distanceBetweenOwnerAndTarget <= attackRangeStat.GetStatValue())
            {
                movementEntityController?.StopMoving();
                CastAbility();
            }
            else
            {
                movementEntityController?.MoveTo(target.transform.position);
            }
        }

        public override void CastAbility()
        {
            if (!CanUseAbility())
            {
                return;
            }
            
            EntityBase target = TryGetTarget();
            if (target == null)
            {
                return;
            }
            
            AttackSpeedStat attackSpeedStat = m_OwnerEntity.GetEntityStat<AttackSpeedStat>();
            if (attackSpeedStat == null)
            {
                Debug.LogError($"Entity {m_OwnerEntity.m_EntityDataset?.m_EntityName} is missing AttackSpeedStat");
                return;
            }
            
            BasicAttackProjectile newProjectile = Instantiate(m_ProjectilePrefab);
            //#TODO spawn at projectile spawn location, not from the entity location
            newProjectile.transform.position = m_OwnerEntity.transform.position;
            newProjectile.Initialize(m_OwnerEntity, target, this);
            
            m_Cooldown = attackSpeedStat.CalculateCooldownBetweenAttacks(); 
            
            StartAbilityCooldown();
        }

        protected EntityBase TryGetTarget()
        {
            //Fetch target from PlayerControllerManager
            PlayerControlsManager playerControlsManager = PlayerControlsManager.Get();
            if (playerControlsManager == null)
            {
                Debug.LogError("PlayerControllerManager is null");
                return null;
            }

            return playerControlsManager.GetBasicAttackTarget();
        }

        public override void CancelAbility()
        {
            //Basic attack ability cannot cancel itself
            return;
        }
    }
}
