using UnityEngine;

namespace Entity
{
    public class DefaultBasicAttackCommand : IBasicAttackCommandComponent
    {
        private float m_CurrentAttackCooldown;

        float IBasicAttackCommandComponent.m_CurrentAttackCooldown
        {
            get => m_CurrentAttackCooldown;
            set => m_CurrentAttackCooldown = value;
        }

        public void AttackTarget(EntityBase owner, EntityBase target)
        {
            if (owner == null || target == null)
            {
                Debug.LogError("Either owner or the target are null. Please check if correct references are sent.");
                return;
            }
            
            if (owner == target)
            {
                Debug.LogError("Entity " + owner.m_EntityDataset.m_EntityName + " is trying to attack itself, which is forbidden." +
                               "If you really need this logic, consider creating a new Attack Command");
                
                return;
            }

            if (m_CurrentAttackCooldown > 0.0f)
            {
                //We cannot attack until cooldown expires
                return;
            }
            
            float distance = Vector3.Distance(owner.transform.position, target.transform.position);
            if (distance > owner.GetAttackRangeStat().GetStatValue())
            {
                owner.SetTarget(target);
                return;
            }

            Debug.Log("Attack!");
            SpawnProjectile(owner, target);
            StartAttackCooldown(owner);
        }

        protected void SpawnProjectile(EntityBase owner, EntityBase target)
        {
            BasicAttackProjectile projectilePrefab = owner.m_EntityDataset.m_ProjectilePrefab;
            Vector3 spawnPosition = owner.GetBasicAttackProjectileSpawnPosition().position;
            BasicAttackProjectile projectile = GameObject.Instantiate(projectilePrefab, spawnPosition, owner.transform.rotation);
            projectile.Initialize(owner, target);
        }

        protected void StartAttackCooldown(EntityBase owner)
        {
            float attackCooldown = 1.0f / owner.GetAttackSpeedStat().GetStatValue();
            m_CurrentAttackCooldown = attackCooldown;
        }

        public void Update()
        {
            if (m_CurrentAttackCooldown > 0.0f)
            {
                m_CurrentAttackCooldown -= Time.deltaTime;
            }
        }
    }
}
