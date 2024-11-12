using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.Abilities
{
    [RequireComponent(typeof(EntityBase))]
    public class EntityAbilitySystem : MonoBehaviour
    {
        [SerializeField] List<AbilityHolder> m_EntityAbilities = new List<AbilityHolder>();

        EntityBase m_Owner;

        void Awake()
        {
            m_Owner = GetComponent<EntityBase>();
        }

        void Update()
        {
            foreach (AbilityHolder abilityHolder in m_EntityAbilities)
            {
                abilityHolder.Update();
            }
        }

        public void PrecastAbility(KeyCode keycode)
        {
            foreach (AbilityHolder abilityHolder in m_EntityAbilities)
            {
                if (abilityHolder.GetTriggerKeyCode() == keycode)
                {
                    abilityHolder.GetAbility().PrecastAbility(m_Owner);
                    break;
                }
            }
        }
    }
}
