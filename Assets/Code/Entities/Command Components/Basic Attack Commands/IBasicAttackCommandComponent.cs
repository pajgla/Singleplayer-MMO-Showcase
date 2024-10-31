using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public interface IBasicAttackCommandComponent
    {
        protected float m_CurrentAttackCooldown { get; set;}
        public void AttackTarget(EntityBase owner, EntityBase target);
        public void Update();
    }
}
