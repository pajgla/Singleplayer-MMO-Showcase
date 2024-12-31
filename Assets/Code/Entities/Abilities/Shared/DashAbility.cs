using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.Abilities
{
    [CreateAssetMenu(fileName = "Dash Ability", menuName = "Entity/Abilities/Dash Ability")]
    public class DashAbility : AbilityBase
    {
        Rigidbody m_OwnerRigidbodyComponent = null;

        [SerializeField]
        public override void Initialize(EntityBase ownerEntity)
        {
            base.Initialize(ownerEntity);
            
            m_OwnerRigidbodyComponent = ownerEntity.GetComponent<Rigidbody>();
        }

        private bool CanDash()
        {
            //#TODO Check entity state and if we can dash
            return true;
        }
        
        public override void PrecastAbility()
        {
            CastAbility();
        }

        public override void CastAbility()
        {
            m_OwnerRigidbodyComponent.AddForce(m_OwnerEntity.transform.forward * 1000.0f);
        }

        public override void CancelAbility()
        {
            m_OwnerRigidbodyComponent.velocity = Vector3.zero;
        }
    }
}
