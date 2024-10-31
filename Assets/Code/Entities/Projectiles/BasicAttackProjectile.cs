using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public class BasicAttackProjectile : MonoBehaviour
    {
        private static float C_PROJECTILE_HIT_DISTANCE = 0.5f;
        
        protected EntityBase m_Owner;
        protected EntityBase m_Target;

        public void Initialize(EntityBase owner, EntityBase target)
        {
            if (owner == null || target == null)
            {
                Debug.LogError("Either owner or target are null. Please inspect if correct references are set. This projectile will be deleted.");
                Destroy(this.gameObject);
                return;
            }
            
            m_Owner = owner;
            m_Target = target;
        }

        private void Update()
        {
            transform.LookAt(m_Target.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, m_Target.transform.position, Time.deltaTime * m_Owner.m_EntityDataset.m_ProjectileSpeed);

            if (Vector3.Distance(transform.position, m_Target.transform.position) < C_PROJECTILE_HIT_DISTANCE)
            {
                //#TODO take damage
                Destroy(this.gameObject);
            }
        }
    }
}
