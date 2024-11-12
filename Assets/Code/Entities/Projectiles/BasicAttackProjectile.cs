using System;
using System.Collections;
using System.Collections.Generic;
using Entity.Abilities;
using UnityEngine;

namespace Entity
{
    public class BasicAttackProjectile : MonoBehaviour
    {
        [Tooltip("Distance between the target and the projectile at which the projectile will register a hit")]
        [SerializeField] protected float m_ProjectileHitDistance = 0.5f;
        
        protected EntityBase m_Owner;
        protected EntityBase m_Target;
        protected AbilityBase m_ParentAbility;

        public void Initialize(EntityBase owner, EntityBase target, AbilityBase parentAbility)
        {
            if (owner == null || target == null || parentAbility == null)
            {
                Debug.LogError("Either owner or target or parent ability are null. Please inspect if correct references are set. This projectile will be deleted.");
                Destroy(this.gameObject);
                return;
            }
            
            m_Owner = owner;
            m_Target = target;
            m_ParentAbility = parentAbility;
        }

        private void Update()
        {
            transform.LookAt(m_Target.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, m_Target.transform.position, Time.deltaTime * m_Owner.m_EntityDataset.m_ProjectileSpeed);

            if (Vector3.Distance(transform.position, m_Target.transform.position) < m_ProjectileHitDistance)
            {
                m_Target.GetHealthEntityComponent().TakeDamage(m_Target, m_Owner, m_ParentAbility);
                Destroy(this.gameObject);
            }
        }
    }
}
