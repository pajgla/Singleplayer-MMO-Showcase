using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.Abilities
{
    [System.Serializable]

    public class AbilityHolder
    {
        [SerializeField]
        AbilityBase m_Ability;
        
        [SerializeField]
        KeyCode m_TriggerKey = KeyCode.Q;

        public void Initialize(EntityBase owner)
        {
            m_Ability.Initialize(owner);
        }
        
        public void Update()
        {
            if (m_Ability == null)
            {
                Debug.LogError("Ability reference not set in AbilityHolder");
                return;
            }
            
            m_Ability.Update();
        }

        public AbilityBase GetAbility()
        {
            return m_Ability;
        }
        
        //Getters
        public KeyCode GetTriggerKeyCode()
        {
            return m_TriggerKey;
        }
    }
}
