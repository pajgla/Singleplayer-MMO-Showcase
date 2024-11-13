using System.Collections;
using System.Collections.Generic;
using Entity.Abilities;
using UnityEngine;

namespace Entity.Abilities
{
    [CreateAssetMenu(fileName = "New Basic Attack Ability", menuName = "Entity/Abilities/New Basic Attack Ability")]
    public class BasicAttackAbility : AbilityBase
    {
        //#TODO create a pool of projectiles
        [SerializeField] protected BasicAttackProjectile m_ProjectilePrefab;
        
        public override void PrecastAbility(EntityBase abilityOwner)
        {
            EntityBase target = TryGetTarget();
            if (target == null)
            {
                return;
            }

            AttackRangeStat attackRangeStat = abilityOwner.GetEntityStat<AttackRangeStat>();
            if (attackRangeStat == null)
            {
                Debug.LogError($"Entity {target.m_EntityDataset?.m_EntityName} is missing AttackRangeStat");
                return;
            }
            
            float distanceBetweenOwnerAndTarget = Vector3.Distance(abilityOwner.transform.position, target.transform.position);
            if (distanceBetweenOwnerAndTarget <= attackRangeStat.GetStatValue())
            {
                abilityOwner.StopMoving();
                CastAbility(abilityOwner);
            }
            else
            {
                abilityOwner.GoToPosition(target.transform.position);
            }
        }

        public override void CastAbility(EntityBase abilityOwner)
        {
            if (!CanUseAbility(abilityOwner))
            {
                return;
            }
            
            EntityBase target = TryGetTarget();
            if (target == null)
            {
                return;
            }
            
            AttackSpeedStat attackSpeedStat = abilityOwner.GetEntityStat<AttackSpeedStat>();
            if (attackSpeedStat == null)
            {
                Debug.LogError($"Entity {abilityOwner.m_EntityDataset?.m_EntityName} is missing AttackSpeedStat");
                return;
            }
            
            BasicAttackProjectile newProjectile = Instantiate(m_ProjectilePrefab);
            //#TODO spawn at projectile spawn location, not from the entity location
            newProjectile.transform.position = abilityOwner.transform.position;
            newProjectile.Initialize(abilityOwner, target, this);
            
            m_Cooldown = attackSpeedStat.CalculateCooldownBetweenAttacks(); 
            
            StartAbilityCooldown(abilityOwner);
        }

        protected EntityBase TryGetTarget()
        {
            //Fetch target from PlayerControllerManager
            PlayerControllerManager playerControllerManager = PlayerControllerManager.Get();
            if (playerControllerManager == null)
            {
                Debug.LogError("PlayerControllerManager is null");
                return null;
            }

            return playerControllerManager.GetBasicAttackTarget();
        }

        public override void CancelAbility(EntityBase abilityOwner)
        {
            //Basic attack ability cannot cancel itself
            return;
        }
    }
}
